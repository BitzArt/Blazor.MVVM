using System.Text.Json;

namespace BitzArt.Blazor.MVVM;

public interface IStateManager
{
    /// <summary>
    /// Encodes <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states into a base64 string.
    /// </summary>
    public string EncodeState(ViewModel viewModel);
}

internal class StateManager(IViewModelFactory viewModelFactory) : IStateManager
{
    private const string NestedStateKey = "__ns";

    /// <inheritdoc/>
    public string EncodeState(ViewModel viewModel)
    {
        var state = GetState(viewModel);
        var json = JsonSerializer.SerializeToUtf8Bytes(state, StateJsonOptionsProvider.Options);

        return Convert.ToBase64String(json);
    }

    private Dictionary<string, object?> GetState(ViewModel viewModel)
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

        return state;
    }

    private Dictionary<string, object?>? GetNestedState(ViewModel viewModel)
    {
        var properties = viewModelFactory.GetNestedViewModelProperties(viewModel.GetType());

        if (!properties.Any()) return null;

        var state = new Dictionary<string, object?>();

        foreach (var property in properties)
        {
            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedViewModelState = GetState(nestedViewModel!);
            state.Add(property.Name, nestedViewModelState);
        }

        return state;
    }
}
