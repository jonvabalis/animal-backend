using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using OpenAI.Chat;
using OpenAI;
using System.Text.Json;
using System.ClientModel;

namespace animal_backend_api.Controllers;

public class ProductController : BaseController
{   
    private readonly OpenAIClient _openAIClient;
    public ProductController(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllProductsQuery()));
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
        
        ChatCompletion initialResponse = await chatClient.CompleteChatAsync(
            messages: new ChatMessage[]
            {
                ChatMessage.CreateUserMessage(message)
            },
            options: new ChatCompletionOptions
            {
                Tools = { getAllProductsTool },
                ToolChoice = ChatToolChoice.CreateAutoChoice()
            }
        );

        // Use the response object directly - it has Content, ToolCalls, etc
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

        return Ok(new {
            reply = finalText,
            productsUsed = products.Count
        });
    }
}