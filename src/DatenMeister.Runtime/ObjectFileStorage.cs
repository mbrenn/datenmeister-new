﻿using System;
using System.IO;
using System.Xml.Serialization;

namespace DatenMeister.Runtime
{
    /// <summary>
    /// Is capable to load a instance of the given type T and stores it into a file as a serialized Xml. 
    /// </summary>
    /// <typeparam name="T">Typeof the instance to be serialized</typeparam>
    public class ObjectFileStorage<T> where T : class
    {
        /// <summary>
        /// Defines a list of types that might get to be serialized. 
        /// Since the serializer needs to know which types might be loaded during the serialization, 
        /// all possibly inherited types need to be returned within this overloadable method
        /// </summary>
        /// <returns>The types that might be serialized/deserialized</returns>
        public virtual Type[] GetAdditionalTypes()
        {
            return null;
        }

        public T Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T), GetAdditionalTypes());
                return (T)serializer.Deserialize(fileStream);
            }
        }

        public void Save(string filePath, T collection)
        {
            var directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(T), GetAdditionalTypes());
                serializer.Serialize(fileStream, collection);
            }
        }
    }
}