using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using OpenAI.Chat;
using OpenAI;
using System.Text.Json;
using animal_backend_domain.Dtos;
using animal_backend_domain.Types;

namespace animal_backend_api.Controllers;

public class ProductController(OpenAIClient? openAIClient, IWebHostEnvironment environment) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQuery query)
    {
        return Ok(await Mediator.Send(new GetAllProductsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdProductQuery(id)));
    }

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file type. Allowed: jpg, jpeg, png, gif, webp");

        // Validate file size (max 5MB)
        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File size exceeds 5MB limit");

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}{extension}";
        var uploadsFolder = Path.Combine(environment.ContentRootPath, "wwwroot", "uploads", "products");
        
        // Ensure directory exists
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the URL path that frontend can use
        var photoUrl = $"/uploads/products/{fileName}";
        
        return Ok(new { photoUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductInfoDto dto)
    {

        var command = new CreateProductCommand(
            dto.Name,
            dto.Type,
            dto.Description,
            dto.PhotoUrl,
            dto.Manufacturer
        );

        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
    {
        var updateCommand = new UpdateProductCommand(
            id,
            command.Name,
            command.Type,
            command.Description,
            command.PhotoUrl,
            command.Manufacturer
        );

        await Mediator.Send(updateCommand);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        if (openAIClient == null)
        {
            return BadRequest(new { reply = "AI funkcionalumas nėra prieinamas. OpenAI API raktas nesukonfigūruotas." });
        }

        var productTypes = string.Join(", ", Enum.GetNames(typeof(ProductType)));

        var chatClient = openAIClient.GetChatClient("gpt-4o");

        var getAllProductsTool = ChatTool.CreateFunctionTool(
            functionName: "get_all_products",
            functionDescription: "Get all products from the database",
            functionParameters: BinaryData.FromString("""{"type":"object","properties":{},"required":[]}""")
        );
        var systemPrompt = "Tu esi draugiškas veterinarijos produktų specialistas vardu pISPas." +
            "Atsakyk į visus naudotojo klausimus apie produktus." +
            "Jei nežinai atsakymo, pasakyk, kad nežinai." +
            "Jei reikia, naudok įrankį 'get_all_products', kad gautum visą produktų sąrašą iš duomenų bazės." +
            "Po to, kai gausi produktų sąrašą, pateik išsamų atsakymą naudotojui." +
            "Atsakymas turi būti lietuvių kalba. Naudok daug EMOJI" +
            "Jei prašo produktų pirmiau pasitikslink kokių produktų jis nori bei kokiam gyvūnui, o tik tada naudok įrankį." +
            "Kai rekomenduoji produktą, pateik nuorodą į produktą šiuo formatu: " +
            "[Produkto pavadinimas](http://localhost:5173/?product=PRODUKTO_ID), vietoj PRODUKTO_ID įrašyk produkto ID." +
            "SVARBU: Kai rodysi produkto nuotrauką, naudok PILNĄ URL su http://localhost:5068 priekyje. " +
            "Pvz.: jei PhotoUrl yra '/uploads/products/abc.jpg', rodyk kaip: ![Produkto pavadinimas](http://localhost:5068/uploads/products/abc.jpg)" +
            "Jei PhotoUrl prasideda su 'http', naudok jį tiesiogiai." +
            "Visada rodyk produkto nuotrauką kai rekomenduoji produktą!" +
            $"Galimi produktų tipai yra: {productTypes}.";

        // Build messages list with history
        var chatMessages = new List<ChatMessage>
        {
            ChatMessage.CreateSystemMessage(systemPrompt)
        };

        // Add conversation history (limit to last 10 messages to avoid token limits)
        if (request.History != null)
        {
            var recentHistory = request.History.TakeLast(10).ToList();
            foreach (var msg in recentHistory)
            {
                if (msg.Role == "user")
                    chatMessages.Add(ChatMessage.CreateUserMessage(msg.Content));
                else if (msg.Role == "assistant")
                    chatMessages.Add(ChatMessage.CreateAssistantMessage(msg.Content));
            }
        }

        // Add current message
        chatMessages.Add(ChatMessage.CreateUserMessage(request.Message));

        ChatCompletion initialResponse = await chatClient.CompleteChatAsync(
            messages: chatMessages,
            options: new ChatCompletionOptions
            {
                Tools = { getAllProductsTool },
                ToolChoice = ChatToolChoice.CreateAutoChoice()
            }
        );

        if (initialResponse.ToolCalls.Count == 0)
        {
            return Ok(new { reply = initialResponse.Content[0].Text });
        }

        var toolCall = initialResponse.ToolCalls[0];
        if (toolCall.FunctionName != "get_all_products")
        {
            return Ok(new { reply = "Unknown tool call" });
        }

        var products = await Mediator.Send(new GetAllProductsQuery());
        
        //Removing id for token limits and channg Type to string
        var productsList = products.Select(p => new
        {   
            Id = p.Id,
            Name = p.Name,
            Type = p.Type.ToString(),
            Description = p.Description,
            PhotoUrl = p.PhotoUrl,
            Manufacturer = p.Manufacturer
        }).ToList();

        var toolResultJson = JsonSerializer.Serialize(productsList);

        // Rebuild messages for tool response
        var toolMessages = new List<ChatMessage>(chatMessages)
        {
            ChatMessage.CreateAssistantMessage(initialResponse),
            ChatMessage.CreateToolMessage(toolCall.Id, toolResultJson)
        };

        ChatCompletion finalResponse = await chatClient.CompleteChatAsync(
            messages: toolMessages
        );

        var finalText = finalResponse.Content[0].Text;

        return Ok(new
        {
            reply = finalText,
            productsUsed = products.Count
        });
    }
}

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
    public List<ChatHistoryMessage>? History { get; set; }
}

public class ChatHistoryMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}