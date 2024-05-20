using System.Text.Json;

namespace BitzArt.Blazor.MVVM;

public interface IStateManager
{
    public byte[] SerializeState(ViewModel viewModel);
}

internal class StateManager(IViewModelFactory viewModelFactory) : IStateManager
{
    private const string ViewModelStateKey = "__vms";
    private const string NestedStateKey = "__ns";

    public byte[] SerializeState(ViewModel viewModel)
    {
        var state = GetViewModelState(viewModel);
        var json = JsonSerializer.SerializeToUtf8Bytes(state, StateJsonOptionsProvider.Options);

        return json;
    }

    private Dictionary<string, object?> GetViewModelState(ViewModel viewModel)
    {
        var state = new Dictionary<string, object?>();

        if (viewModel is IStatefulViewModel statefulViewModel)
            state.Add(ViewModelStateKey, statefulViewModel.State);

        var nestedState = GetNestedState(viewModel);
        if (nestedState is not null)
            state.Add(NestedStateKey, nestedState);

        return state;
    }

    private Dictionary<string, object?>? GetNestedState(ViewModel viewModel)
    {
        var nestedViewModelProperties = viewModelFactory.GetNestedViewModelProperties(viewModel.GetType());

        if (!nestedViewModelProperties.Any()) return null;

        var state = new Dictionary<string, object?>();

        foreach (var property in nestedViewModelProperties)
        {
            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedViewModelState = GetViewModelState(nestedViewModel!);
            state.Add(property.Name, nestedViewModelState);
        }

        return state;
    }
}
