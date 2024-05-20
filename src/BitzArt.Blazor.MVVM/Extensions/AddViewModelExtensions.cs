using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Extension methods for adding view models to <see cref="IViewModelFactory"/>.
/// </summary>
public static class AddViewModelExtensions
{
    /// <summary>
    /// Adds all view models from all loaded assemblies to <see cref="IViewModelFactory"/>.
    /// </summary>
    public static IBlazorMvvmBuilder AddViewModels(this IBlazorMvvmBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies) builder.AddViewModelsFromAssembly(assembly);
        return builder;
    }

    /// <summary>
    /// Adds all view models from the assembly containing the specified type to <see cref="IViewModelFactory"/>.
    /// </summary>
    public static IBlazorMvvmBuilder AddViewModelsFromAssemblyContaining(this IBlazorMvvmBuilder builder, Type type)
    {
        return builder.AddViewModelsFromAssembly(type.Assembly);
    }

    /// <summary>
    /// Adds all view models from the assembly containing the specified type to <see cref="IViewModelFactory"/>.
    /// </summary>
    public static IBlazorMvvmBuilder AddViewModelsFromAssemblyContaining<T>(this IBlazorMvvmBuilder builder)
    {
        return builder.AddViewModelsFromAssembly(typeof(T).Assembly);
    }

    /// <summary>
    /// Adds all view models from the specified assembly to <see cref="IViewModelFactory"/>.
    /// </summary>
    public static IBlazorMvvmBuilder AddViewModelsFromAssembly(this IBlazorMvvmBuilder builder, Assembly assembly)
    {
        var viewModelTypes = assembly
            .GetTypes()
            .Where(t => t.IsPublic)
            .Where(t => !t.IsAbstract)
            .Where(t => t.IsSubclassOf(typeof(ViewModel)));

        foreach (var viewModelType in viewModelTypes) builder.AddViewModel(viewModelType);

        return builder;
    }

    /// <summary>
    /// Adds specified view models to <see cref="IViewModelFactory"/>.
    /// </summary>
    public static IBlazorMvvmBuilder AddViewModels(this IBlazorMvvmBuilder builder, IEnumerable<Type> viewModelTypes)
    {
        foreach (var viewModelType in viewModelTypes) builder.AddViewModel(viewModelType);
        return builder;
    }

    /// <summary>
    /// Adds a view model to <see cref="IViewModelFactory"/>.
    /// </summary>
    public static IBlazorMvvmBuilder AddViewModel(this IBlazorMvvmBuilder builder, Type viewModelType)
    {
        var registrationKey = viewModelType.FullName!;

        builder.Factory.AddViewModel(viewModelType, registrationKey); // Add to factory
        builder.ServiceCollection.AddKeyedTransient(typeof(ViewModel), registrationKey, viewModelType); // Add to DI

        builder.ServiceCollection.AddTransient(viewModelType, serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<IViewModelFactory>();
            return factory.Create(serviceProvider, viewModelType);
        });

        return builder;
    }

    /// <summary>
    /// Adds a view model to <see cref="IViewModelFactory"/>.
    /// </summary>
    public static IBlazorMvvmBuilder AddViewModel<TViewModel>(this IBlazorMvvmBuilder builder)
        where TViewModel : ViewModel
    {
        var registrationKey = typeof(TViewModel).FullName!;

        builder.Factory.AddViewModel(typeof(TViewModel), registrationKey); // Add to factory
        builder.ServiceCollection.AddKeyedTransient(typeof(ViewModel), registrationKey, typeof(TViewModel)); // Add to DI

        builder.ServiceCollection.AddTransient(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<IViewModelFactory>();
            return factory.Create<TViewModel>(serviceProvider);
        });

        return builder;
    }
}
