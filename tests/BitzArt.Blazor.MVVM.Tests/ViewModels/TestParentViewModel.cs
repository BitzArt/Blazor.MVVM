using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestParentViewModel : ViewModel
{
    [NestViewModel]
    public TestLayer1ViewModel TestLayer1ViewModel { get; set; } = null!;
}
