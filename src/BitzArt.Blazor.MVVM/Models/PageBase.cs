using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Blazor page base class with view model support.
/// </summary>
/// <typeparam name="TViewModel">Type of this component's ViewModel</typeparam>
public abstract partial class PageBase<TViewModel> : ComponentBase, IStateComponent
    where TViewModel : ComponentViewModel
{
    private const string StateKey = "state";

    /// <summary>
    /// This page's ViewModel.
    /// </summary>
    [Inject]
    protected TViewModel ViewModel { get; set; } = null!;

    [Inject]
    private IJSRuntime Js { get; set; } = default!;

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
        ViewModel.Component = this;
        ViewModel.RenderingEnvironment = RenderingEnvironment;
        await RestoreStateAsync();
    }

    private async Task RestoreStateAsync()
    {
        var isPrerender = RenderingEnvironment.IsPrerender;
        var state = isPrerender
            ? null
            : await Js.InvokeAsync<string?>("getInnerText", [StateKey]);

        await RestoreComponentStateAsync(ViewModel, state);
    }

    private async Task RestoreComponentStateAsync(ComponentViewModel viewModel, string? state)
    {
        if (viewModel is not IStatefulViewModel statefulViewModel) return;

        if (state is not null)
        {
            var buffer = Convert.FromBase64String(state);
            var json = Encoding.UTF8.GetString(buffer);
            statefulViewModel.ComponentState = JsonSerializer.Deserialize(json, statefulViewModel.StateType, StateJsonOptionsProvider.Options)!;
        }
        else
        {
            statefulViewModel.ComponentState = Activator.CreateInstance(statefulViewModel.StateType)!;

            statefulViewModel.InitializeState();
            await statefulViewModel.InitializeStateAsync();
        }
    }

    /// <summary>
    /// Set the parameters from the query string.
    /// </summary>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        ViewModel.SetParametersFromQueryString(NavigationManager);

        return base.SetParametersAsync(parameters);
    }

    void IStateComponent.StateHasChanged() => InvokeAsync(StateHasChanged);
}

internal static class StateJsonOptionsProvider
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
}
