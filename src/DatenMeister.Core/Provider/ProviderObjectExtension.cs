namespace DatenMeister.Core.Provider;

public static class ProviderObjectExtension
{
    /// <summary>
    /// Gets the property of the provider object and returns the first element of a list
    /// if the property contains a list
    /// </summary>
    /// <param name="value">providerObject to be evaluated</param>
    /// <param name="propertyName">Name of the property</param>
    /// <returns>The found property</returns>
    public static object? GetPropertyAsSingle(this IProviderObject value, string propertyName)
    {
        if (!value.IsPropertySet(propertyName))
        {
            return null;
        }
            
        var result = value.GetProperty(propertyName);
        if (result is IList<object> asList)
        {
            return asList.FirstOrDefault();
        }

        return result;
    }
}