namespace DatenMeister.Runtime.ExtentStorage.Interfaces
{
    /// <summary>
    /// Maps the extent storage type to a configuration type which is used by the logic to find out the best type
    /// which can be used to satisfy a load request. 
    /// </summary>
    public interface IConfigurationToExtentStorageMapper
    {
        IExtentStorage CreateFor(ExtentStorageConfiguration configuration);
    }
}