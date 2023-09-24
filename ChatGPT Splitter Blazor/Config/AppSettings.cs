using Newtonsoft.Json;

namespace ChatGPT_Splitter_Blazor_New.Config;

public class AppSettings
{
    public OpenAISettings OpenAI { get; set; }

    public static AppSettings Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Config file not found: {path}");

        var content = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<AppSettings>(content);
    }
}