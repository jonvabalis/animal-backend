using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using OpenAI.Chat;
using OpenAI;
using System.Text.Json;
using System.ClientModel;
using animal_backend_domain.Dtos;

namespace animal_backend_api.Controllers;

public class ProductController : BaseController
{   
    private readonly OpenAIClient _openAIClient;
    public ProductController(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

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
    public async Task<IActionResult> Chat([FromBody] string message)
    {
        var chatClient = _openAIClient.GetChatClient("gpt-4o");

        var getAllProductsTool = ChatTool.CreateFunctionTool(
            functionName: "get_all_products",
            functionDescription: "Get all products from the database",
            functionParameters: BinaryData.FromString("""{"type":"object","properties":{},"required":[]}""")
        );
        var systemPrompt = "Tu esi draugiškas veterinarijos produktų specialistas vardu pISPpas." +
            "Atsakyk į visus naudotojo klausimus apie produktus.";

        ChatCompletion initialResponse = await chatClient.CompleteChatAsync(
            messages: new ChatMessage[]
            {   
                ChatMessage.CreateSystemMessage(systemPrompt),
                ChatMessage.CreateUserMessage(message)
            },
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
        var toolResultJson = JsonSerializer.Serialize(products);

        ChatCompletion finalResponse = await chatClient.CompleteChatAsync(
            messages: new ChatMessage[]
            {
                ChatMessage.CreateUserMessage(message),
                ChatMessage.CreateAssistantMessage(initialResponse),
                ChatMessage.CreateToolMessage(toolCall.Id, toolResultJson)
            }
        );

        var finalText = finalResponse.Content[0].Text;

        return Ok(new
        {
            reply = finalText,
            productsUsed = products.Count
        });
    }
}