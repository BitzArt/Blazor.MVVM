namespace BitzArt.Blazor.MVVM;

internal interface IStatefulViewModel
{
    public Type StateType { get; }
    public object ComponentState { get; set; }
}
