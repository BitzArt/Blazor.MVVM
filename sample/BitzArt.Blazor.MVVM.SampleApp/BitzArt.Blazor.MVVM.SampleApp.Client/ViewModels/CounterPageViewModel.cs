using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.SampleApp;

public class CounterPageViewModel(
    RenderingEnvironment renderingEnvironment)
    : ViewModel<CounterPageViewModelState>
{
    [Inject]
    public CounterViewModel Counter1ViewModel { get; set; } = null!;

    [Inject]
    public CounterViewModel Counter2ViewModel { get; set; } = null!;

    public override void InitializeState()
    {
        State.Text = $"ViewModel State initialized on: {renderingEnvironment}";

        OnStateRestored();
    }

    public override void OnStateRestored()
    {
    }
}

public class CounterPageViewModelState
{
    public string Text { get; set; } = "State not initialized";
}
