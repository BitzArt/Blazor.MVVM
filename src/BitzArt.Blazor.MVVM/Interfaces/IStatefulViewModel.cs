namespace BitzArt.Blazor.MVVM;

internal interface IStatefulViewModel
{
    public Type StateType { get; }
    public object State { get; set; }

    public void InitializeState();
    public Task InitializeStateAsync();
    public void OnStateRestored();
}
