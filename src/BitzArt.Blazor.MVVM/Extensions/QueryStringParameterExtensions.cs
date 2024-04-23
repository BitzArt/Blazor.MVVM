using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Reflection;

namespace BitzArt.Blazor.MVVM;

internal static class QueryStringParameterExtensions
{
    // Apply the values from the query string to the current component
    public static void SetParametersFromQueryString<T>(this T component, NavigationManager navigationManager)
        where T : class
    {
        if (!Uri.TryCreate(navigationManager.Uri, UriKind.RelativeOrAbsolute, out var uri))
            throw new InvalidOperationException("The current url is not a valid URI. Url: " + navigationManager.Uri);

        // Parse the query string
        Dictionary<string, StringValues> queryString = QueryHelpers.ParseQuery(uri.Query);

        // Enumerate all properties of the component
        foreach (var property in GetProperties<T>())
        {
            // Get the name of the parameter to read from the query string
            var parameterName = GetQueryStringParameterName(property);
            if (parameterName == null)
                continue; // The property is not decorated by [QueryStringParameterAttribute]

            if (queryString.TryGetValue(parameterName, out var value))
            {
                // Convert the value from string to the actual property type
                var convertedValue = ConvertValue(value, property.PropertyType);
                property.SetValue(component, convertedValue);
            }
        }
    }

    private static object ConvertValue(StringValues value, Type type)
    {
        return Convert.ChangeType(value[0], type, CultureInfo.InvariantCulture)!;
    }

    private static PropertyInfo[] GetProperties<T>()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private static string? GetQueryStringParameterName(PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<QueryStringParameterAttribute>();
        if (attribute == null)
            return null;

        return attribute.Name ?? property.Name;
    }
}
