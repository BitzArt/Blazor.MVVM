namespace BitzArt.Blazor.MVVM;

internal interface IStatefulViewModel
{
    public Type StateType { get; }
    public ComponentState State { get; set; }

    public void InitializeState();
    public Task InitializeStateAsync();

    public void OnStateRestored();
    public Task OnStateRestoredAsync();

    public void OnStateChanged();
    public Task OnStateChangedAsync();
}
