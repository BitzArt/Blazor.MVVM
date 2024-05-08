using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BitzArt.Blazor.MVVM;

public partial class ComponentStateContainer : ComponentBase
{
    [Parameter] public ViewModel ViewModel { get; set; } = null!;
    [Parameter] public string StateKey { get; set; } = "state";

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var state = SerializeState();
        if (state is not null) builder.AddMarkupContent(1, state);
    }

    private string? SerializeState()
    {
        return SerializeComponentState(ViewModel, StateKey, strict: false);
    }

    private string? SerializeComponentState(ViewModel viewModel, string key, bool strict = true)
    {
        if (ViewModel is not IStatefulViewModel statefulViewModel)
        {
            if (strict) throw new InvalidOperationException($"ViewModel '{viewModel.GetType().Name}' must implement IStatefulViewModel");
            return null;
        }

        return Serialize(statefulViewModel.State, key);
    }

    private static string? Serialize(object state, string key)
    {
        if (state is null || OperatingSystem.IsBrowser())
            return null;

        var json = JsonSerializer.SerializeToUtf8Bytes(state, StateJsonOptionsProvider.Options);
        var base64 = Convert.ToBase64String(json);
        return $"<script id=\"{key}\" type=\"text/template\">{base64}</script>";
    }
}