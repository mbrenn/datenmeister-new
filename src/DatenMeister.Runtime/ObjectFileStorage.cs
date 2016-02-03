using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DatenMeister.Runtime
{
    public class ObjectFileStorage<T> where T : class
    {
        public static T Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fileStream);
            }
        }

        public static void Save(string filePath, T collection)
        {
            var directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fileStream, collection);
            }
        }
    }
}