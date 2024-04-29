namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Base class for Blazor view models.
/// </summary>
public abstract class ComponentViewModel
{
    internal IPersistentComponent Component { get; set; } = null!;

    internal Task InitializeAsyncInternal() => InitializeAsync();

    /// <summary>
    /// Initializes this view model.
    /// </summary>
    protected virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    internal void InitializeInternal() => Initialize();

    /// <summary>
    /// Initializes this view model.
    /// </summary>
    protected virtual void Initialize() { }

    /// <summary>
    /// Notifies the component that the state has changed.
    /// </summary>
    protected void StateHasChanged()
    {
        Component.StateHasChanged();
    }
}

/// <summary>
/// Base class for Blazor view models with state.
/// </summary>
/// <typeparam name="TState"></typeparam>
public abstract class ComponentViewModel<TState> : ComponentViewModel, IStatefulViewModel
    where TState : class, new()
{
    /// <summary>
    /// Persistent state of this view model.
    /// </summary>
    public TState State { get; internal set; } = null!;

    /// <summary>
    /// A property linked to this component's state, used for state persistence.
    /// </summary>
    public object ComponentState
    {
        get => State;
        set => State = (TState)value;
    }

    /// <summary>
    /// Type of this view model's state.
    /// </summary>
    public Type StateType => typeof(TState);
}
