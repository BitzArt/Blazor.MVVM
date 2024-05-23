using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace BitzArt.Blazor.MVVM;

internal class BlazorViewModelStateManager(IViewModelFactory viewModelFactory)
{
    private ViewModelFactory _viewModelFactory { get; } = (ViewModelFactory)viewModelFactory;

    private const string _nestedStatePrefix = "n__";

    /// <summary>
    /// Serializes states in <see cref="ViewModel"/>s hierarchy to JSON encoded as UTF-8 bytes.
    /// </summary>
    public byte[]? SerializeState(ViewModel viewModel)
    {
        var injectionMap = _viewModelFactory.GetInjectionMap(viewModel.GetType());
        var state = GetState(viewModel, injectionMap);
        state ??= [];

        return JsonSerializer.SerializeToUtf8Bytes(state, StateJsonOptionsProvider.Options);
    }

    private Dictionary<string, object?>? GetState(ViewModel viewModel, ViewModelInjectionMap injectionMap)
    {
        var state = new Dictionary<string, object?>();

        if (viewModel is IStatefulViewModel statefulViewModel && statefulViewModel.State.IsInitialized)
        {
            var nodeJson = (JsonObject)JsonSerializer.SerializeToNode(statefulViewModel.State, statefulViewModel.State.GetType(), StateJsonOptionsProvider.Options)!;
            foreach (var node in nodeJson) state.Add(node.Key, node.Value);
        }

        foreach (var injection in injectionMap.Injections.Where(x => x.IsNestedViewModelInjection))
        {
            var property = injection.Property;
            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedMap = _viewModelFactory.GetInjectionMap(injection.DependencyType);
            var nestedState = GetState(nestedViewModel!, nestedMap);

            if (nestedState is not null)
                state.Add($"{_nestedStatePrefix}{property.Name}", nestedState);
        }

        return state.Values.Count != 0 ? state : null;
    }

    /// <summary>
    /// Restores states in <see cref="ViewModel"/>s hierarchy from JSON string.
    /// </summary>
    public async Task<PageStateDictionary?> RestoreStateAsync(ViewModel rootViewModel, string json)
    {
        var node = JsonNode.Parse(json);
        if (node is null) return null;

        var injectionMap = _viewModelFactory.GetInjectionMap(rootViewModel.GetType());
        var result = new PageStateDictionary();
        await RestoreStateAsync(rootViewModel, node, injectionMap, result);

        return result;
    }

    private async Task RestoreStateAsync(ViewModel viewModel, JsonNode node, ViewModelInjectionMap injectionMap, PageStateDictionary pageStateDictionary)
    {
        foreach (var injection in injectionMap.Injections.Where(x => x.IsNestedViewModelInjection))
        {
            var property = injection.Property;
            var jsonKey = $"{_nestedStatePrefix}{property.Name}";
            var nestedNode = node[jsonKey];

            if (nestedNode is null) continue;

            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedMap = _viewModelFactory.GetInjectionMap(injection.DependencyType);

            await RestoreStateAsync(nestedViewModel!, nestedNode, nestedMap, pageStateDictionary);

            (node as JsonObject)!.Remove(jsonKey);
        }

        if (viewModel is not IStatefulViewModel statefulViewModel) return;

        var state = JsonSerializer.Deserialize(node, statefulViewModel.StateType, StateJsonOptionsProvider.Options);
        pageStateDictionary.Add(viewModel.Signature, state as ComponentState);

        statefulViewModel.OnStateRestored();
        await statefulViewModel.OnStateRestoredAsync();

        statefulViewModel.OnStateChanged();
        await statefulViewModel.OnStateChangedAsync();
    }

    /// <summary>
    /// Initializes states in <see cref="ViewModel"/>s hierarchy.
    /// </summary>
    public async Task InitializeStateAsync(ViewModel viewModel)
    {
        var injectionMap = _viewModelFactory.GetInjectionMap(viewModel.GetType());
        await InitializeStateAsync(viewModel, injectionMap);
    }

    private async Task InitializeStateAsync(ViewModel viewModel, ViewModelInjectionMap injectionMap)
    {
        if (viewModel is not IStatefulViewModel statefulViewModel) return;

        statefulViewModel.State = (Activator.CreateInstance(statefulViewModel.StateType) as ComponentState)!;

        statefulViewModel.OnStateChanged();
        await statefulViewModel.OnStateChangedAsync();

        statefulViewModel.InitializeState();
        await statefulViewModel.InitializeStateAsync();

        statefulViewModel.OnStateChanged();
        await statefulViewModel.OnStateChangedAsync();
    }
}
