namespace BitzArt.Blazor.MVVM;

/// <summary>
/// ViewModel base class.
/// </summary>
public abstract class ViewModel
{
    public class StateChangeEventArgs : EventArgs { }
    public delegate Task<StateChangeEventArgs> StateHasChangedHandler(object sender, StateChangeEventArgs? args);
    public StateHasChangedHandler? OnStateChange { get; set; }

    /// <summary>
    /// Notifies the component that the state has changed.
    /// </summary>
    protected void StateHasChanged(StateChangeEventArgs? args = null)
    {
        OnStateChange?.Invoke(this, args);
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
        set => _state = value;
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

    public virtual Task OnStateRestoredAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when the state has changed.
    /// </summary>
    public virtual void OnStateChanged() { }

    public virtual Task OnStateChangedAsync()
    {
        return Task.CompletedTask;
    }
}
