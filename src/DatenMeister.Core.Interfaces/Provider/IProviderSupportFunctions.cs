namespace DatenMeister.Core.Interfaces.Provider;

/// <summary>
/// This interface is to be implemented by all providers which support additional
/// functions which support the MofExtents and elements.
///
/// The implementation of this interface or the functions included in the ProviderSupportFunctions
/// is optional and the caller need to be able to handle a missing implementation
/// </summary>
public interface IProviderSupportFunctions
{
    /// <summary>
    /// Gets the function table of supported functions
    /// </summary>
    public ProviderSupportFunctions ProviderSupportFunctions { get; }
}