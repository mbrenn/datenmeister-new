namespace DatenMeister.Core.EMOF.Implementation;

public interface IHasExtentConfiguration
{
    /// <summary>
    /// Stores the configuration for the extent
    /// </summary>
    ExtentConfiguration ExtentConfiguration { get; }
}