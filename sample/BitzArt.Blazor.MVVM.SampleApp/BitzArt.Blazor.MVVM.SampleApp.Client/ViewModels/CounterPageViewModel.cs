namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterPageViewModel : ComponentViewModel<CounterPageViewModelState>
{
    private readonly Timer _timer;

    public CounterPageViewModel()
    {
        _timer = new Timer(TimerIncrementCount, null, 1000, 1000);
    }

    private void TimerIncrementCount(object? state)
    {
        State.Count++;
        StateHasChanged();
    }

    public override void InitializeState()
    {
        State.Count = 0;
        State.Text = $"ViewModel State initialized on: {RenderingEnvironment}";
    }

    public void IncrementCount()
    {
        State.Count++;
    }
}

public class CounterPageViewModelState
{
    public int? Count { get; set; } = null;

    public string? Text { get; set; } = "State not initialized";
}
