namespace DatenMeister.Runtime.ExtentStorage.Interfaces
{
    public class ExtentFileStorageConfiguration : ExtentStorageConfiguration
    {
        /// <summary>
        /// Gets or sets the path to which the storage shall be stored
        /// </summary>
        public string Path { get; set; }
    }
}