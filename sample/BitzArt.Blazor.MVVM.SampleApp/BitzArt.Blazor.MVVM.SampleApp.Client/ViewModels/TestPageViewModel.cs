namespace BitzArt.Blazor.MVVM.SampleApp;

public class TestPageViewModel : ViewModel<TestPageState>
{
    protected override void OnDependenciesInjected()
    {
        OnStateInitializedAsync += async (_) =>
        {
            await Task.Delay(2000);
            State.Text = "Test Page State initialized";
            ComponentStateHasChanged();
        };
    }
}

public class TestPageState : ComponentState
{
    public string Text { get; set; } = "Test Page State not initialized";
}