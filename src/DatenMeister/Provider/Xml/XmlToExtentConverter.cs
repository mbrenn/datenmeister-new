using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.Xml
{
    /// <summary>
    /// Converts an xml file to an extent and its elements
    /// </summary>
    public class XmlToExtentConverter
    {
        private readonly XmlReferenceLoaderConfig _loaderConfig;

        /// <summary>
        /// Initializes a new instance of the XmlToExtentConverter
        /// </summary>
        /// <param name="loaderConfig">Settings to be used</param>
        public XmlToExtentConverter(XmlReferenceLoaderConfig loaderConfig)
        {
            _loaderConfig = loaderConfig;
        }

        /// <summary>
        /// Converts the document to a mof extent
        /// </summary>
        /// <param name="document"></param>
        /// <param name="extent"></param>
        public void Convert(XDocument document, MofExtent extent)
        {
            Convert(document, extent.elements());
        }

        /// <summary>
        /// Converts the document to a mof extent
        /// </summary>
        /// <param name="document"></param>
        /// <param name="collection"></param>
        public void Convert(XDocument document, IReflectiveCollection collection)
        {
            var factory = new MofFactory(collection);
            foreach (var element in document.Elements())
            {
                var mofElement = factory.create(null);
                Convert(element, mofElement, string.Empty, factory);
                collection.add(mofElement);
            }
        }

        /// <summary>
        /// Converts the content of the element into the mof element
        /// </summary>
        /// <param name="xmlElement">Xml element to be converted</param>
        /// <param name="mofElement">Element which shall be filled</param>
        /// <param name="innerName">The name for hte inner element</param>
        /// <param name="factory">The MOF Factory being used</param>
        private void Convert(XElement xmlElement, IElement mofElement, string innerName, IFactory factory)
        {
            // Converts the attributes
            foreach (var attribute in xmlElement.Attributes())
            {
                var name = GetNameNormalized(attribute.Name);
                if (name == "xmlns")
                {
                    // Skip xmlns
                    continue;
                }

                mofElement.set(name, attribute.Value);
            }

            // Converts the elements
            var set = new Dictionary<string, List<object>>();

            foreach (var innerElement in xmlElement.Elements())
            {
                var name = GetNameNormalized(innerElement.Name);

                // Check, if the element also has subelements
                if (!innerElement.HasElements)
                {
                    mofElement.set(name, innerElement.Value);
                }
            }

            // Try to figure out the id
            var id = GuessId(mofElement);
            var localName = id;
            id = string.IsNullOrEmpty(innerName) ? id : $"{innerName}.{id}";
            if (mofElement is ICanSetId canSetId)
            {
                canSetId.Id = id;
            }

            // Now go through the more complex elements
            foreach (var innerElement in xmlElement.Elements())
            {
                var name = GetNameNormalized(innerElement.Name);

                // Check, if the element also has subelements
                if (innerElement.HasElements)
                {
                    var innerMofElement = factory.create(null);

                    if (!set.TryGetValue(name, out var list))
                    {
                        list = new List<object>();
                        set[name] = list;
                    }

                    Convert(innerElement, innerMofElement, id, factory);

                    list.Add(innerMofElement);
                }
            }

            // Sets the result
            foreach (var pair in set)
            {
                if (pair.Value.Count == 1)
                {
                    mofElement.set(pair.Key, pair.Value.First());
                }
                else
                {
                    mofElement.set(pair.Key, pair.Value);

                    if (!mofElement.isSet(_UML._CommonStructure._NamedElement.name))
                    {
                        mofElement.set(_UML._CommonStructure._NamedElement.name, pair.Key);
                    }
                }
            }

            if (!mofElement.isSet(_UML._CommonStructure._NamedElement.name) && localName != ((IHasId) mofElement).Id)
            {
                mofElement.set(_UML._CommonStructure._NamedElement.name, localName);
            }
        }

        private static string GuessId(IElement mofElement)
        {
            var result = ((IHasId) mofElement).Id;
            var name = mofElement.getOrDefault<string>("name");
            if (!string.IsNullOrEmpty(name))
            {
                result = name;
            }
            else
            {
                name = mofElement.getOrDefault<string>("Name");
                if (!string.IsNullOrEmpty(name))
                {
                    result = name;
                }
                else
                {
                    name = mofElement.getOrDefault<string>("title");
                    if (!string.IsNullOrEmpty(name))
                    {
                        result = name;
                    }
                    else
                    {
                        name = mofElement.getOrDefault<string>("Title");
                        if (!string.IsNullOrEmpty(name))
                        {
                            result = name;
                        }
                    }
                }
            }

            return result ?? string.Empty;
        }

        private string GetNameNormalized(XName xname)
        {
            var name = xname.ToString();
            if (!_loaderConfig.keepNamespaces)
            {
                name = xname.LocalName;

            }

            return name;
        }
    }
}