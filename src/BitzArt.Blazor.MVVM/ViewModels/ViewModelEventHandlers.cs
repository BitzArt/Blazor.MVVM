using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM;

public delegate void StateInitializedHandler(StateInitializedEventArgs args);

public delegate Task StateInitializedAsyncHandler(StateInitializedEventArgs args);

public class StateInitializedEventArgs(
    ViewModel viewModel,
    ComponentState state,
    ComponentBase component
    ) : EventArgs
{

    /// <summary>
    /// State that was initialized.
    /// </summary>
    public ComponentState State { get; } = state;

    /// <summary>
    /// ViewModel that owns the state.
    /// </summary>
    public ViewModel ViewModel { get; set; } = viewModel;

    /// <summary>
    /// Component that triggered the state initialization.
    /// </summary>
    public ComponentBase Component { get; set; } = component;
}

internal delegate Task ComponentStateContainerWasSetAsyncHandler(ComponentStateContainer sender);

public delegate Task ComponentStateHasChangedHandler(object sender);

public delegate Task ViewModelExceptionHandler(object sender, Exception ex);
