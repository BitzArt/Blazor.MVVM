using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static BitzArt.Blazor.MVVM.ViewModel;

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
        ViewModel.OnStateChange += async (_, args) =>
        {
            await InvokeAsync(StateHasChanged);
            return args!;
        };

        await RestoreStateAsync();
    }

    protected virtual Task RestoreStateAsync() => Task.CompletedTask;

    void IStateComponent.StateHasChanged() => InvokeAsync(StateHasChanged);
}
