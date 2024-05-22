using BitzArt.Blazor.MVVM.Tests.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM.Tests;

public class StateManagerTests
{
    [Fact]
    public void SerializeState_StatefulViewModel_ReturnsString()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestLayer1ViewModel>();

        // Act
        var state = stateManager.SerializeState(viewModel);

        // Assert
        Assert.NotNull(state);
    }

    [Fact]
    public void SerializeState_StatefulViewModelWithNestedViewModels_ReturnsString()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestParentViewModel>();

        // Act
        var state = stateManager.SerializeState(viewModel);

        // Assert
        Assert.NotNull(state);
    }

    [Fact]
    public void SerializeState_ViewModelWithoutState_ReturnsNull()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestLayer2ViewModel>();

        // Act
        var state = stateManager.SerializeState(viewModel);

        // Assert
        Assert.Null(state);
    }
}
