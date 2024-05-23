namespace BitzArt.Blazor.MVVM.SampleApp;

public class TestPageViewModel : ViewModel<TestPageState>
{
    protected override void OnDependenciesInjected()
    {
        OnStateInitializedAsync += async (_) =>
        {
            State.Text = "Test Page State initialized";
            await ComponentStateChangedAsync();
        };
    }
}

public class TestPageState : ComponentState
{
    public string Text { get; set; } = "Test Page State not initialized";
}