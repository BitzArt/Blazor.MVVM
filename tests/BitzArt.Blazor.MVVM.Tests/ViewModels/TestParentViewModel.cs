using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestParentViewModel : ViewModel<TestParentViewModelState>
{
    [Inject]
    public TestLayer1ViewModel TestLayer1ViewModel { get; set; } = null!;
}

public class TestParentViewModelState
{
    public string Title { get; set; } = "Title";
}
