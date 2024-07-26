using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Dto;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Services.OpenAI;
using OpenAI_API;
using OpenAI_API.Chat;
using System.Diagnostics;

namespace MyRecipeBook.Infrastructure.Services.OpenAI;

public class ChatGPTService : IGenerateRecipeAI
{
    private const string CHAT_MODEL = "gpt-3.5-turbo";

    private readonly IOpenAIAPI _openAIAPI;

    public ChatGPTService(IOpenAIAPI openAIAPI)
    {
        _openAIAPI = openAIAPI;
    }

    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        var conversation = _openAIAPI.Chat.CreateConversation( new ChatRequest { Model = CHAT_MODEL } );

        conversation.AppendSystemMessage(ResourceOpenAI.STARTING_GENERATE_RECIPE);

        conversation.AppendUserInput(string.Join(";", ingredients));

        var response = await conversation.GetResponseFromChatbotAsync();

        var responseList = response
            .Split("\n")
            .Where(reposne => !response.Trim().Equals(string.Empty))
            .Select(item => item.Replace("[", "").Replace("]", ""))
            .ToList();

        var step = 1;
        return new GeneratedRecipeDto
        {
            Title = responseList[0],
            CookingTime = (CookingTime)Enum.Parse(typeof(CookingTime), responseList[1]),
            Ingredients = responseList[2].Split(";"),
            Instructions = responseList[3].Split("@").Select(instruction => new GeneratedInstructionsDto
            {
                Text = instruction.Trim(),
                Step = step++
            }).ToList()

        };
    }
}
