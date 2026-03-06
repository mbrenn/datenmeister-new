namespace DatenMeister.Core.TypeIndexAssembly.Model;

/// <summary>
/// Abstracts the information of the extent
/// </summary>
public class ExtentModel
{
    /// <summary>
    /// Stores the uri of the extent
    /// </summary>
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// Converts the extentmodel to a string 
    /// </summary>
    /// <returns>The found string</returns>
    public override string ToString()
    {
        return Uri;
    }
}