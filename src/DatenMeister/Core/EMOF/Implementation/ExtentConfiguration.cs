using DatenMeister.Core.EMOF.Implementation.AutoEnumerate;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines the configuration of the extent being used to reflect MOF and UML properties
    /// </summary>
    public class ExtentConfiguration
    {
        /// <summary>
        /// Defines the type of the enumeration being used for properties with 'IsID == true'
        /// </summary>
        public AutoEnumerateType AutoEnumerateType { get; set; } = AutoEnumerateType.Ordinal;
    }
}