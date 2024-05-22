namespace BitzArt.Blazor.MVVM;

// TODO: Implement ParentAttribute
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class ParentViewModelAttribute : Attribute
{
}
