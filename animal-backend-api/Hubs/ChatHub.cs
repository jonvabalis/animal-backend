using Microsoft.AspNetCore.SignalR;
using OpenAI;
using OpenAI.Chat;
using System.Text.Json;
using animal_backend_core.Queries;
using MediatR;
using animal_backend_domain.Types;

namespace animal_backend_api.Hubs;

public class ChatHub : Hub
{
    private readonly OpenAIClient? _openAIClient;
    private readonly IMediator _mediator;

    public ChatHub(OpenAIClient? openAIClient, IMediator mediator)
    {
        _openAIClient = openAIClient;
        _mediator = mediator;
    }

    public async Task SendMessage(string message, List<ChatHistoryItem>? history)
    {
        var chatClient = _openAIClient.GetChatClient("gpt-4o");

        var getAllProductsTool = ChatTool.CreateFunctionTool(
            functionName: "get_all_products",
            functionDescription: "Get all products from the database",
            functionParameters: BinaryData.FromString("""{"type":"object","properties":{},"required":[]}""")
        );

        var productTypes = string.Join(", ", Enum.GetNames(typeof(ProductType)));

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

        var messages = new List<ChatMessage>
        {
            ChatMessage.CreateSystemMessage(systemPrompt)
        };

        // Add history
        if (history != null)
        {
            foreach (var item in history.TakeLast(10))
            {
                if (item.Role == "user")
                    messages.Add(ChatMessage.CreateUserMessage(item.Content));
                else if (item.Role == "assistant")
                    messages.Add(ChatMessage.CreateAssistantMessage(item.Content));
            }
        }

        messages.Add(ChatMessage.CreateUserMessage(message));

        var options = new ChatCompletionOptions
        {
            Tools = { getAllProductsTool },
            ToolChoice = ChatToolChoice.CreateAutoChoice()
        };

        // Send "typing" status
        await Clients.Caller.SendAsync("ReceiveTyping", true);

        try
        {
            var response = await chatClient.CompleteChatAsync(messages, options);

            if (response.Value.ToolCalls.Count > 0)
            {
                var toolCall = response.Value.ToolCalls[0];
                if (toolCall.FunctionName == "get_all_products")
                {
                    var products = await _mediator.Send(new GetAllProductsQuery());
                    var productsForChat = products.Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Type = p.Type.ToString(),
                        Description = p.Description,
                        PhotoUrl = p.PhotoUrl,
                        Manufacturer = p.Manufacturer
                    }).ToList();

                    var toolResultJson = JsonSerializer.Serialize(productsForChat);

                    messages.Add(ChatMessage.CreateAssistantMessage(response.Value));
                    messages.Add(ChatMessage.CreateToolMessage(toolCall.Id, toolResultJson));

                    // Stream the final response
                    await StreamResponse(chatClient, messages);
                    return;
                }
            }

            // Stream regular response
            await StreamResponse(chatClient, messages);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ReceiveTyping", false);
            await Clients.Caller.SendAsync("ReceiveMessageComplete", $"Atsiprašau, įvyko klaida: {ex.Message}");
        }
    }

    private async Task StreamResponse(ChatClient chatClient, List<ChatMessage> messages)
    {
        var fullResponse = "";

        await foreach (var update in chatClient.CompleteChatStreamingAsync(messages))
        {
            foreach (var contentPart in update.ContentUpdate)
            {
                fullResponse += contentPart.Text;
                await Clients.Caller.SendAsync("ReceiveMessageChunk", contentPart.Text);
            }
        }

        await Clients.Caller.SendAsync("ReceiveTyping", false);
        await Clients.Caller.SendAsync("ReceiveMessageComplete", fullResponse);
    }
}

public class ChatHistoryItem
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
