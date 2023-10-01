using ChatGPT_Splitter_Blazor_New.Config;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Reflection;
using Forge.OpenAI.Models.ChatCompletions;
using Forge.OpenAI.Models.Common;
using ChatMessage = Forge.OpenAI.Models.ChatCompletions.ChatMessage;
using Forge.OpenAI.Interfaces.Services;

namespace ChatGPT_Splitter_Blazor_New;

public class ConfigurationLoader
{
    public static AppSettings LoadEmbeddedAppSettings()
    {
        const string resourceName = "ChatGPT_Splitter_Blazor_New.appsettings.json";
        string content = ReadEmbeddedResourceContent(resourceName);
        return JsonConvert.DeserializeObject<AppSettings>(content);
    }

    private static string ReadEmbeddedResourceContent(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        using StreamReader reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}

public class ChatGptClient
{
    private readonly OpenAIAPI _api;
    private readonly IOpenAIService _openAiService;
    private readonly string _apiKey;

    public ChatGptClient(IOpenAIService openAiService)
    {
        // Due to the nature of Blazor WebAssembly (similar to JS), traditional server-side configuration files are not accessible.
        // Blazor WebAssembly, in fact, run entirely within the client's browser. As a result, I've embedded the configuration as a 
        // resource within the assembly to allow for its secure retrieval and usage without exposing sensitive details on public repositories (github).

        //var config = AppSettings.Load("appsettings.json");
        var settings = ConfigurationLoader.LoadEmbeddedAppSettings();
        _apiKey = settings.OpenAI.ApiKey;
        
        _api = new OpenAIAPI(_apiKey);

        _openAiService = openAiService;
    }

    public async Task<string> GenerateTextAsync(string userInput, string istruzioni, Model model)
    {
        //https://github.com/OkGoDoIt/OpenAI-API-dotnet#chat-conversations
        ChatRequest config = new ChatRequest(){Model = model};
        var chat = _api.Chat.CreateConversation(config);

        /// give instruction as System
        chat.AppendSystemMessage(istruzioni);

        // give a few examples as user and assistant
        chat.AppendUserInput(userInput);

        // and get the response
        string response = await chat.GetResponseFromChatbotAsync();

        return response;
    }

    public async Task<string> GenerateTextWithForgeAsync(string userInput, string istruzioni, Model model)
    {
        // Crea la richiesta iniziale
        ChatCompletionRequest request = new ChatCompletionRequest(ChatMessage.CreateFromSystem(istruzioni), model);
        request.Messages.Add(ChatMessage.CreateFromUser(userInput));

        // Non sono sicuro di come tu stia gestendo 'Model' nel tuo codice attuale, 
        // ma potresti doverlo incorporare nella richiesta qui, se necessario.

        // Effettua la richiesta e ottieni la risposta
        HttpOperationResult<ChatCompletionResponse> response =
            await _openAiService.ChatCompletionService.GetAsync(request, CancellationToken.None).ConfigureAwait(false);

        if (response.IsSuccess && response.Result != null && response.Result.Choices.Any())
        {
            return response.Result.Choices[0].Message.Content;
        }
        else
        {
            // Gestisci l'errore come preferisci
            return "Errore durante l'ottenimento della risposta. " + response.ErrorMessage;
        }
    }


}