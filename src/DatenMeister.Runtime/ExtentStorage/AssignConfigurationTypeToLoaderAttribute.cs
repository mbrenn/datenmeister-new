using System;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This attribute is allocated the the classes implementing the interface IExtentStorage
    /// and defines the corresponding ConfigurationStorage being inherited from ExtentStorageConfiguration
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ConfiguredByAttribute : Attribute
    {
        public Type ConfigurationType { get; private set; }

        public ConfiguredByAttribute(Type configurationType)
        {
            ConfigurationType = configurationType;
        }
    }
}