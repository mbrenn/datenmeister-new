using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DatenMeister.EMOF.InMemory
{
    public class MofUriExtent : IUriExtent
    {
        /// <summary>
        /// Stores all the elements
        /// </summary>
        List<object> _elements = new List<object>();
        
        private string _contextUri;

        public MofUriExtent(string uri)
        {
            _contextUri = uri;
        }

        public string contextURI()
        {
            return _contextUri;
        }

        public IElement element(string uri)
        {
            var uriAsUri = new Uri(uri);
            if (string.IsNullOrEmpty(uriAsUri.Fragment))
            {
                throw new ArgumentException(
                    "Uri does not contain a URI-Fragment defining the object being looked for.",
                    nameof(uri));
            }

            // Queries the object
            var queryObjectId = uriAsUri.Fragment.Substring(1);
            
            // Now go through the list
            foreach (var element in elements())
            {
                var elementAsMofObject = element as MofElement;
                Debug.Assert(elementAsMofObject != null, "elementAsMofObject != null");

                if (elementAsMofObject.guid.ToString() == queryObjectId)
                {
                    return elementAsMofObject;
                }
            }

            // According to MOF Specification, return null, if not found
            return null;
        }

        public string uri(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var elementAsObject = element as MofObject;
            if (elementAsObject == null)
            {
                throw new InvalidOperationException("element is not of type MofObject. Element is: " + element.ToString());
            }

            return _contextUri + "#" + elementAsObject.guid.ToString();
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
        {
            return new MofReflectiveSequence(_elements);
        }
    }
}
