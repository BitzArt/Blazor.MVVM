using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestNestedStatefulViewModel : ViewModel<TestNestedStatefulViewModelState>
{
    [Inject]
    public TestNestedViewModel TestNestedViewModel { get; set; } = null!;
}

public class TestNestedStatefulViewModelState
{
    public string? Title { get; set; } = "Title";
}