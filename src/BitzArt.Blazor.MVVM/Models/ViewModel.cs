namespace BitzArt.Blazor.MVVM;

/// <summary>
/// ViewModel base class.
/// </summary>
public abstract class ViewModel
{
    public class StateChangeEventArgs : EventArgs { }
    public delegate StateChangeEventArgs StateHasChangedHandler(StateChangeEventArgs? args);
    public StateHasChangedHandler? OnStateHasChanged { get; set; }

    /// <summary>
    /// Notifies the component that the state has changed.
    /// </summary>
    protected void StateHasChanged(StateChangeEventArgs? args = null)
    {
        OnStateHasChanged?.Invoke(args);
    }
}

/// <summary>
/// ViewModel base class with state.
/// </summary>
/// <typeparam name="TState"></typeparam>
public abstract class ViewModel<TState> : ViewModel, IStatefulViewModel
    where TState : class, new()
{
    private TState _state = null!;

    /// <summary>
    /// Persistent state of this ViewModel.
    /// </summary>
    public TState State
    {
        get => _state;
        set
        {
            _state = value;
            OnStateChanged(State);
        }
    }

    object IStatefulViewModel.State
    {
        get => State;
        set => State = (TState)value;
    }

    /// <summary>
    /// Type of this ViewModel's state.
    /// </summary>
    public Type StateType => typeof(TState);

    /// <summary>
    /// Initializes this ViewModel's State.
    /// </summary>
    public virtual void InitializeState() { }

    /// <summary>
    /// Initializes this ViewModel's State.
    /// </summary>
    public virtual Task InitializeStateAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when the state is restored.
    /// </summary>
    public virtual void OnStateRestored() { }

    /// <summary>
    /// Called when the state has changed.
    /// </summary>
    protected virtual void OnStateChanged(TState state) { }
}
