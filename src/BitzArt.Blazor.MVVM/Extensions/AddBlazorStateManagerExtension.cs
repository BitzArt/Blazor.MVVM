using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM;

public static class AddBlazorStateManagerExtension
{
    public static IServiceCollection AddBlazorStateManager(this IServiceCollection services)
    {
        services.AddSingleton<IStateManager, StateManager>();
        return services;
    }
}
