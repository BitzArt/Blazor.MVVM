using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestLayer1ViewModel : ViewModel
{
    [Inject]
    public TestLayer2ViewModel TestLayer2ViewModel { get; set; } = null!;
}