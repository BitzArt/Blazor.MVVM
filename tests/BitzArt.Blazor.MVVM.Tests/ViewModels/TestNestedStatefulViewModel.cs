using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestNestedStatefulViewModel : ViewModel<TestNestedStatefulViewModelState>
{
    [NestViewModel]
    public TestNestedViewModel TestNestedViewModel { get; set; } = null!;
}

public class TestNestedStatefulViewModelState : ComponentState
{
    public string? Title { get; set; } = "Title";
}