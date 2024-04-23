namespace BitzArt.Blazor.MVVM;

/// <summary>
/// Attribute to decorate properties that should be read from the query string.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class QueryStringParameterAttribute : Attribute
{
    /// <summary>Name of the query string parameter. It uses the property name by default.</summary>
    public string? Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStringParameterAttribute"/> class.
    /// </summary>
    public QueryStringParameterAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStringParameterAttribute"/> class.
    /// </summary>
    /// <param name="name"></param>
    public QueryStringParameterAttribute(string name)
    {
        Name = name;
    }
}
