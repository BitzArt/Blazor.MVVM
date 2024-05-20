using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BitzArt.Blazor.MVVM;

public interface IViewModelFactory
{
    public void AddViewModel(Type viewModelType, string registrationKey);

    public ViewModel Create(IServiceProvider serviceProvider, Type viewModelType);
    public TViewModel Create<TViewModel>(IServiceProvider serviceProvider) where TViewModel : ViewModel;

    public IEnumerable<PropertyInfo> GetNestedViewModelProperties(Type viewModelType);
}

internal class ViewModelFactory : IViewModelFactory
{
    public ICollection<ViewModelInjectionMap> InjectionMaps { get; set; }

    public ViewModelFactory()
    {
        InjectionMaps = [];
    }

    public void AddViewModel(Type viewModelType, string registrationKey)
    {
        if (!typeof(ViewModel).IsAssignableFrom(viewModelType)) throw new InvalidOperationException(
            $"Type {viewModelType.Name} is not a ViewModel.");

        if (InjectionMaps.Any(x => x.ViewModelType == viewModelType))
            throw new InvalidOperationException(
                $"ViewModel {viewModelType.Name} is already registered in the factory.");

        InjectionMaps.Add(new(viewModelType, registrationKey));
    }

    public TViewModel Create<TViewModel>(IServiceProvider serviceProvider)
        where TViewModel : ViewModel
        => (TViewModel)Create(serviceProvider, typeof(TViewModel));

    public ViewModel Create(IServiceProvider serviceProvider, Type viewModelType)
    {
        var viewModelMap = InjectionMaps.FirstOrDefault(x => x.ViewModelType == viewModelType)
            ?? throw new InvalidOperationException(
                $"ViewModel {viewModelType.Name} is not registered in the factory.");

        var viewModel = (ViewModel)serviceProvider.GetRequiredKeyedService(typeof(ViewModel), viewModelMap.RegistrationKey);
        foreach (var injection in viewModelMap.Injections)
        {
            var injectedViewModel = Create(serviceProvider, injection.ViewModelType);
            injection.Property.SetValue(viewModel, injectedViewModel);
        }
        viewModel.OnDependenciesInjected();
        return viewModel;
    }

    public IEnumerable<PropertyInfo> GetNestedViewModelProperties(Type viewModelType)
    {
        var injectionMap = InjectionMaps.FirstOrDefault(x => x.ViewModelType == viewModelType)
            ?? throw new InvalidOperationException($"ViewModel {viewModelType.Name} is not registered in the factory.");

        return injectionMap.Injections.Select(x => x.Property);
    }
}
