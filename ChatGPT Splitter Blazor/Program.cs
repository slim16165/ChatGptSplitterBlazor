using Serilog;
using ChatGPT_Splitter_Blazor_New;
using Forge.OpenAI;
using Forge.OpenAI.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configuration for Serilog
//var environmentName = builder.HostEnvironment.Environment;
//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("appsettings.json")
//    //.AddJsonFile($"appsettings.{environmentName}.json", true)
//    .Build();

Log.Logger = new LoggerConfiguration()
    //.ReadFrom.Configuration(configuration)
    //.WriteTo.BrowserConsole()  // Log to browser console
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    var settings = ConfigurationLoader.LoadEmbeddedAppSettings();
    // Add the ForgeOpenAI service
    builder.Services.AddForgeOpenAI(options =>
    {
        options.AuthenticationInfo = new AuthenticationInfo(settings.OpenAI.ApiKey);
    });
    builder.Services.AddScoped<ChatGptClient>();


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