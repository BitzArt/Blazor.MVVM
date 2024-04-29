using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Blazor page base class with view model support.
/// </summary>
/// <typeparam name="TViewModel">Type of this component's ViewModel</typeparam>
public abstract class PageBase<TViewModel> : ComponentBase, IPersistentComponent
    where TViewModel : ComponentViewModel
{
    private const string StateKey = "_state";

    /// <summary>
    /// This component's persistent state.
    /// </summary>
    [Inject]
    public PersistentComponentState ComponentState { get; private set; } = null!;

    /// <summary>
    /// This page's ViewModel.
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
        ComponentState!.RegisterOnPersisting(PersistState);

        await RestoreStateAsync();
    }

    private Task PersistState()
    {
        PersistComponentState(ViewModel, StateKey, strict: false);

        return Task.CompletedTask;
    }

    private void PersistComponentState(ComponentViewModel viewModel, string key, bool strict = true)
    {
        if (ViewModel is not IStatefulViewModel statefulViewModel)
        {
            if (strict) throw new InvalidOperationException($"ViewModel '{viewModel.GetType().Name}' must implement IStatefulViewModel");
            return;
        }

        ComponentState.PersistAsJson(StateKey, statefulViewModel.ComponentState);
    }

    private async Task RestoreStateAsync()
    {
        await RestoreComponentStateAsync(ViewModel, StateKey);
    }

    private async Task RestoreComponentStateAsync(ComponentViewModel viewModel, string key)
    {
        if (viewModel is not IStatefulViewModel statefulViewModel) return;

        var stateExists = ComponentState.TryTakeFromJson<JsonElement>(key, out var state);

        if (stateExists)
        {
            statefulViewModel.ComponentState = JsonSerializer.Deserialize(state, statefulViewModel.StateType, PersistentStateJsonSerializerOptionsProvider.Options)!;
        }
        else
        {
            statefulViewModel.ComponentState = Activator.CreateInstance(statefulViewModel.StateType)!;
            ViewModel.InitializeInternal();
            await ViewModel.InitializeAsyncInternal();
        }
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

internal static class PersistentStateJsonSerializerOptionsProvider
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
}
