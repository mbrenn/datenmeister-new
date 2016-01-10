using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Serialization;

namespace DatenMeister.Runtime.ExtentStorage
{
    public class ExtentStorageConfigurationLoader
    {
        public ExtentStorageConfigurationCollection Load(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(ExtentStorageConfigurationCollection));
                return (ExtentStorageConfigurationCollection) serializer.Deserialize(fileStream);
            }
        }

        public void Save(string filePath, ExtentStorageConfigurationCollection collection)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(ExtentStorageConfigurationCollection));
                serializer.Serialize(fileStream, collection);
            }
        }
    }
}