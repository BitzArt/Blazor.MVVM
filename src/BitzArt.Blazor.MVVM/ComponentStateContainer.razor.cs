using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BitzArt.Blazor.MVVM;

public partial class ComponentStateContainer : ComponentBase
{
    [Parameter] public ViewModel ViewModel { get; set; } = null!;
    [Parameter] public string StateElementKey { get; set; } = "state";

    [Inject] private IStateManager StateManager { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var stateElement = BuildStateElement();
        if (stateElement is not null) builder.AddMarkupContent(1, stateElement);
    }

    private string BuildStateElement()
    {
        var stateEncoded = StateManager.EncodeState(ViewModel);

        return $"<script id=\"{StateElementKey}\" type=\"text/template\">{stateEncoded}</script>";
    }
}