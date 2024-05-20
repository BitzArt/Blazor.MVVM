using Microsoft.AspNetCore.Components;

namespace BitzArt.Blazor.MVVM.Tests.ViewModels;

public class TestParentWithDuplicateChildViewModel : ViewModel
{
    [Inject]
    public TestDuplicateChildViewModel Child1 { get; set; } = null!;

    [Inject]
    public TestDuplicateChildViewModel Child2 { get; set; } = null!;

    public TestParentWithDuplicateChildViewModel()
    {
        Child1.Something.DoSomething();
    }
}

public class TestDuplicateChildViewModel : ViewModel
{
}
