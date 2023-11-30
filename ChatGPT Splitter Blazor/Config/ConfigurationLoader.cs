using System.Reflection;
using Newtonsoft.Json;

namespace ChatGPT_Splitter_Blazor_New.Config;

public class ConfigurationLoader
{
    public static AppSettings? LoadEmbeddedAppSettings()
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