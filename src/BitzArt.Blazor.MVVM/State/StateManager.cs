using System.Text.Json;

namespace BitzArt.Blazor.MVVM;

public interface IStateManager
{
    /// <summary>
    /// Encodes <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states into a base64 string.
    /// </summary>
    public string? EncodeState(ViewModel viewModel);
}

internal class StateManager(IViewModelFactory viewModelFactory) : IStateManager
{
    private const string NestedStateKey = "__ns";

    /// <inheritdoc/>
    public string? EncodeState(ViewModel viewModel)
    {
        var state = GetCombinedState(viewModel);
        if (state is null) return null;

        var json = JsonSerializer.SerializeToUtf8Bytes(state, StateJsonOptionsProvider.Options);
        return Convert.ToBase64String(json);
    }

    /// <summary>
    /// Returns a dictionary containing combined state of <see cref="ViewModel"/> 
    /// and it's nested <see cref="ViewModel"/>s. <br />
    /// If <see cref="ViewModel"/> does not have own state or nested <see cref="ViewModel"/>s' states, 
    /// returns <see langword="null"/>.
    /// </summary>
    private Dictionary<string, object?>? GetCombinedState(ViewModel viewModel)
    {
        var state = new Dictionary<string, object?>();

        if (viewModel is IStatefulViewModel statefulViewModel)
        {
            foreach (var property in statefulViewModel.State.GetType().GetProperties())
                state.Add(property.Name, property.GetValue(statefulViewModel.State));
        }

        var nestedState = GetNestedState(viewModel);
        if (nestedState is not null)
            state.Add(NestedStateKey, nestedState);

        return state.Values.Count != 0 ? state : null;
    }

    /// <summary>
    /// Returns a dictionary containing states of nested <see cref="ViewModel"/>s. <br />
    /// If there are no nested <see cref="ViewModel"/>s, returns <see langword="null"/>.
    /// </summary>
    private Dictionary<string, object?>? GetNestedState(ViewModel viewModel)
    {
        var properties = viewModelFactory.GetNestedViewModelProperties(viewModel.GetType());

        if (!properties.Any()) return null;

        var state = new Dictionary<string, object?>();

        foreach (var property in properties)
        {
            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedViewModelState = GetCombinedState(nestedViewModel!);

            if (nestedViewModelState is null) continue;
            
            state.Add(property.Name, nestedViewModelState);
        }

        return state.Values.Count != 0 ? state : null;
    }
}