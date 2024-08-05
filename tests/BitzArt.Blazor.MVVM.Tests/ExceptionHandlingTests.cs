using BitzArt.Blazor.MVVM.Tests.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM.Tests;

public class ExceptionHandlingTests
{
    [Fact]
    public async Task HandleAsync_WhenExceptionIsThrown_ShouldInvokeExceptionHandler()
    {
        // Arrange
        var viewModel = new TestLayer1ViewModel();
        var exceptionHandlerInvoked = false;

        viewModel.ExceptionHandler = (sender, ex) =>
        {
            exceptionHandlerInvoked = true;
            return Task.CompletedTask;
        };

        // Act
        try
        {
            await viewModel.HandleAsync(() => throw new Exception());
        }
        catch (Exception)
        {
        }

        // Assert
        Assert.True(exceptionHandlerInvoked);
    }

    [Fact]
    public async Task HandleAsync_HierarchicalStructure_ShouldForwardToRoot()
    {
        // Arrange

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm()
            .AddViewModel<TestLayer1ViewModel>()
            .AddViewModel<TestLayer2ViewModel>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var rootViewModel = serviceProvider.GetRequiredService<TestLayer1ViewModel>();
        var childViewModel = rootViewModel.TestLayer2ViewModel;

        var rootExceptionHandlerInvoked = false;
        rootViewModel.ExceptionHandler += (sender, ex) =>
        {
            rootExceptionHandlerInvoked = true;
            return Task.CompletedTask;
        };

        // Act
        try
        {
            await childViewModel.HandleAsync(() => throw new Exception());
        }
        catch (Exception)
        {
        }

        // Assert
        Assert.True(rootExceptionHandlerInvoked);
    }
}
