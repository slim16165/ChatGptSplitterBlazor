using System.Reflection;
using Newtonsoft.Json;

namespace ChatGPT_Splitter_Blazor_New.Config;

public class ConfigurationLoader
{
    public static AppSettings? LoadEmbeddedAppSettings()
    {
        string resourceName = "ChatGPT_Splitter_Blazor_New.appsettings.json";
        string localResourceName = "ChatGPT_Splitter_Blazor_New.appsettings.local.json";

        // Verifica se il file di configurazione locale esiste
        if (ResourceExists(localResourceName))
        {
            resourceName = localResourceName;
        }

        string content = ReadEmbeddedResourceContent(resourceName);
        return JsonConvert.DeserializeObject<AppSettings>(content);
    }

    private static bool ResourceExists(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceNames().Any(name => name.Equals(resourceName));
    }

    private static string ReadEmbeddedResourceContent(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        using StreamReader reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}