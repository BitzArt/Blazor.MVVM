using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM;

public interface IViewModelFactory
{
    public void AddViewModel(Type viewModelType, string registrationKey);

    public ViewModel Create(IServiceProvider serviceProvider, Type viewModelType);
    public TViewModel Create<TViewModel>(IServiceProvider serviceProvider) where TViewModel : ViewModel;
}

internal class ViewModelFactory : IViewModelFactory
{
    private Dictionary<Type, ViewModelInjectionMap> InjectionMaps { get; set; }

    public ViewModelFactory()
    {
        InjectionMaps = [];
    }

    public void AddViewModel(Type viewModelType, string registrationKey)
    {
        if (!typeof(ViewModel).IsAssignableFrom(viewModelType)) throw new InvalidOperationException(
            $"Type {viewModelType.Name} is not a ViewModel.");

        if (InjectionMaps.ContainsKey(viewModelType))
            throw new InvalidOperationException(
                               $"ViewModel {viewModelType.Name} is already registered in the factory.");

        InjectionMaps.Add(viewModelType, new(viewModelType, registrationKey));
    }

    public TViewModel Create<TViewModel>(IServiceProvider serviceProvider)
        where TViewModel : ViewModel
        => (TViewModel)Create(serviceProvider, typeof(TViewModel));

    public ViewModel Create(IServiceProvider serviceProvider, Type viewModelType)
    {
        var viewModelMap = InjectionMaps.GetValueOrDefault(viewModelType)
            ?? throw new InvalidOperationException($"ViewModel {viewModelType.Name} is not registered in the factory.");

        var viewModel = (ViewModel)serviceProvider.GetRequiredKeyedService(typeof(ViewModel), viewModelMap.RegistrationKey);
        foreach (var injection in viewModelMap.Injections)
        {
            var injectedViewModel = Create(serviceProvider, injection.ViewModelType);
            injection.Property.SetValue(viewModel, injectedViewModel);
        }
        viewModel.OnDependenciesInjected();
        return viewModel;
    }

    public ViewModelInjectionMap GetInjectionMap(Type viewModelType)
    {
        var injectionMap = InjectionMaps.GetValueOrDefault(viewModelType)
            ?? throw new InvalidOperationException($"ViewModel {viewModelType.Name} is not registered in the factory.");

        return injectionMap;
    }
}
