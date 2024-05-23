using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BitzArt.Blazor.MVVM;

public partial class ComponentStateContainer : ComponentBase
{
    [Inject]
    private BlazorViewModelStateManager StateManager { get; set; } = null!;

    [Inject]
    private RenderingEnvironment RenderingEnvironment { get; set; } = null!;

    [Parameter]
    public ViewModel ViewModel { get; set; } = null!;

    [Parameter]
    public string StateElementKey { get; set; } = "state";

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ViewModel.ComponentStateContainer = this;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var stateElement = BuildStateElement();
        if (stateElement is not null)
            builder.AddMarkupContent(1, stateElement);
    }

    private string? BuildStateElement()
    {
        if (!RenderingEnvironment.IsPrerender) return null;

        var json = StateManager.SerializeState(ViewModel);
        if (json is null) return null;

        var stateEncoded = Convert.ToBase64String(json);

        return $"<script id=\"{StateElementKey}\" type=\"text/template\">{stateEncoded}</script>";
    }

    public void NotifyStateChanged()
    {
        StateHasChanged();
    }
}
