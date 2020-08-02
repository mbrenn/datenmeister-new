using System;
using System.IO;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;

namespace DatenMeister.DotNet
{
    public static class ResourceHelper
    {
        public static string LoadStringFromAssembly(Type typeInAssembly, string resourcePath)
        {
            using (var stream =
                typeInAssembly.Assembly.GetManifestResourceStream(resourcePath))
            {
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException("Stream is empty")))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Loads an xml document from the assembly
        /// </summary>
        /// <param name="typeInAssembly">Type containing the assembly in which the resource is located</param>
        /// <param name="resourcePath">Path to the resource</param>
        /// <returns>Document to be located</returns>
        public static XDocument LoadXmlFromAssembly(Type typeInAssembly, string resourcePath)
        {
            using (var stream =
                typeInAssembly.Assembly.GetManifestResourceStream(resourcePath))
            {
                return XDocument.Load(stream);
            }
        }

        public static IObject LoadElementFromResource(Type typeInAssembly, string resourcePath)
        {
            var xml = LoadXmlFromAssembly(typeInAssembly, resourcePath);
            var xmlProvider = new XmiProvider(xml);

            var extent = new MofExtent(xmlProvider);
            var element = new MofElement(
                xmlProvider.CreateProviderObject(xml.Root), extent);

            return element;
        }
    }
}