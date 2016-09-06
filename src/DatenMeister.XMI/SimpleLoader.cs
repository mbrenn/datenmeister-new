using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.XMI.Standards;

namespace DatenMeister.XMI
{
    public class SimpleLoader
    {
        /// <summary>
        ///     Stores the factory to be used to create the instances
        /// </summary>
        private readonly IFactory _factory;

        private readonly Dictionary<string, IElement> _idToElement = new Dictionary<string, IElement>();

        /// <summary>
        ///     Initializes a new instance of the Loader class.
        /// </summary>
        /// <param name="factory">Factory to be used</param>
        public SimpleLoader(
            IFactory factory)
        {
            _factory = factory;
        }

        public void Load(IUriExtent extent, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                Load(extent, stream);
            }
        }

        /// <summary>
        ///     Loads the file from a stream
        /// </summary>
        /// <param name="extent">Extent to which the data is loaded</param>
        /// <param name="stream">Stream to be used for loading</param>
        public void Load(IUriExtent extent, Stream stream)
        {
            var document = XDocument.Load(stream);
            Load(extent, document);
        }

        /// <summary>
        ///     Loads the document from an XDocument
        /// </summary>
        /// <param name="extent">Extent to which the data is loaded</param>
        /// <param name="document">Document to be loaded</param>
        public void Load(IUriExtent extent, XDocument document)
        {
            _idToElement.Clear();

            // Skip the first element
            foreach (var element in document.Elements().Elements())
            {
                extent.elements().add(LoadElement(element));
            }
        }

        /// <summary>
        ///     Loads the specific element with a very simple loading algorithm
        /// </summary>
        /// <param name="element"></param>
        private IObject LoadElement(XElement element)
        {
            var resultingElement = _factory.create(null);
            foreach (var attribute in element.Attributes())
            {
                resultingElement.set(attribute.Name.ToString(), attribute.Value);
            }

            // Check, if element has id
            var xmiId = XmiId.Get(element);
            if (xmiId != null)
            {
                // In some Xmi files, the Xmi-id is used multiple times (e.g. Uml). We only assign it the first time
                if (!_idToElement.ContainsKey(xmiId))
                {
                    _idToElement[xmiId] = resultingElement;

                    var resultSetId = resultingElement as ICanSetId;
                    if (resultSetId != null)
                    {
                        resultSetId.Id = xmiId;
                    }
                }
            }

            var dict = new Dictionary<string, List<object>>();

            foreach (var subElement in element.Elements())
            {
                var name = subElement.Name.ToString();
                List<object> currentList;
                if (dict.ContainsKey(name))
                {
                    currentList = dict[name];
                }
                else
                {
                    currentList = new List<object>();
                    dict[name] = currentList;
                    resultingElement.set(name, currentList);
                }

                if (subElement.HasElements || subElement.HasAttributes)
                {
                    var loadedElement = LoadElement(subElement);

                    // Sets the container being used
                    var asSetContainer = loadedElement as IElementSetContainer;
                    asSetContainer?.setContainer(resultingElement);

                    // Adds the item to the current list
                    currentList.Add(loadedElement);
                }
                else
                {
                    currentList.Add(subElement.Value);
                }
            }

            return resultingElement;
        }
    }
}