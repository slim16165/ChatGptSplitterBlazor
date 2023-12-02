using System.Diagnostics;
using OpenAI_API;
using Forge.OpenAI.Models.ChatCompletions;
using Forge.OpenAI.Models.Common;
using Forge.OpenAI.Interfaces.Services;
using ChatGPT_Splitter_Blazor_New.Config;
using Forge.OpenAI.Models.Models;

namespace ChatGPT_Splitter_Blazor_New.Bll;

public class ChatGptClient
{
    private OpenAIAPI _api;
    private readonly IOpenAIService _openAiService;
    private string _apiKey;

    public ChatGptClient(IOpenAIService openAiService)
    {
        // Due to the nature of Blazor WebAssembly (similar to JS), traditional server-side configuration files are not accessible.
        // Blazor WebAssembly, in fact, run entirely within the client's browser. As a result, I've embedded the configuration as a 
        // resource within the assembly to allow for its secure retrieval and usage without exposing sensitive details on public repositories (github).
        _openAiService = openAiService;

        // Carica il token API da una configurazione statica se disponibile
        var settings = ConfigurationLoader.LoadEmbeddedAppSettings();
        _apiKey = settings?.OpenAI.ApiKey;

        // Inizializza il client OpenAI se il token è disponibile
        if (!string.IsNullOrEmpty(_apiKey))
        {
            _api = new OpenAIAPI(_apiKey);
        }
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

    public async IAsyncEnumerable<Model> GetAvailableModels()
    {
        // Utilizza il servizio Forge.OpenAI per ottenere i modelli disponibili
        HttpOperationResult<ModelsResponse> response = await _openAiService.ModelService.GetAsync(CancellationToken.None);

        if (!response.IsSuccess || response.Result == null || !response.Result.Models.Any())
        {
            Console.WriteLine("Errore durante l'ottenimento della lista dei modelli. " + response.ErrorMessage);
            yield return null;
        }

        Debug.Assert(response.Result != null, "response.Result != null");
        foreach (var model in response.Result.Models)
        {
            yield return model;
        }
    }

    public void UpdateToken(string apiToken)
    {
        if (string.IsNullOrEmpty(apiToken) || apiToken == _apiKey) return;

        _apiKey = apiToken;
        _api = new OpenAIAPI(_apiKey);
    }
}