using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterPageViewModel(
    RenderingEnvironment renderingEnvironment)
    : ViewModel<CounterPageViewModelState>
{
    [Inject]
    public CounterViewModel Counter1ViewModel { get; set; } = null!;

    [Inject]
    public CounterViewModel Counter2ViewModel { get; set; } = null!;

    public override void InitializeState()
    {
        State.Text = $"ViewModel State initialized on: {renderingEnvironment}";

        OnStateRestored();
        Counter2ViewModel.State!.Count += 100;
    }

    public override void OnStateRestored()
    {
        Counter1ViewModel.State = State.Counter1State;
        Counter2ViewModel.State = State.Counter2State;
    }
}

public class CounterPageViewModelState
{
    public string Text { get; set; } = "State not initialized";
    public CounterState Counter1State { get; set; } = new();
    public CounterState Counter2State { get; set; } = new();
}
