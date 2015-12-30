using System;

namespace DatenMeister.Runtime.ExtentStorage
{
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