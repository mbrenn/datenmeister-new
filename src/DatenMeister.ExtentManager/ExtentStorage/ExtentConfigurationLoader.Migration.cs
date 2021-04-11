using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.ExtentManager.ExtentStorage
{
    /// <summary>
    /// This loader is used to store and load the extent storage out of a file.
    /// In addition, it will also use the ExtentManager class to load the actual data
    /// of the extents
    /// </summary>
    public partial class ExtentConfigurationLoader
    {
        /// <summary>
        /// Just a namespace for the migration things
        /// </summary>
        private static class Migration
        {
            /// <summary>
            /// Performs the translation of the configuration type
            /// </summary>
            /// <param name="oldName">Old name of the configuration</param>
            /// <returns>Translated name</returns>
            public static IElement TranslateLegacyConfigurationType(IElement oldName)
            {
                return oldName;
            }
        }
    }
}