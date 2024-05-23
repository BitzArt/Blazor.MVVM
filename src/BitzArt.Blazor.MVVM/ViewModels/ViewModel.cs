﻿using Microsoft.AspNetCore.Components;

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

    [Inject]
    protected RenderingEnvironment RenderingEnvironment { get; set; } = null!;

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
}