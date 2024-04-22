using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Base class for Blazor view models.
/// </summary>
public abstract class PageViewModel
{
    internal IPersistentComponent Component { get; set; } = null!;

    /// <summary>
    /// Initializes this view model.
    /// </summary>
    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    internal abstract Task PersistStateAsync();
}

/// <summary>
/// Base class for Blazor view models with state.
/// </summary>
/// <typeparam name="TState"></typeparam>
public abstract class PageViewModel<TState> : PageViewModel
    where TState : class, new()
{
    private const string StateKey = "_state";

    /// <summary>
    /// The component's persistent state.
    /// </summary>
    [Inject]
    public PersistentComponentState ComponentState { get; private set; } = null!;

    private TState? _state;

    /// <summary>
    /// Persistent state of this view model.
    /// </summary>
    public TState State => _state is not null ? _state! : throw new Exception("The state is not initialized.");

    /// <summary>
    /// Initializes this view model with its persistent state.
    /// </summary>
    public override async Task InitializeAsync()
    {
        var stateExists = Component.ComponentState.TryTakeFromJson<TState>(StateKey, out var state);

        if (stateExists) _state = state!;

        if (_state is null)
        {
            _state = new();
            InitializeState();
            await InitializeStateAsync();
        }
    }

    /// <summary>
    /// Initializes the state of this view model.
    /// </summary>
    protected virtual void InitializeState()
    {
        return;
    }

    /// <summary>
    /// Initializes the state of this view model.
    /// </summary>
    protected virtual Task InitializeStateAsync()
    {
        return Task.CompletedTask;
    }

    internal override Task PersistStateAsync()
    {
        Component.ComponentState!.PersistAsJson(StateKey, _state);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Notifies the component that the state has changed.
    /// </summary>
    protected void StateHasChanged()
    {
        Component.StateHasChanged();
    }
}
