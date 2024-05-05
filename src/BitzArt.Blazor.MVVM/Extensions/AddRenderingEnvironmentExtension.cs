using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BitzArt.Blazor.MVVM;

public static class AddRenderingEnvironmentExtension
{
    public static IServiceCollection AddRenderingEnvironment(this IServiceCollection services)
    {
        services.AddScoped(x => x.GetRenderingEnvironment());

        return services;
    }

    private static RenderingEnvironment GetRenderingEnvironment(this IServiceProvider serviceProvider)
    {
        var isBrowser = OperatingSystem.IsBrowser();
        var isServer = !isBrowser;

        var isPrerender = isServer && serviceProvider.GetIsPrerender();

        return new()
        {
            IsServer = !isBrowser,
            IsClient = isBrowser,
            IsPrerender = isPrerender
        };
    }

    private static bool GetIsPrerender(this IServiceProvider serviceProvider)
    {
        var jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();

        var JSRuntimeType = jsRuntime.GetType();
        if (JSRuntimeType.Name != "RemoteJSRuntime") return false;

        var IsInitializedProperty = jsRuntime.GetType().GetProperty("IsInitialized");
        var isInitialized = IsInitializedProperty?.GetValue(jsRuntime);

        return isInitialized is not true;
    }
}
