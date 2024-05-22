using BitzArt.Blazor.MVVM.Tests.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace BitzArt.Blazor.MVVM.Tests;

public class StateManagerTests
{/*
    [Fact]
    public void SerializeState_StatefulViewModel_ReturnsString()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestNestedStatefulViewModel>();

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
        var viewModel = serviceProvider.GetRequiredService<TestStatefulParentViewModel>();

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
        var viewModel = serviceProvider.GetRequiredService<TestNestedViewModel>();

        // Act
        var state = stateManager.SerializeState(viewModel);

        // Assert
        Assert.Null(state);
    }

    [Fact]
    public async Task RestoreStateAsync_StatefulViewModel_RestoresState()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestNestedStatefulViewModel>();

        var viewModelTitle = nameof(TestLayer1ViewModel);
        viewModel.State.Title = viewModelTitle;

        var state = stateManager.SerializeState(viewModel);
        viewModel.State = new();

        // Act
        await stateManager.RestoreStateAsync(viewModel, Encoding.UTF8.GetString(state!));

        // Assert
        Assert.Equal(viewModelTitle, viewModel.State.Title);
    }

    [Fact]
    public async Task RestoreStateAsync_StatefulViewModelWithNestedViewModels_RestoresStateHierarchy()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestStatefulParentViewModel>();
        var nestedViewModel = viewModel.TestNestedStatefulViewModel;

        var viewModelTitle = nameof(TestStatefulParentViewModel);
        viewModel.State.Title = viewModelTitle;

        var nestedViewModelTitle = nameof(TestNestedStatefulViewModel);
        nestedViewModel.State.Title = nestedViewModelTitle;

        var state = stateManager.SerializeState(viewModel);
        viewModel.State = new();
        nestedViewModel.State = new();

        // Act
        await stateManager.RestoreStateAsync(viewModel, Encoding.UTF8.GetString(state!));

        // Assert
        Assert.Equal(viewModelTitle, viewModel.State.Title);
        Assert.Equal(nestedViewModelTitle, nestedViewModel.State.Title);
    }

    [Fact]
    public async Task InitializeStateAsync_StatefulViewModel_InitializesState()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestNestedStatefulViewModel>();

        var viewModelTitle = viewModel.State.Title;
        viewModel.State.Title = nameof(TestNestedStatefulViewModel);

        // Act
        await stateManager.InitializeStateAsync(viewModel);

        // Assert
        Assert.Equal(viewModelTitle, viewModel.State.Title);
    }

    [Fact]
    public async Task InitializeStateAsync_StatefulViewModelWithNestedViewModels_InitializesStateHierarchy()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestStatefulParentViewModel>();
        var nestedViewModel = viewModel.TestNestedStatefulViewModel;

        var viewModelTitle = viewModel.State.Title;
        viewModel.State.Title = nameof(TestStatefulParentViewModel);

        var nestedViewModelTitle = nestedViewModel.State.Title;
        nestedViewModel.State.Title = nameof(TestNestedStatefulViewModel);

        // Act
        await stateManager.InitializeStateAsync(viewModel);

        // Assert
        Assert.Equal(viewModelTitle, viewModel.State.Title);
        Assert.Equal(nestedViewModelTitle, nestedViewModel.State.Title);
    }*/
}
