namespace DatenMeister.Core.EMOF.Implementation.AutoEnumerate;

/// <summary>
/// Defines the enumeration type that is used within the extent
/// </summary>
public enum AutoEnumerateType
{
    // No auto enumeration
    None, 
        
    /// <summary>
    /// IDs will retrieve a GUID
    /// </summary>
    Guid,
        
    /// <summary>
    /// IDs will retrieve an increasing number
    /// </summary>
    Ordinal
}