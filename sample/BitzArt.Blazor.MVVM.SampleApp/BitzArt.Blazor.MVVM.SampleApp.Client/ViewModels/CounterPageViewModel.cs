namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterPageViewModel : PageViewModel<CounterPageViewModelState>
{
    protected override void InitializeState()
    {
        State.Count = 0;
        State.Text = "Initialized";
    }

    public void IncrementCount()
    {
        State.Count++;
    }
}

public class CounterPageViewModelState
{
    public int? Count { get; set; }

    public string? Text { get; set; } = "NotInitialized";
}
