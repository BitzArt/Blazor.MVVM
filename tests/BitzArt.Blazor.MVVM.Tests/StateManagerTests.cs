using BitzArt.Blazor.MVVM.Tests.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

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

    [Fact]
    public async Task RestoreStateAsync_StatefulViewModel_RestoresState()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestLayer1ViewModel>();

        var subtitle = nameof(TestLayer1ViewModel);
        viewModel.State.Subtitle = subtitle;

        var state = stateManager.SerializeState(viewModel);
        viewModel.State = new();

        // Act
        await stateManager.RestoreStateAsync(viewModel, Encoding.UTF8.GetString(state!));

        // Assert
        Assert.Equal(subtitle, viewModel.State.Subtitle);
    }

    [Fact]
    public async Task RestoreStateAsync_StatefulViewModelWithNestedViewModels_RestoresStateHierarchy()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestParentViewModel>();
        var nestedViewModel = viewModel.TestLayer1ViewModel;

        var title = nameof(TestParentViewModel);
        viewModel.State.Title = title;

        var subtitle = nameof(TestLayer1ViewModel);
        nestedViewModel.State.Subtitle = subtitle;

        var state = stateManager.SerializeState(viewModel);
        viewModel.State = new();
        nestedViewModel.State = new();

        // Act
        await stateManager.RestoreStateAsync(viewModel, Encoding.UTF8.GetString(state!));

        // Assert
        Assert.Equal(title, viewModel.State.Title);
        Assert.Equal(subtitle, nestedViewModel.State.Subtitle);
    }

    [Fact]
    public async Task InitializeStateAsync_StatefulViewModel_InitializesState()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestParentViewModel>();

        var title = viewModel.State.Title;
        viewModel.State.Title = nameof(TestParentViewModel);

        // Act
        await stateManager.InitializeStateAsync(viewModel);

        // Assert
        Assert.Equal(title, viewModel.State.Title);
    }

    [Fact]
    public async Task InitializeStateAsync_StatefulViewModelWithNestedViewModels_InitializesStateHierarchy()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm().AddViewModels();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var stateManager = serviceProvider.GetRequiredService<BlazorViewModelStateManager>();
        var viewModel = serviceProvider.GetRequiredService<TestParentViewModel>();
        var nestedViewModel = viewModel.TestLayer1ViewModel;

        var title = viewModel.State.Title;
        viewModel.State.Title = nameof(TestParentViewModel);

        var subtitle = nestedViewModel.State.Subtitle;
        nestedViewModel.State.Subtitle = nameof(TestLayer1ViewModel);

        // Act
        await stateManager.InitializeStateAsync(viewModel);

        // Assert
        Assert.Equal(title, viewModel.State.Title);
        Assert.Equal(subtitle, nestedViewModel.State.Subtitle);
    }
}
