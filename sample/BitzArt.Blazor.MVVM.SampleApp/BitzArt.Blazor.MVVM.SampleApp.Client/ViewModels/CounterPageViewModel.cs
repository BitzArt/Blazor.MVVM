namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterPageViewModel(RenderingEnvironment renderingEnvironment)
    : ViewModel<CounterPageState>
{
    [NestViewModel]
    public CounterViewModel Counter1 { get; set; } = null!;

    [NestViewModel]
    public CounterViewModel Counter2 { get; set; } = null!;

    [NestViewModel]
    public CounterViewModel Counter3 { get; set; } = null!;

    public override void InitializeState()
    {
        State.Text = $"Page State initialized on: {renderingEnvironment}";
    }

    protected override void OnDependenciesInjected()
    {
        Counter2.OnStateInitialized += (_) =>
        {
            Counter2.State!.Count += 100;
        };
    }
}

public class CounterPageState : ComponentState
{
    public string Text { get; set; } = "Page State not initialized";
}
