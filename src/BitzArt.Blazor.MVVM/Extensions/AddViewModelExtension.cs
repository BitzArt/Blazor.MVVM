using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Extension methods for adding view models to the service collection.
/// </summary>
public static class AddViewModelExtension
{
    /// <summary>
    /// Adds all view models from all loaded assemblies to the service collection.
    /// </summary>
    public static IServiceCollection AddBlazorViewModels(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies) services.AddBlazorViewModelsFromAssembly(assembly);
        return services;
    }

    /// <summary>
    /// Adds all view models from the assembly containing the specified type to the service collection.
    /// </summary>
    public static IServiceCollection AddBlazorViewModelsFromAssemblyContaining(this IServiceCollection services, Type type)
    {
        return services.AddBlazorViewModelsFromAssembly(type.Assembly);
    }

    /// <summary>
    /// Adds all view models from the assembly containing the specified type to the service collection.
    /// </summary>
    public static IServiceCollection AddBlazorViewModelsFromAssemblyContaining<T>(this IServiceCollection services)
    {
        return services.AddBlazorViewModelsFromAssembly(typeof(T).Assembly);
    }

    /// <summary>
    /// Adds all view models from the specified assembly to the service collection.
    /// </summary>
    public static IServiceCollection AddBlazorViewModelsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var viewModelTypes = assembly
            .GetTypes()
            .Where(t => t.IsPublic)
            .Where(t => !t.IsAbstract)
            .Where(t => t.IsSubclassOf(typeof(ViewModel)));

        foreach (var viewModelType in viewModelTypes) services.AddTransient(viewModelType);

        return services;
    }

    /// <summary>
    /// Adds a view model to the service collection.
    /// </summary>
    public static IServiceCollection AddBlazorViewModel(this IServiceCollection services, Type viewModelType)
    {
        services.AddTransient(viewModelType);
        return services;
    }

    /// <summary>
    /// Adds a view model to the service collection.
    /// </summary>
    public static IServiceCollection AddBlazorViewModel<TViewModel>(this IServiceCollection services)
        where TViewModel : ViewModel
    {
        services.AddTransient<TViewModel>();
        return services;
    }
}
