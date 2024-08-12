using System.Diagnostics.CodeAnalysis;

namespace BitzArt.Blazor.MVVM.Tests;

[SuppressMessage("Usage", "xUnit1031:Do not use blocking task operations in test method")]
public class ViewModelRequirementCollectionTests
{
    [Fact]
    public void WaitAsync_ForSimpleCondition_ShouldWaitUntilTrue()
    {
        // Arrange
        var requirementCollection = new ViewModelRequirementCollection();

        var condition = false;

        requirementCollection.Add(() => condition);

        // Act
        var task = requirementCollection.WaitAsync();

        // Assert
        task.Wait(100);
        Assert.False(task.IsCompleted);

        condition = true;
        task.Wait(100);
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void WaitAsync_ForConditionWithCancellationToken_ShouldWaitUntilCancelled()
    {
        // Arrange
        var requirementCollection = new ViewModelRequirementCollection();
        
        var condition = false;
        var cts = new CancellationTokenSource();

        requirementCollection.Add(() => condition, cts.Token);

        // Act
        var task = requirementCollection.WaitAsync();

        // Assert
        task.Wait(100);
        Assert.False(task.IsCompleted);

        condition = true;
        cts.Cancel();
        task.Wait(100);
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public async Task WaitAsync_WithCancellationTokenButConditionNeverTrue_ShouldThrow()
    {
        // Arrange
        var requirementCollection = new ViewModelRequirementCollection();

        var condition = false;
        var cts = new CancellationTokenSource();

        requirementCollection.Add(() => condition, cts.Token);

        // Act
        var task = requirementCollection.WaitAsync();

        // Assert
        task.Wait(100);
        Assert.False(task.IsCompleted);

        cts.Cancel();
        await Task.Delay(10);
        Assert.NotNull(task.Exception);
    }
}
