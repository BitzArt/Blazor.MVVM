using System.Reflection;

namespace BitzArt.Blazor.MVVM;

internal class ViewModelInjection
{
    public PropertyInfo Property { get; set; }

    public Type DependencyType { get; set; }

    public virtual bool IsNestedViewModelInjection { get; set; }

    public virtual bool IsParentViewModelInjection { get; set; }

    public virtual bool IsServiceInjection { get; set; }

    public ViewModelInjection(
        PropertyInfo property,
        Type dependencyType,
        bool nest = false,
        bool parent = false,
        bool service = false)
    {
        Property = property;
        DependencyType = dependencyType;

        if (!nest && !parent && !service) throw new InvalidOperationException(
            "ViewModelInjection must be either a nested view model, parent view model, or service injection.");

        IsNestedViewModelInjection = nest;
        IsParentViewModelInjection = parent;
        IsServiceInjection = service;
    }

    public static ViewModelInjection Nest(PropertyInfo property, Type dependencyType)
        => new(property, dependencyType, nest: true);

    public static ViewModelInjection Parent(PropertyInfo property, Type dependencyType)
        => new(property, dependencyType, parent: true);

    public static ViewModelInjection Service(PropertyInfo property, Type dependencyType)
        => new(property, dependencyType, service: true);
}