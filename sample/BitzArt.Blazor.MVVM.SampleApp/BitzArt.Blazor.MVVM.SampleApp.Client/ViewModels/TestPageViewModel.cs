
namespace BitzArt.Blazor.MVVM.SampleApp;

public class TestPageViewModel : ViewModel<TestPageState>
{
    public override async Task InitializeStateAsync()
    {
        State.Text = "Test Page State was initialized";
        await ComponentStateChangedAsync();
    }

    public override async Task OnStateRestoredAsync()
    {
        await Task.Delay(3000);

        State.Text = "Test Page State was restored";
        await ComponentStateChangedAsync();
    }
}

public class TestPageState : ComponentState
{
    public string Text { get; set; } = "Test Page State not initialized";
}