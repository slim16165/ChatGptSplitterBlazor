using ChatGPT_Splitter_Blazor_New.Config;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace ChatGPT_Splitter_Blazor_New;

public class ChatGptClient
{
    private readonly OpenAIAPI api;

    public ChatGptClient()
    {
        var config = AppSettings.Load("appsettings.json");
        string OPENAI_API_KEY = config.OpenAI.ApiKey;
        api = new OpenAIAPI(OPENAI_API_KEY);
    }

    public async Task<string> GenerateTextAsync(string userInput, string istruzioni, Model model)
    {
        //https://github.com/OkGoDoIt/OpenAI-API-dotnet#chat-conversations
        ChatRequest config = new ChatRequest(){Model = model};
        var chat = api.Chat.CreateConversation(config);

        /// give instruction as System
        chat.AppendSystemMessage(istruzioni);

        // give a few examples as user and assistant
        chat.AppendUserInput(userInput);

        // and get the response
        string response = await chat.GetResponseFromChatbotAsync();

        return response;
    }
}