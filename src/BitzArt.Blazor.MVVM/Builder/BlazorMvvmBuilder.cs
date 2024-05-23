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

    public ViewModelFactory ViewModelFactory { get; set; }

    IViewModelFactory IBlazorMvvmBuilder.Factory => ViewModelFactory;

    public BlazorMvvmBuilder(
        IServiceCollection serviceCollection,
        ViewModelFactory factory)
    {
        ServiceCollection = serviceCollection;
        ViewModelFactory = factory;

        if (ServiceCollection.Any(x => x.ServiceType == typeof(IViewModelFactory)))
            throw new InvalidOperationException("IViewModelFactory is already registered in this ServiceCollection.");

        ServiceCollection.AddSingleton(ViewModelFactory);
        ServiceCollection.AddSingleton<IViewModelFactory>(ViewModelFactory);

        ServiceCollection.AddScoped<PageStateDictionaryContainer>();
    }
}
