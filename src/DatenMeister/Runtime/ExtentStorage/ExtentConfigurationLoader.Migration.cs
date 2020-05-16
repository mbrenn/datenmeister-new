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
                switch (oldName)
                {
                    case "DatenMeister.Provider.Xml.XmlReferenceSettings":
                        return "DatenMeister.Provider.Xml.XmlReferenceLoaderConfig";
                    case "DatenMeister.Provider.XMI.ExtentStorage.XmiStorageConfiguration":
                        return "DatenMeister.Provider.XMI.ExtentStorage.XmiStorageLoaderConfig";
                    case "DatenMeister.Excel.Helper.ExcelImportSettings":
                        return "DatenMeister.Excel.Helper.ExcelImportLoaderConfig";
                    case "DatenMeister.Excel.Helper.ExcelReferenceSettings":
                        return "DatenMeister.Excel.Helper.ExcelReferenceLoaderConfig";
                    case "DatenMeister.Excel.Helper.ExcelSettings":
                        return "DatenMeister.Excel.Helper.ExcelLoaderConfig";
                    default:
                        return oldName;
                }
            }
        }
    }
}