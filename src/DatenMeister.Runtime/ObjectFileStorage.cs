using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DatenMeister.Runtime
{
    public class ObjectFileStorage<T> where T : class
    {
        public static T Load(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fileStream);
            }
        }

        public static void Save(string filePath, T collection)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fileStream, collection);
            }
        }
    }
}