using BitzArt.Blazor.MVVM.Tests.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM.Tests;

public class ServiceRegistrationTests
{
    [Fact]
    public void AddBlazorMvvm_OnServiceCollection_ShouldRegisterViewModelFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddBlazorMvvm();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var factory = serviceProvider.GetRequiredService<IViewModelFactory>();

        Assert.NotNull(factory);
    }

    [Fact]
    public void AddBlazorMvvm_OnServiceCollectionWhenAlreadyRegistered_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddBlazorMvvm();

        // Act
        var act = serviceCollection.AddBlazorMvvm;

        // Assert
        Assert.ThrowsAny<Exception>(act);
    }

    [Fact]
    public void AddViewModels_OnBlazorMvvmBuilder_ShouldAddViewModelsFromAllAssemblies()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddBlazorMvvm().AddViewModels();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var factory = (ViewModelFactory)serviceProvider.GetRequiredService<IViewModelFactory>();

        Assert.NotNull(factory.GetInjectionMap(typeof(TestParentViewModel)));
        Assert.NotNull(factory.GetInjectionMap(typeof(TestLayer1ViewModel)));
        Assert.NotNull(factory.GetInjectionMap(typeof(TestLayer2ViewModel)));
    }

    [Fact]
    public void AddViewModel_OnBlazorMvvmBuilder_ShouldMapViewModelInjections()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddBlazorMvvm().AddViewModel<TestParentViewModel>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var factory = (ViewModelFactory)serviceProvider.GetRequiredService<IViewModelFactory>();

        Assert.NotNull(factory.GetInjectionMap(typeof(TestParentViewModel)));

        var parentInjectionMap = factory.GetInjectionMap(typeof(TestParentViewModel));

        Assert.Single(parentInjectionMap.Injections.Where(x => x.IsNestedViewModelInjection));
    }

    [Fact]
    public void ViewModelFactory_Create_ShouldBuildViewModelHierarchy()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        var viewModelTypes = new[]
        {
            typeof(TestParentViewModel),
            typeof(TestLayer1ViewModel),
            typeof(TestLayer2ViewModel)
        };
        serviceCollection.AddBlazorMvvm().AddViewModels(viewModelTypes);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act
        var parentViewModel = serviceProvider.GetRequiredService<TestParentViewModel>();

        // Assert
        Assert.NotNull(parentViewModel);
        Assert.IsType<TestParentViewModel>(parentViewModel);

        Assert.NotNull(parentViewModel.TestLayer1ViewModel);
        Assert.IsType<TestLayer1ViewModel>(parentViewModel.TestLayer1ViewModel);

        Assert.NotNull(parentViewModel.TestLayer1ViewModel.TestLayer2ViewModel);
        Assert.IsType<TestLayer2ViewModel>(parentViewModel.TestLayer1ViewModel.TestLayer2ViewModel);
    }

    [Fact]
    public void ViewModelFactory_Create_WithDuplicateChild_ShouldBuildViewModelHierarchy()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        var viewModelTypes = new[]
        {
            typeof(TestParentWithDuplicateChildViewModel),
            typeof(TestDuplicateChildViewModel)
        };
        serviceCollection.AddBlazorMvvm().AddViewModels(viewModelTypes);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act
        var parentViewModel = serviceProvider.GetRequiredService<TestParentWithDuplicateChildViewModel>();

        // Assert
        Assert.NotNull(parentViewModel);
        Assert.IsType<TestParentWithDuplicateChildViewModel>(parentViewModel);

        Assert.NotNull(parentViewModel.Child1);
        Assert.IsType<TestDuplicateChildViewModel>(parentViewModel.Child1);

        Assert.NotNull(parentViewModel.Child2);
        Assert.IsType<TestDuplicateChildViewModel>(parentViewModel.Child2);

        Assert.NotSame(parentViewModel.Child1, parentViewModel.Child2);
    }
}
