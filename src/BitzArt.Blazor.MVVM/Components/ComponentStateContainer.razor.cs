using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BitzArt.Blazor.MVVM;

public partial class ComponentStateContainer : ComponentBase
{
    [Inject]
    private RenderingEnvironment RenderingEnvironment { get; set; } = null!;

    [Inject]
    private ViewModelFactory ViewModelFactory { get; set; } = null!;

    [Parameter]
    public ViewModel ViewModel { get; set; } = null!;

    [Parameter]
    public string StateElementKey { get; set; } = "state";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        ViewModel.ComponentStateContainer = this;

        if (ViewModel.OnComponentStateContainerWasSet is not null)
            await ViewModel.OnComponentStateContainerWasSet!.Invoke(this);
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

        var json = ViewModelFactory.SerializePageState(ViewModel);
        if (json is null) return null;

        var stateEncoded = Convert.ToBase64String(json);

        return $"<script id=\"{StateElementKey}\" type=\"text/template\">{stateEncoded}</script>";
    }

    public async Task NotifyStateChangedAsync()
    {
        await InvokeAsync(StateHasChanged);
    }
}
