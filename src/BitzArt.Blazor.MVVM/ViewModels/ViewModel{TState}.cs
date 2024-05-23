using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// ViewModel base class with state.
/// </summary>
/// <typeparam name="TState"></typeparam>
public abstract class ViewModel<TState> : ViewModel, IStatefulViewModel
    where TState : ComponentState, new()
{
    // ===========================   STATE   ===========================

    private TState _state = null!;

    /// <summary>
    /// Persistent state of this ViewModel.
    /// </summary>
    public TState State
    {
        get => _state;
        set => _state = value;
    }

    // =====================   IStatefulViewModel    =====================

    ComponentState IStatefulViewModel.State
    {
        get => State;
        set => State = (TState)value;
    }

    /// <summary>
    /// Type of this ViewModel's state.
    /// </summary>
    public Type StateType => typeof(TState);

    // ==========================   LIFECYCLE   ==========================

    /// <summary>
    /// Initializes this ViewModel's State.
    /// </summary>
    public virtual void InitializeState() { }

    /// <summary>
    /// Initializes this ViewModel's State.
    /// </summary>
    public virtual Task InitializeStateAsync() => Task.CompletedTask;

    /// <summary>
    /// Called when the state is restored.
    /// </summary>
    public virtual void OnStateRestored() { }

    /// <summary>
    /// Called when the state is restored.
    /// </summary>
    public virtual Task OnStateRestoredAsync() => Task.CompletedTask;

    /// <summary>
    /// Called when the state has changed.
    /// </summary>
    public virtual void OnStateChanged() { }

    /// <summary>
    /// Called when the state has changed.
    /// </summary>
    public virtual Task OnStateChangedAsync() => Task.CompletedTask;

    // ========================   SUBSCRIBTIONS   ========================

    /// <summary>
    /// Called when the state is initialized.
    /// </summary>
    public StateInitializedHandler? OnStateInitialized { get; set; }

    /// <summary>
    /// Called when the state is initialized.
    /// </summary>
    public StateInitializedAsyncHandler? OnStateInitializedAsync { get; set; }
}
