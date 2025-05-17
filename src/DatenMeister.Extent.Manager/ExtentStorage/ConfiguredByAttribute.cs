namespace DatenMeister.Extent.Manager.ExtentStorage
{
    /// <summary>
    /// This attribute is allocated the the classes implementing the interface IProviderLoader
    /// and defines the corresponding ConfigurationStorage being inherited from ExtentLoaderConfig.
    /// The autoloading of all classes having this attribute is not defined within this library/assembby since it
    /// is portable and AppDomains are not supported by the Portable Framework. They are currently defined in 'DatenMeister.Full.Integration.Integration'.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfiguredByAttribute : Attribute
    {
        public Type ConfigurationType { get; private set; }

        public ConfiguredByAttribute(Type configurationType)
        {
            ConfigurationType = configurationType;
        }
    }
}