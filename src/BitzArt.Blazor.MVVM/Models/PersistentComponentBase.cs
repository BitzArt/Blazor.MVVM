using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text;

namespace BitzArt.Blazor.MVVM;

public abstract class PersistentComponentBase<TState> : ComponentBase
    where TState : new()
{
    [Inject]
    private IJSRuntime Js { get; set; } = default!;
    protected TState State { get; set; } = new();
    private const string StateKey = "state";

    protected override void BuildRenderTree(RenderTreeBuilder builder) =>
        builder.AddMarkupContent(1, Serialize());

    protected virtual Task InitializeStateAsync() => Task.CompletedTask;

    protected override async Task OnInitializedAsync()
    {
        if (!OperatingSystem.IsBrowser())
        {
            await InitializeStateAsync();
            return;
        }

        var stateJson = await Js.InvokeAsync<string?>($"document.getElementById({StateKey}).innerText");

        if (string.IsNullOrWhiteSpace(stateJson))
        {
            await InitializeStateAsync();
            return;
        }

        try
        {
            var buffer = Convert.FromBase64String(stateJson);
            var json = Encoding.UTF8.GetString(buffer);
            State = JsonSerializer.Deserialize<TState>(json)!;
        }
        catch
        {
            await InitializeStateAsync();
        }
    }

    private string Serialize()
    {
        if (State is null || OperatingSystem.IsBrowser())
            return "";

        var json = JsonSerializer.SerializeToUtf8Bytes(State);
        var base64 = Convert.ToBase64String(json);
        return $"<script id=\"{StateKey}\" type=\"text/template\">{base64}</script>";
    }
}
