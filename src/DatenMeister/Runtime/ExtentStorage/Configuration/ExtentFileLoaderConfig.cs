namespace DatenMeister.Runtime.ExtentStorage.Configuration
{
    public class ExtentFileStorageConfiguration : ExtentStorageConfiguration
    {
        /// <summary>
        /// Gets or sets the path to which the storage shall be stored
        /// </summary>
        public string Path { get; set; }

        /// <summary>Gibt eine Zeichenfolge zurück, die das aktuelle Objekt darstellt.</summary>
        /// <returns>Eine Zeichenfolge, die das aktuelle Objekt darstellt.</returns>
        public override string ToString()
        {
            return $"{GetType().Name} - {System.IO.Path.GetFileName(Path)}";
        }
    }
}