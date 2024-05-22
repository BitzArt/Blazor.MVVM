using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM;

public interface IBlazorMvvmBuilder
{
    public IServiceCollection ServiceCollection { get; }

    internal IViewModelFactory Factory { get; }
}

internal class BlazorMvvmBuilder : IBlazorMvvmBuilder
{
    public IServiceCollection ServiceCollection { get; set; }

    public IViewModelFactory Factory { get; set; }

    public BlazorMvvmBuilder(
        IServiceCollection serviceCollection,
        IViewModelFactory factory)
    {
        ServiceCollection = serviceCollection;
        Factory = factory;

        if (ServiceCollection.Any(x => x.ServiceType == typeof(IViewModelFactory)))
            throw new InvalidOperationException("IViewModelFactory is already registered in this ServiceCollection.");

        ServiceCollection.AddSingleton(Factory);
        ServiceCollection.AddSingleton<BlazorViewModelStateManager>();
        ServiceCollection.AddScoped<PageStateDictionaryContainer>();
    }
}
