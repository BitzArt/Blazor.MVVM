using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestStatefulParentViewModel : ViewModel<TestStatefulParentViewModelState>
{
    [Inject]
    public TestNestedStatefulViewModel TestNestedStatefulViewModel { get; set; } = null!;
}

public class TestStatefulParentViewModelState
{
    public string Title { get; set; } = "Title";
}
