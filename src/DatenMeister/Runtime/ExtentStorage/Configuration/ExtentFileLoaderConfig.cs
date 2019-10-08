// ReSharper disable InconsistentNaming
namespace DatenMeister.Runtime.ExtentStorage.Configuration
{
    public class ExtentFileLoaderConfig : ExtentLoaderConfig
    {
        /// <summary>
        /// Gets or sets the path to which the storage shall be stored
        /// </summary>
        public string filePath { get; set; }

        /// <summary>Gibt eine Zeichenfolge zurück, die das aktuelle Objekt darstellt.</summary>
        /// <returns>Eine Zeichenfolge, die das aktuelle Objekt darstellt.</returns>
        public override string ToString() => $"{GetType().Name} - {System.IO.Path.GetFileName(filePath)}";
    }
}