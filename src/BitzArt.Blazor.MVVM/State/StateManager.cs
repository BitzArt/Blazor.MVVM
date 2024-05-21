using System.Text.Json;

namespace BitzArt.Blazor.MVVM;

internal class StateManager(IViewModelFactory viewModelFactory) : IStateManager
{
    private const string NestedStateKey = "__ns_";

    /// <inheritdoc/>
    public string? EncodeState(ViewModel viewModel)
    {
        var injectionMap = viewModelFactory.GetInjectionMap(viewModel.GetType());
        var state = GetState(viewModel, injectionMap);

        if (state is null) return null;

        var json = JsonSerializer.SerializeToUtf8Bytes(state, StateJsonOptionsProvider.Options);
        return Convert.ToBase64String(json);
    }

    private Dictionary<string, object?>? GetState(ViewModel viewModel, ViewModelInjectionMap injectionMap)
    {
        var state = new Dictionary<string, object?>();

        if (viewModel is IStatefulViewModel statefulViewModel)
        {
            foreach (var property in statefulViewModel.StateType.GetProperties())
                state.Add(property.Name, property.GetValue(statefulViewModel.State));
        }

        foreach (var injection in injectionMap.Injections)
        {
            var property = injection.Property;
            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedMap = viewModelFactory.GetInjectionMap(injection.ViewModelType);
            var nestedState = GetState(nestedViewModel!, nestedMap);

            if (nestedState is not null)
                state.Add($"{NestedStateKey}{property.Name}", nestedState);
        }

        return state.Values.Count != 0 ? state : null;
    }
}
