namespace BitzArt.Blazor.MVVM;

public delegate void StateInitializedHandler(object sender);
public delegate Task StateInitializedAsyncHandler(object sender);

internal delegate Task ComponentStateContainerWasSetAsyncHandler(ComponentStateContainer sender);

public delegate Task ComponentStateHasChangedHandler(object sender);

public delegate Task ViewModelExceptionHandler(object sender, Exception ex);
