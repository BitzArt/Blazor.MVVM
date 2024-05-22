using System.Text.Json;
using System.Text.Json.Nodes;

namespace BitzArt.Blazor.MVVM;

internal class BlazorViewModelStateManager(IViewModelFactory viewModelFactory)
{
    private ViewModelFactory _viewModelFactory { get; } = (ViewModelFactory)viewModelFactory;

    private const string _nestedStateKey = "__ns_";

    /// <summary>
    /// Serializes <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states 
    /// to JSON encoded as UTF-8 bytes.
    /// </summary>
    public byte[]? SerializeState(ViewModel viewModel)
    {
        var injectionMap = _viewModelFactory.GetInjectionMap(viewModel.GetType());
        var state = GetState(viewModel, injectionMap);

        if (state is null) return null;

        return JsonSerializer.SerializeToUtf8Bytes(state, StateJsonOptionsProvider.Options);
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
            var nestedMap = _viewModelFactory.GetInjectionMap(injection.ViewModelType);
            var nestedState = GetState(nestedViewModel!, nestedMap);

            if (nestedState is not null)
                state.Add($"{_nestedStateKey}{property.Name}", nestedState);
        }

        return state.Values.Count != 0 ? state : null;
    }

    /// <summary>
    /// Restores <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states from JSON string.
    /// </summary>
    public async Task RestoreStateAsync(ViewModel viewModel, string json)
    {
        var node = JsonNode.Parse(json);

        if (node is null) return;

        var injectionMap = _viewModelFactory.GetInjectionMap(viewModel.GetType());
        await RestoreStateAsync(viewModel, node, injectionMap);
    }

    private async Task RestoreStateAsync(ViewModel viewModel, JsonNode node, ViewModelInjectionMap injectionMap)
    {
        foreach (var injection in injectionMap.Injections)
        {
            var property = injection.Property;
            var jsonKey = $"{_nestedStateKey}{property.Name}";
            var nestedNode = node[jsonKey];

            if (nestedNode is null) continue;

            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedMap = _viewModelFactory.GetInjectionMap(injection.ViewModelType);

            await RestoreStateAsync(nestedViewModel!, nestedNode, nestedMap);

            node.AsObject().Remove(jsonKey);
        }

        if (viewModel is not IStatefulViewModel statefulViewModel) return;

        var state = JsonSerializer.Deserialize(node, statefulViewModel.StateType, StateJsonOptionsProvider.Options);
        statefulViewModel.State = state!;

        statefulViewModel.OnStateRestored();
        await statefulViewModel.OnStateRestoredAsync();

        statefulViewModel.OnStateChanged();
        await statefulViewModel.OnStateChangedAsync();
    }

    /// <summary>
    /// Initializes <see cref="ViewModel"/>'s and it's nested <see cref="ViewModel"/>s' states.
    /// </summary>
    public async Task InitializeStateAsync(ViewModel viewModel)
    {
        var injectionMap = _viewModelFactory.GetInjectionMap(viewModel.GetType());
        await InitializeStateAsync(viewModel, injectionMap);
    }

    private async Task InitializeStateAsync(ViewModel viewModel, ViewModelInjectionMap injectionMap)
    {
        foreach (var injection in injectionMap.Injections)
        {
            var property = injection.Property;
            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedMap = _viewModelFactory.GetInjectionMap(injection.ViewModelType);

            await InitializeStateAsync(nestedViewModel!, nestedMap);
        }

        if (viewModel is not IStatefulViewModel statefulViewModel) return;

        statefulViewModel.State = Activator.CreateInstance(statefulViewModel.StateType)!;

        statefulViewModel.OnStateChanged();
        await statefulViewModel.OnStateChangedAsync();

        statefulViewModel.InitializeState();
        await statefulViewModel.InitializeStateAsync();

        statefulViewModel.OnStateChanged();
        await statefulViewModel.OnStateChangedAsync();
    }
}