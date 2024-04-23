using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Blazor page base class with view model support.
/// </summary>
/// <typeparam name="TViewModel">Type of this component's ViewModel</typeparam>
public abstract class PageBase<TViewModel> : ComponentBase, IPersistentComponent
    where TViewModel : PageViewModel
{
    /// <summary>
    /// This component's persistent state.
    /// </summary>
    [Inject]
    public PersistentComponentState ComponentState { get; private set; } = null!;

    /// <summary>
    /// This component's ViewModel.
    /// </summary>
    [Inject]
    protected TViewModel ViewModel { get; set; } = null!;

    /// <summary>
    /// Navigation manager.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Method invoked when the component is ready to start, having received its initial
    /// parameters from its parent in the render tree. Override this method if you will
    /// perform an asynchronous operation and want the component to refresh when that
    /// operation is completed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        ViewModel.Component = this;
        ComponentState!.RegisterOnPersisting(PersistStateAsync);
        await ViewModel.InitializeAsync();
    }

    private async Task PersistStateAsync()
    {
        await ViewModel.PersistStateAsync();
    }

    void IPersistentComponent.StateHasChanged()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Set the parameters from the query string.
    /// </summary>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        ViewModel.SetParametersFromQueryString(NavigationManager);

        return base.SetParametersAsync(parameters);
    }
}
