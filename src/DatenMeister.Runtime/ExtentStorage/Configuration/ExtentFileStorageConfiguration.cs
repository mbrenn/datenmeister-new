using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage.Configuration
{
    public class ExtentFileStorageConfiguration : ExtentStorageConfiguration
    {
        /// <summary>
        /// Gets or sets the path to which the storage shall be stored
        /// </summary>
        public string Path { get; set; }
    }
}