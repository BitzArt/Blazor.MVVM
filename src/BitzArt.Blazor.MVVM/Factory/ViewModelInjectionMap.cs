using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace BitzArt.Blazor.MVVM;

public class ViewModelInjectionMap
{
    public Type ViewModelType { get; set; }

    public string RegistrationKey { get; set; }

    public IEnumerable<ViewModelInjection> Injections { get; set; }

    public ViewModelInjectionMap(Type viewModelType, string registrationKey)
    {
        ViewModelType = viewModelType;
        Injections = ParseInjections(viewModelType);
        RegistrationKey = registrationKey;
    }

    private static IEnumerable<ViewModelInjection> ParseInjections(Type viewModelType)
    {
        var properties = viewModelType.GetProperties()
            .Where(x => x.GetCustomAttribute<InjectAttribute>() is not null)
            .Where(x => typeof(ViewModel).IsAssignableFrom(x.PropertyType));

        return properties.Select(x => new ViewModelInjection(x, x.PropertyType));
    }
}
