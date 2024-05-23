using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BitzArt.Blazor.MVVM;

public abstract class ComponentBase<TViewModel> : ComponentBase, IStateComponent
    where TViewModel : ViewModel
{
    [Parameter, EditorRequired]
    public TViewModel ViewModel { get; set; } = null!;

    [Inject]
    protected IServiceProvider ServiceProvider { get; set; } = null!;

    [Inject]
    protected IJSRuntime Js { get; set; } = default!;

    [Inject]
    internal PageStateDictionaryContainer PageStateDictionaryContainer { get; set; } = null!;

    [Inject]
    private protected BlazorViewModelStateManager StateManager { get; set; } = null!;

    [Inject]
    protected RenderingEnvironment RenderingEnvironment { get; set; } = null!;

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
        ViewModel.OnComponentStateChanged += async (_) =>
        {
            await InvokeAsync(StateHasChanged);
        };

        await SetupStateAsync();
    }

    private async Task SetupStateAsync()
    {
        var shouldRestoreState = ShouldRestoreState();

        if (shouldRestoreState) await RestoreStateAsync(); 
        else await InitializeStateAsync();
    }

    protected virtual bool ShouldRestoreState()
    {
        return !RenderingEnvironment.IsPrerender;
    }

    protected virtual async Task InitializeStateAsync()
    {
        if (ViewModel is not IStatefulViewModel statefulViewModel) return;

        await StateManager.InitializeStateAsync(ViewModel);

        statefulViewModel.State.IsInitialized = true;

        statefulViewModel.OnStateInitialized?.Invoke(ViewModel);
        statefulViewModel.OnStateInitializedAsync?.Invoke(ViewModel);

        StateHasChanged();
        ViewModel.ComponentStateContainer?.NotifyStateChanged();
    }

    protected virtual async Task RestoreStateAsync()
    {
        if (ViewModel is not IStatefulViewModel statefulViewModel) return;

        await PageStateDictionaryContainer!.WaitUntilConfiguredAsync();
        var state = PageStateDictionaryContainer!.PageStateDictionary!.Get(ViewModel.Signature);
        if (state is null)
        {
            await InitializeStateAsync();
            return;
        }
        if (state!.GetType() != statefulViewModel.StateType) throw new InvalidOperationException("State type mismatch.");
        statefulViewModel.State = state;
        statefulViewModel.State.IsInitialized = true;
    }

    void IStateComponent.StateHasChanged() => InvokeAsync(StateHasChanged);
}
