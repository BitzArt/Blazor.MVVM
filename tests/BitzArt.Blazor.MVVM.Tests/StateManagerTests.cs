using BitzArt.Blazor.MVVM.Tests.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM.Tests;

public class StateManagerTests
{
    [Fact]
    public void EncodeState_StatefulViewModel_ReturnsString()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        serviceCollection.AddBlazorStateManager();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<IStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestLayer1ViewModel>();

        // Act
        var encodedState = stateManager.EncodeState(viewModel);

        // Assert
        Assert.NotNull(encodedState);
    }

    [Fact]
    public void EncodeState_StatefulViewModelWithNestedViewModels_ReturnsString()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        serviceCollection.AddBlazorStateManager();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<IStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestParentViewModel>();

        // Act
        var encodedState = stateManager.EncodeState(viewModel);

        // Assert
        Assert.NotNull(encodedState);
    }

    [Fact]
    public void EncodeState_ViewModelWithoutState_ReturnsNull()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        serviceCollection.AddBlazorStateManager();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<IStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestLayer2ViewModel>();

        // Act
        var encodedState = stateManager.EncodeState(viewModel);

        // Assert
        Assert.Null(encodedState);
    }
}
