﻿namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Base class for Blazor components' ViewModels.
/// </summary>
public abstract class ComponentViewModel
{
    internal IStateComponent Component { get; set; } = null!;

    protected internal RenderingEnvironment RenderingEnvironment { get; internal set; } = null!;

    /// <summary>
    /// Notifies the component that the state has changed.
    /// </summary>
    protected void StateHasChanged()
    {
        Component.StateHasChanged();
    }
}

/// <summary>
/// Base class for Blazor components' ViewModels that have a State.
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
        set
        {
            State = (TState)value;
            OnStateChanged(State);
        }
    }

    /// <summary>
    /// Type of this view model's state.
    /// </summary>
    public Type StateType => typeof(TState);

    /// <summary>
    /// Initializes this view model.
    /// </summary>
    public virtual void InitializeState() { }

    /// <summary>
    /// Initializes this view model.
    /// </summary>
    public virtual Task InitializeStateAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when the state has changed.
    /// </summary>
    protected virtual void OnStateChanged(TState state) { }
}
