using Microsoft.AspNetCore.Components;
using System.Text;
using System.Text.Json;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Blazor page base class with view model support.
/// </summary>
/// <typeparam name="TViewModel">Type of this component's ViewModel</typeparam>
public abstract class PageBase<TViewModel> : ComponentBase<TViewModel>, IStateComponent
    where TViewModel : ViewModel
{
    [Inject]
    public new TViewModel ViewModel
    {
        get => base.ViewModel;
        set => base.ViewModel = value;
    }

    private const string StateKey = "state";

    protected override async Task RestoreStateAsync()
    {
        var isPrerender = RenderingEnvironment.IsPrerender;
        var state = isPrerender
            ? null
            : await Js.InvokeAsync<string?>("getInnerText", [StateKey]);

        await RestoreComponentStateAsync(ViewModel, state);
    }

    private static async Task RestoreComponentStateAsync(ViewModel viewModel, string? state)
    {
        if (viewModel is not IStatefulViewModel statefulViewModel) return;

        if (state is not null)
        {
            var buffer = Convert.FromBase64String(state);
            var json = Encoding.UTF8.GetString(buffer);
            statefulViewModel.State = JsonSerializer.Deserialize(json, statefulViewModel.StateType, StateJsonOptionsProvider.Options)!;
            
            statefulViewModel.OnStateRestored();
            await statefulViewModel.OnStateRestoredAsync();
        }
        else
        {
            statefulViewModel.State = Activator.CreateInstance(statefulViewModel.StateType)!;

            statefulViewModel.OnStateChanged();
            await statefulViewModel.OnStateChangedAsync();

            statefulViewModel.InitializeState();
            await statefulViewModel.InitializeStateAsync();
        }

        statefulViewModel.OnStateChanged();
        await statefulViewModel.OnStateChangedAsync();
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

internal static class StateJsonOptionsProvider
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
}
