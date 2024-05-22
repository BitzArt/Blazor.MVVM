using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// ViewModel base class.
/// </summary>
public abstract class ViewModel
{
    [Inject]
    private protected IViewModelFactory ViewModelFactory { get; set; } = null!;

    [Inject]
    private protected IServiceProvider ServiceProvider { get; set; } = null!;

    internal ComponentSignature Signature { get; set; } = null!;

    protected TViewModel CreateNestedViewModel<TViewModel>()
        where TViewModel : ViewModel
    {
        var nestedSignature = Signature.NestNew();
        return ViewModelFactory.Create<TViewModel>(ServiceProvider, nestedSignature);
    }

    public delegate Task ComponentStateHasChangedHandler(object sender);
    public ComponentStateHasChangedHandler? OnComponentStateChanged { get; set; }

    /// <summary>
    /// Notifies the component that the state has changed.
    /// </summary>
    protected void ComponentStateHasChanged()
    {
        OnComponentStateChanged?.Invoke(this);
    }

    /// <summary>
    /// Called when this ViewModel's dependencies have been injected.
    /// </summary>
    protected internal virtual void OnDependenciesInjected() { }
}

/// <summary>
/// ViewModel base class with state.
/// </summary>
/// <typeparam name="TState"></typeparam>
public abstract class ViewModel<TState> : ViewModel, IStatefulViewModel
    where TState : ComponentState, new()
{
    public ViewModel()
    {
        State = new();
    }

    public bool IsStateInitialized { get; set; }

    private StateInitializedHandler _onStateInitializedHandler = null!;
    public StateInitializedHandler? OnStateInitialized
    {
        get => _onStateInitializedHandler;
        set
        {
            var initialValue = _onStateInitializedHandler;

            _onStateInitializedHandler = value!;
            if (IsStateInitialized)
            {
                var diff = value - initialValue;
                diff!.Invoke(this);
            }
        }
    }

    private StateInitializedAsyncHandler _onStateInitializedAsyncHandler = null!;
    public StateInitializedAsyncHandler? OnStateInitializedAsync
    {
        get => _onStateInitializedAsyncHandler;
        set
        {
            var initialValue = _onStateInitializedAsyncHandler;
            
            _onStateInitializedAsyncHandler = value!;
            if (IsStateInitialized)
            {
                var diff = value - initialValue;
                diff!.Invoke(this);
            }
        }
    }

    private TState _state = null!;

    /// <summary>
    /// Persistent state of this ViewModel.
    /// </summary>
    public TState State
    {
        get => _state;
        set => _state = value;
    }

    ComponentState IStatefulViewModel.State
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

public delegate void StateInitializedHandler(object sender);
public delegate Task StateInitializedAsyncHandler(object sender);
