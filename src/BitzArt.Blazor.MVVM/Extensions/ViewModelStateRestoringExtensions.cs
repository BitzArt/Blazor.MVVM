using System.Text.Json;
using System.Text.Json.Nodes;

namespace BitzArt.Blazor.MVVM;

internal static class ViewModelStateRestoringExtensions
{
    public static async Task<PageStateDictionary?> RestorePageComponentsStateAsync(this ViewModelFactory factory, ViewModel rootViewModel, string json)
    {
        var node = JsonNode.Parse(json);
        if (node is null) return null;

        var injectionMap = factory.GetInjectionMap(rootViewModel.GetType());
        var result = new PageStateDictionary();
        await factory.RestoreComponentStateAsync(rootViewModel, node, injectionMap, result);

        return result;
    }

    private static async Task RestoreComponentStateAsync(this ViewModelFactory factory, ViewModel viewModel, JsonNode node, ViewModelInjectionMap injectionMap, PageStateDictionary pageStateDictionary)
    {
        foreach (var injection in injectionMap.Injections.Where(x => x.IsNestedViewModelInjection))
        {
            var property = injection.Property;
            var jsonKey = $"{NestedStatePrefix}{property.Name}";
            var nestedNode = node[jsonKey];

            if (nestedNode is null) continue;

            var nestedViewModel = property.GetValue(viewModel) as ViewModel;
            var nestedMap = factory.GetInjectionMap(injection.DependencyType);

            await factory.RestoreComponentStateAsync(nestedViewModel!, nestedNode, nestedMap, pageStateDictionary);

            (node as JsonObject)!.Remove(jsonKey);
        }

        if (viewModel is not IStatefulViewModel statefulViewModel) return;

        var state = JsonSerializer.Deserialize(node, statefulViewModel.StateType, StateJsonOptionsProvider.Options);
        pageStateDictionary.Add(viewModel.Signature, state as ComponentState);
    }
}
