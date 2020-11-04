using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Extents.Configuration
{
    /// <summary>
    /// Defines an additional property of the extent.
    /// This extent property will be set via name to the extent itself.
    /// A new instance of the metaclass will be created
    /// </summary>
    public class ExtentPropertyDefinition
    {
        /// <summary>
        /// Gets the name of the extent property definition
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// Gets the title to be shown
        /// </summary>
        public string title { get; set; } = string.Empty;

        /// <summary>
        /// Gets the metaclass to be created
        /// </summary>
        public IElement? metaClass { get; set; }
    }
}