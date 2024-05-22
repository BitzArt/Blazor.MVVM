using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestLayer1ViewModel : ViewModel<TestLayer1ViewModelState>
{
    [Inject]
    public TestLayer2ViewModel TestLayer2ViewModel { get; set; } = null!;
}

public class TestLayer1ViewModelState
{
    public string? Title { get; set; } = "Title";
}