using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM;

internal interface IViewModelFactory
{
    public void AddViewModel(Type viewModelType, string registrationKey);

    public TViewModel Create<TViewModel>(IServiceProvider serviceProvider, ComponentSignature? signature, ViewModel? parent = null) where TViewModel : ViewModel;
    public ViewModel Create(IServiceProvider serviceProvider, Type viewModelType, ComponentSignature? signature, ViewModel? parent = null, List<ViewModel>? affectedViewModels = null);
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
        if (!typeof(ViewModel).IsAssignableFrom(viewModelType)) 
            throw new InvalidOperationException($"Type {viewModelType.Name} is not a ViewModel.");

        if (InjectionMaps.ContainsKey(viewModelType))
            throw new InvalidOperationException($"ViewModel {viewModelType.Name} is already registered in the factory.");

        InjectionMaps.Add(viewModelType, new(viewModelType, registrationKey));
    }

    public TViewModel Create<TViewModel>(IServiceProvider serviceProvider, ComponentSignature? signature, ViewModel? parent = null)
        where TViewModel : ViewModel
        => (TViewModel)Create(serviceProvider, typeof(TViewModel), signature, parent);

    public ViewModel Create(IServiceProvider serviceProvider, Type viewModelType, ComponentSignature? signature, ViewModel? parent = null, List<ViewModel>? affectedViewModels = null)
    {
        signature ??= new RootComponentSignature();
        var isRoot = signature is RootComponentSignature;
        affectedViewModels ??= [];

        var viewModelMap = InjectionMaps.GetValueOrDefault(viewModelType)
            ?? throw new InvalidOperationException($"ViewModel {viewModelType.Name} is not registered in the factory.");

        var viewModel = (ViewModel)serviceProvider.GetRequiredKeyedService(typeof(ViewModel), viewModelMap.RegistrationKey);
        viewModel.Signature = signature;

        viewModel.OnComponentStateChanged += (sender) =>
        {
            viewModel.ComponentStateContainer?.NotifyStateChanged();
            return Task.CompletedTask;
        };

        foreach (var injection in viewModelMap.Injections)
        {
            if (injection.IsServiceInjection)
            {
                var injectedDependency = serviceProvider.GetRequiredService(injection.DependencyType);
                injection.Property.SetValue(viewModel, injectedDependency);
            }

            else if (injection.IsNestedViewModelInjection)
            {
                var nestedSignature = new ComponentSignature(parent: signature);
                var injectedViewModel = Create(serviceProvider, injection.DependencyType, nestedSignature, parent: viewModel, affectedViewModels: affectedViewModels);
                injection.Property.SetValue(viewModel, injectedViewModel);

                viewModel.OnComponentStateContainerWasSet += (container) =>
                {
                    injectedViewModel.ComponentStateContainer = container;
                };
            }
            
            else if (injection.IsParentViewModelInjection)
            {
                if (parent is null) throw new InvalidOperationException(
                    $"Parent injection '{injection.Property.Name}' in {viewModelType.Name} requires a parent ViewModel, but no parent was found in current ViewModel hierarchy.");
                injection.Property.SetValue(viewModel, parent);
            }
        }

        affectedViewModels.Add(viewModel);

        if (isRoot) foreach (var affected in affectedViewModels) affected.OnDependenciesInjected();

        return viewModel;
    }

    public ViewModelInjectionMap GetInjectionMap(Type viewModelType)
    {
        var injectionMap = InjectionMaps.GetValueOrDefault(viewModelType)
            ?? throw new InvalidOperationException($"ViewModel {viewModelType.Name} is not registered in the factory.");

        return injectionMap;
    }
}
