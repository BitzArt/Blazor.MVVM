namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

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
}
