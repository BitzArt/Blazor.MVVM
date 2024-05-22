using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterPageViewModel(
    RenderingEnvironment renderingEnvironment)
    : ViewModel<CounterPageViewModelState>
{
    [Inject]
    public CounterViewModel Counter1 { get; set; } = null!;

    [Inject]
    public CounterViewModel Counter2 { get; set; } = null!;

    public override void InitializeState()
    {
        State.Text = $"ViewModel State initialized on: {renderingEnvironment}";
        Counter2.State!.Count = 100;
    }
}

public class CounterPageViewModelState
{
    public string Text { get; set; } = "State not initialized";
}
