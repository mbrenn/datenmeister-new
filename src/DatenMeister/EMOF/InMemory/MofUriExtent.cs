using System;
using System.Collections.Generic;
using System.Diagnostics;
using DatenMeister.EMOF.Attributes;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;

namespace DatenMeister.EMOF.InMemory
{
    [AssignFactoryForExtentType(typeof(MofFactory))]
    public class MofUriExtent : IUriExtent, IExtentCachesObject
    {
        private readonly object _syncObject = new object();

        private readonly string _contextUri;

        private readonly MofExtentReflectiveSequence _reflectiveSequence;

        /// <summary>
        ///     Stores all the elements
        /// </summary>
        private readonly List<object> _elements = new List<object>();

        private readonly Dictionary<string, IElement> _cacheIds = new Dictionary<string, IElement>();

        public MofUriExtent(string uri)
        {
            _contextUri = uri;
            _reflectiveSequence = new MofExtentReflectiveSequence(this, _elements);
        }

        public string contextURI()
        {
            return _contextUri;
        }

        public IElement element(string uri)
        {
            lock (_syncObject)
            {
                // Check, if the element is in the cache and if yes, return it
                IElement result;
                if (_cacheIds.TryGetValue(uri, out result))
                {
                    if (this.uri(result) == uri)
                    {
                        return result;
                    }
                }

                // Ok, not found, try to find it
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
                foreach (var element in AllDescendentsQuery.getDescendents(this))
                {
                    var elementAsMofObject = element as MofElement;
                    Debug.Assert(elementAsMofObject != null, "elementAsMofObject != null");

                    if (elementAsMofObject.guid.ToString() == queryObjectId)
                    {
                        _cacheIds[uri] = elementAsMofObject;
                        return elementAsMofObject;
                    }
                }

                // According to MOF Specification, return null, if not found
                return null;
            }
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
                throw new InvalidOperationException("element is not of type MofObject. Element is: " + element);
            }

            return _contextUri + "#" + elementAsObject.guid;
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
        {
            return _reflectiveSequence;
        }

        public bool HasObject(IObject value)
        {
            lock (_reflectiveSequence)
            {
                return _reflectiveSequence.HasObject(value);
            }
        }
    }
}