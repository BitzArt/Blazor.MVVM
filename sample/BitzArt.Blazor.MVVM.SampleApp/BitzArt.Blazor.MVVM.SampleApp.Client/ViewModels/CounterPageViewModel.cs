namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterPageViewModel : ComponentViewModel<CounterPageViewModelState>
{
    protected override void Initialize()
    {
        State.Count = 0;

        State.Text = "ViewModel State initialized on: " + (OperatingSystem.IsBrowser() ? "Client" : "Server");
    }

    public void IncrementCount()
    {
        State.Count++;
    }
}

public class CounterPageViewModelState
{
    public int? Count { get; set; }

    public string? Text { get; set; } = "State not initialized";
}
