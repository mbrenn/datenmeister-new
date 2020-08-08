namespace DatenMeister.Runtime.ExtentStorage
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
        public class Migration
        {
            /// <summary>
            /// Performs the translation of the configuration type
            /// </summary>
            /// <param name="oldName">Old name of the configuration</param>
            /// <returns>Translated name</returns>
            public static string TranslateLegacyConfigurationType(string oldName)
            {
                return oldName switch
                {
                    "DatenMeister.Provider.Xml.XmlReferenceSettings" =>
                    "DatenMeister.Provider.Xml.XmlReferenceLoaderConfig",
                    "DatenMeister.Provider.XMI.ExtentStorage.XmiStorageConfiguration" =>
                    "DatenMeister.Provider.XMI.ExtentStorage.XmiStorageLoaderConfig",
                    "DatenMeister.Excel.Helper.ExcelImportSettings" =>
                    "DatenMeister.Excel.Helper.ExcelImportLoaderConfig",
                    "DatenMeister.Excel.Helper.ExcelReferenceSettings" =>
                    "DatenMeister.Excel.Helper.ExcelReferenceLoaderConfig",
                    "DatenMeister.Excel.Helper.ExcelSettings" => "DatenMeister.Excel.Helper.ExcelLoaderConfig",
                    _ => oldName
                };
            }
        }
    }
}