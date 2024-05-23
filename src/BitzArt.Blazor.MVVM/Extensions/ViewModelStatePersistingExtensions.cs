using System.Text.Json;
using System.Text.Json.Nodes;

namespace BitzArt.Blazor.MVVM;

internal static class ViewModelStatePersistingExtensions
{
    public static byte[]? SerializePageState(this ViewModelFactory factory, ViewModel viewModel)
    {
        var injectionMap = factory.GetInjectionMap(viewModel.GetType());
        var state = factory.SerializeComponentState(viewModel, injectionMap);
        state ??= [];

        return JsonSerializer.SerializeToUtf8Bytes(state, StateJsonOptionsProvider.Options);
    }

    private static JsonObject SerializeComponentState(this ViewModelFactory factory, ViewModel viewModel, ViewModelInjectionMap injectionMap)
    {
        JsonObject? currentNode = null;

        if (viewModel is IStatefulViewModel statefulViewModel && statefulViewModel.State.IsInitialized)
            currentNode = (JsonObject)JsonSerializer.SerializeToNode(
                statefulViewModel.State,
                statefulViewModel.State.GetType(),
                StateJsonOptionsProvider.Options)!;

        currentNode ??= [];

        foreach (var injection in injectionMap.Injections.Where(x => x.IsNestedViewModelInjection))
        {
            var property = injection.Property;
            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedMap = factory.GetInjectionMap(injection.DependencyType);
            var nestedNode = factory.SerializeComponentState(nestedViewModel!, nestedMap);

            if (nestedNode is not null && nestedNode!.Count != 0)
                currentNode.Add($"{NestedStatePrefix}{property.Name}", nestedNode);
        }

        return currentNode;
    }
}
