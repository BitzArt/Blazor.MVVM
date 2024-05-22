namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestLayer1ViewModel : ViewModel
{
    [NestViewModel]
    public TestLayer2ViewModel TestLayer2ViewModel { get; set; } = null!;
}