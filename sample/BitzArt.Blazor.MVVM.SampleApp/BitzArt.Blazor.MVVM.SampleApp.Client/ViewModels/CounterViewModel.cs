namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterViewModel : ViewModel
{
    public CounterState? State { get; set; }

    private readonly Timer _timer;

    public CounterViewModel()
    {
        _timer = new Timer(TimerIncrementCount, null, 1000, 1000);
    }

    private void TimerIncrementCount(object? state)
    {
        if (State is null) return;

        State.Count++;
        StateHasChanged();
    }

    public void IncrementCount()
    {
        if (State is null) return;

        State.Count++;
        StateHasChanged();
    }
}

public class CounterState
{
    public int Count { get; set; } = 0;
}
