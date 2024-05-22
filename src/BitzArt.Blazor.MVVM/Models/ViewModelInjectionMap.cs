using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BitzArt.Blazor.MVVM;

internal class ViewModelInjectionMap
{
    public Type ViewModelType { get; set; }

    public string RegistrationKey { get; set; }

    public IEnumerable<ViewModelInjection> Injections { get; set; }

    [SuppressMessage("Style", "IDE0290:Use primary constructor")]
    public ViewModelInjectionMap(Type viewModelType, string registrationKey)
    {
        ViewModelType = viewModelType;
        Injections = ParseInjections(viewModelType);
        RegistrationKey = registrationKey;
    }

    private static List<ViewModelInjection> ParseInjections(Type viewModelType)
    {
        var result = new List<ViewModelInjection>();

        AddServiceInjections(viewModelType, result);
        AddNestInjections(viewModelType, result);
        AddParentInjections(viewModelType, result);

        return result;
    }

    private static void AddServiceInjections(Type viewModelType, List<ViewModelInjection> list)
    {
        var injectProperties = viewModelType.GetProperties(
            BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic)
            .Where(x => x.GetCustomAttribute<InjectAttribute>() is not null);

        foreach (var prop in injectProperties)
        {
            var dependencyType = prop.PropertyType;

            list.Add(ViewModelInjection.Service(prop, dependencyType));
        }
    }

    private static void AddNestInjections(Type viewModelType, List<ViewModelInjection> list)
    {
        var nestProperties = viewModelType.GetProperties(
            BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic)
            .Where(x => x.GetCustomAttribute<NestViewModelAttribute>() is not null);

        foreach (var prop in nestProperties)
        {
            if (!typeof(ViewModel).IsAssignableFrom(prop.PropertyType)) throw new InvalidOperationException(
                $"Property {prop.Name} in {viewModelType.Name} is not a ViewModel.");
            var dependencyType = prop.PropertyType;

            list.Add(ViewModelInjection.Nest(prop, dependencyType));
        }
    }

    private static void AddParentInjections(Type viewModelType, List<ViewModelInjection> list)
    {
        var parentProperties = viewModelType.GetProperties(
            BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic)
            .Where(x => x.GetCustomAttribute<ParentViewModelAttribute>() is not null);

        foreach (var prop in parentProperties)
        {
            if (!typeof(ViewModel).IsAssignableFrom(prop.PropertyType)) throw new InvalidOperationException(
                $"Property {prop.Name} in {viewModelType.Name} is not a ViewModel.");
            var dependencyType = prop.PropertyType;

            list.Add(ViewModelInjection.Parent(prop, dependencyType));
        }
    }
}
