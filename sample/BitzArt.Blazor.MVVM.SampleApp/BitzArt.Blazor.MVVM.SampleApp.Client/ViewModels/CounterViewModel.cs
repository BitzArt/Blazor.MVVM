namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterViewModel(RenderingEnvironment renderingEnvironment)
    : ViewModel<CounterState>, IDisposable
{
    [ParentViewModel]
    public CounterPageViewModel Parent { get; set; } = null!;

    private Timer? _timer;

    public string NameOnPage
    {
        get
        {
            if (this == Parent.Counter1) return "Counter 1";
            if (this == Parent.Counter2) return "Counter 2";
            if (this == Parent.Counter3) return "Counter 3";
            return string.Empty;
        }
    }

    public override async Task InitializeStateAsync()
    {
        await Task.Delay(100);
        State.Text = $"Counter State initialized on: {renderingEnvironment}";
    }

    protected override void OnDependenciesInjected()
    {
        if (this == Parent.Counter3) _timer = new Timer(IncrementCount, null, 1000, 1000);
    }

    private void IncrementCount(object? state)
    {
        _ = IncrementCountAsync();
    }

    public async Task IncrementCountAsync()
    {
        State.Count++;
        await ComponentStateChangedAsync();
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class CounterState : ComponentState
{
    public int Count { get; set; } = 0;

    public string Text { get; set; } = "Counter State not initialized";
}
