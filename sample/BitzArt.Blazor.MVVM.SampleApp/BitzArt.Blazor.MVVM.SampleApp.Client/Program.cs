using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BitzArt.Blazor.MVVM.SampleApp.Client;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddRenderingEnvironment();
        builder.Services.AddBlazorViewModels();

        await builder.Build().RunAsync();
    }
}
