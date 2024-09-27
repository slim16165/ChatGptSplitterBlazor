using Serilog;
using ChatGPT_Splitter_Blazor_New;
using Forge.OpenAI;
using Forge.OpenAI.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatGPT_Splitter_Blazor_New.Config;
using ChatGPT_Splitter_Blazor_New.Bll;
using ChatGPT_Splitter_Blazor_New.Pages.TextComparer.Services;
using Blazored.LocalStorage;
using ChatGPT_Splitter_Blazor_New.TextComparer.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

Log.Logger = new LoggerConfiguration()
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    builder.Services.AddScoped(sp => new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
        Timeout = TimeSpan.FromMinutes(2) // Aumenta il timeout a 2 minuti
    });
    var settings = ConfigurationLoader.LoadEmbeddedAppSettings();
    // Add the ForgeOpenAI service
    builder.Services.AddForgeOpenAI(options =>
    {
        options.AuthenticationInfo = new AuthenticationInfo(settings.OpenAI.ApiKey);
    });
    builder.Services.AddScoped<ChatGptClient>();
    builder.Services.AddScoped<IStorageService, StorageService>();
    builder.Services.AddBlazoredLocalStorage();
    builder.Services.AddScoped<IControllerService, ControllerService>();



    //builder.WebHost.ConfigureKestrel(serverOptions =>
    //{
    //    serverOptions.ListenAnyIP(5108); // Configura Kestrel per ascoltare sulla porta 5108
    //});

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    throw;  // Consider whether you want to propagate the exception or handle it.
}
finally
{
    Log.CloseAndFlush();
}