namespace DatenMeister.Core.Provider;

public class ProviderSupportFunctions
{
    /// <summary>
    /// Performs a query by id and the provider returns the given element
    /// </summary>
    public Func<string, IProviderObject?>? QueryById;
}