using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.Standards;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.XMI
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

        /// <summary>
        /// Loads the xmi from the embedded resources
        /// </summary>
        /// <param name="extent">Extent being loaded</param>
        /// <param name="resourceName">Path to the resources</param>
        public void LoadFromEmbeddedResource(IUriExtent extent, string resourceName)
        {
            using (var stream = typeof(WorkspaceNames).GetTypeInfo().Assembly.GetManifestResourceStream(resourceName))
            {
                LoadFromStream(extent, stream);
            }
        }

        public void LoadFromFile(IUriExtent extent, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                LoadFromStream(extent, stream);
            }
        }

        /// <summary>
        ///     Loads the file from a stream
        /// </summary>
        /// <param name="extent">Extent to which the data is loaded</param>
        /// <param name="stream">Stream to be used for loading</param>
        public void LoadFromStream(IUriExtent extent, Stream stream)
        {
            var document = XDocument.Load(stream);
            LoadFromDocument(extent, document);
        }

        /// <summary>
        ///     Loads the document from an XDocument
        /// </summary>
        /// <param name="extent">Extent to which the data is loaded</param>
        /// <param name="document">Document to be loaded</param>
        public void LoadFromDocument(IUriExtent extent, XDocument document)
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

            var dict = new Dictionary<string, IReflectiveCollection>();

            foreach (var subElement in element.Elements())
            {
                var name = subElement.Name.ToString();
                IReflectiveCollection currentList;
                if (dict.ContainsKey(name))
                {
                    currentList = dict[name];
                }
                else
                {
                    resultingElement.set(name, new List<object>());
                    currentList = resultingElement.GetAsReflectiveCollection(name);
                    dict[name] = currentList;
                }

                if (subElement.HasElements || subElement.HasAttributes)
                {
                    var loadedElement = LoadElement(subElement);

                    // Sets the container being used
                    var asSetContainer = loadedElement as IElementSetContainer;
                    asSetContainer?.setContainer(resultingElement);

                    // Adds the item to the current list
                    currentList.add(loadedElement);
                }
                else
                {
                    currentList.add(subElement.Value);
                }
            }

            return resultingElement;
        }
    }
}