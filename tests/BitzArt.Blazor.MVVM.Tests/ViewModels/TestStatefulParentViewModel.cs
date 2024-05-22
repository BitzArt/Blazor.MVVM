using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestStatefulParentViewModel : ViewModel<TestStatefulParentViewModelState>
{
    [NestViewModel]
    public TestNestedStatefulViewModel TestNestedStatefulViewModel { get; set; } = null!;
}

public class TestStatefulParentViewModelState : ComponentState
{
    public string Title { get; set; } = "Title";
}
