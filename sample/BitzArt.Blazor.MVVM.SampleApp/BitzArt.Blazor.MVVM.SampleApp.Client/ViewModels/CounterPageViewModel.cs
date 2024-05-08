namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterPageViewModel(
    CounterViewModel counter1ViewModel,
    CounterViewModel counter2ViewModel,
    RenderingEnvironment renderingEnvironment)
    : ViewModel<CounterPageViewModelState>
{
    public CounterViewModel Counter1ViewModel { get; } = counter1ViewModel;
    public CounterViewModel Counter2ViewModel { get; } = counter2ViewModel;

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
