using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.SampleApp.Client.Components;

public partial class Counter : ComponentBase<CounterViewModel>
{
    [Parameter, EditorRequired]
    public string? Description { get; set; }
}
