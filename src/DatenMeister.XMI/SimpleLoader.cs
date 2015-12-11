using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.XMI
{
    public class SimpleLoader
    {
        /// <summary>
        ///     Stores the factory to be used to create the instances
        /// </summary>
        private readonly IFactory _factory;

        /// <summary>
        ///     Stores the workspace to find the meta types
        /// </summary>
        private Workspace<IUriExtent> _metaWorkspace;

        /// <summary>
        ///     Initializes a new instance of the Loader class.
        /// </summary>
        /// <param name="extent">Extent to be used</param>
        /// <param name="factory">Factory to be used</param>
        public SimpleLoader(
            IFactory factory,
            Workspace<IUriExtent> metaWorkspace = null)
        {
            _factory = factory;
            _metaWorkspace = metaWorkspace;
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
        /// <param name="stream">Stream to be used for loading</param>
        public void Load(IUriExtent extent, Stream stream)
        {
            var document = XDocument.Load(stream);
            Load(extent, document);
        }

        /// <summary>
        ///     Loads the document from an XDocument
        /// </summary>
        /// <param name="document">Document to be loaded</param>
        public void Load(IUriExtent extent, XDocument document)
        {
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
            var result = _factory.create(null);
            foreach (var attribute in element.Attributes())
            {
                result.set(attribute.Name.ToString(), attribute.Value);
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
                    result.set(name, currentList);
                }

                if (subElement.HasElements || subElement.HasAttributes)
                {
                    currentList.Add(LoadElement(subElement));
                }
                else
                {
                    currentList.Add(subElement.Value);
                }
            }

            return result;
        }
    }
}