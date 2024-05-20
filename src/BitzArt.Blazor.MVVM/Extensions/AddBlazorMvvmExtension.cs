using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Blazor.MVVM;

public static class AddBlazorMvvmExtension
{
    public static IBlazorMvvmBuilder AddBlazorMvvm(this IServiceCollection services)
        => new BlazorMvvmBuilder(services, new ViewModelFactory());
}
