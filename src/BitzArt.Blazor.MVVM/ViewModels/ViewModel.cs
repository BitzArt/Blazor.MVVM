using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM;

/// <summary>
/// ViewModel base class.
/// </summary>
public abstract class ViewModel
{
    // ==========================   INJECTIONS   ==========================

    [Inject]
    private protected IViewModelFactory ViewModelFactory { get; set; } = null!;

    [Inject]
    private protected IServiceProvider ServiceProvider { get; set; } = null!;

    // =====================   MVVM INTERNAL WIRING   =====================

    internal ComponentSignature Signature { get; set; } = null!;

    internal ComponentStateContainerWasSetAsyncHandler? OnComponentStateContainerWasSet { get; set; }

    internal ComponentStateContainer? ComponentStateContainer { get; set; }

    // ==============================   NESTING   ==============================

    protected TViewModel CreateNestedViewModel<TViewModel>()
        where TViewModel : ViewModel
    {
        var nestedSignature = Signature.NestNew();
        return ViewModelFactory.Create<TViewModel>(ServiceProvider, nestedSignature);
    }

    // ==========================   COMPONENT STATE   ==========================

    /// <summary>
    /// Called to notify and re-render the component.
    /// </summary>
    public ComponentStateHasChangedHandler? OnComponentStateChanged { get; set; }

    /// <summary>
    /// Invokes <see cref="OnComponentStateChanged"/>
    /// </summary>
    protected async Task ComponentStateChangedAsync()
    {
        if (OnComponentStateChanged is not null)
            await OnComponentStateChanged.Invoke(this);
    }

    // =============================   LIFECYCLE   =============================

    /// <summary>
    /// Called when ViewModel hierarchy is built and all dependencies are injected.
    /// </summary>
    protected internal virtual void OnDependenciesInjected() { }

    // =======================    EXCEPTION HANDLING    ========================

    /// <summary>
    /// Called when an exception is thrown and needs to be handled.
    /// </summary>
    public ViewModelExceptionHandler? ExceptionHandler { get; set; }

    /// <summary>
    /// Invokes <see cref="ExceptionHandler"/>
    /// </summary>
    public async Task HandleAsync(Exception ex)
    {
        if (ExceptionHandler is not null)
            await ExceptionHandler.Invoke(this, ex);
    }

    /// <summary>
    /// Notifies subscribed exception handlers in case of an exception.
    /// </summary>
    public async Task HandleAsync(Func<Task> action)
    {
        try
        {
            await action.Invoke();
        }
        catch (Exception ex)
        {
            await HandleAsync(ex);
            throw;
        }
    }

    /// <summary>
    /// Notifies subscribed exception handlers in case of an exception.
    /// </summary>
    /// <returns>Result of the action.</returns>
    public async Task<TResult> HandleAsync<TResult>(Func<Task<TResult>> action)
    {
        try
        {
            return await action.Invoke();
        }
        catch (Exception ex)
        {
            await HandleAsync(ex);
            throw;
        }
    }

    // =======================  REQUIREMENTS  ========================

    private readonly ViewModelRequirementCollection _requirements = new();

    /// <summary>
    /// Requires the specified condition to be met before this ViewModel can be used.
    /// </summary>
    /// <param name="condition">The condition to be met.</param>
    /// <param name="cancellationToken">Wait for condition cancellation token</param>
    public void Requires(Func<bool> condition, CancellationToken? cancellationToken = null)
        => _requirements.Add(condition, cancellationToken);

    /// <summary>
    /// Requires the specified ComponentState to be initialized
    /// before this ViewModel can be used.
    /// </summary>
    /// <param name="state">The ComponentState which should be initialized.</param>
    public void Requires(ComponentState state)
    {
        var cts = new CancellationTokenSource();
        state.OnInitializedAsync += (_)
            =>
        {
            cts.Cancel();
            return Task.CompletedTask;
        };

        Requires(() => state.IsInitialized, cts.Token);
    }

    /// <summary>
    /// Waits for all ViewModel requirements to be met.
    /// </summary>
    public async Task WaitForRequirementsAsync()
        => await _requirements.WaitAsync();
}