using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Serialization;

namespace DatenMeister.Runtime.ExtentStorage
{
    public class ExtentStorageLoader
    {
        public ExtentStorageCollection Load(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(ExtentStorageCollection));
                return (ExtentStorageCollection) serializer.Deserialize(fileStream);
            }
        }

        public void Save(string filePath, ExtentStorageCollection collection)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(ExtentStorageCollection));
                serializer.Serialize(fileStream, collection);
            }
        }
    }
}