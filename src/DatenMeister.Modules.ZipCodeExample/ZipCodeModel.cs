using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Modules.ZipCodeExample;

public class ZipCodeModel
{
    /// <summary>
    /// Stores the path for the packages
    /// </summary>
    public const string PackagePath = "Apps::ZipCodes";

    /// <summary>
    /// Gets or sets the type for the zipcode
    /// </summary>
    public IElement? ZipCode { get; set; }

    /// <summary>
    /// Gets or sets the type for the zipcode
    /// </summary>
    public IElement? ZipCodeWithState { get; set; }

    public string? ZipCodeUri => ZipCode?.GetUri();
}