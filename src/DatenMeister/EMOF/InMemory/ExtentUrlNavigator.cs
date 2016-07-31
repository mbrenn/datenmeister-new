using System;
using System.Collections.Generic;
using System.Diagnostics;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.InMemory
{
    public class ExtentUrlNavigator<T> where T : class, IElement, IHasId
    {
        private readonly Dictionary<string, IHasId> _cacheIds = new Dictionary<string, IHasId>();

        private IUriExtent _extent;

        public ExtentUrlNavigator(IUriExtent extent)
        {
            _extent = extent;
        }

        public virtual T element(string uri)
        {

            // Check, if the element is in the cache and if yes, return it
            IHasId result;
            if (_cacheIds.TryGetValue(uri, out result))
            {
                var resultAsMof = result as T;
                if (this.uri(resultAsMof) == uri)
                {
                    return resultAsMof;
                }
            }

            // Ok, not found, try to find it
            var uriAsUri = new Uri(uri);
            var fragment = uriAsUri.Fragment;
            if (string.IsNullOrEmpty(uriAsUri.Fragment))
            {
                throw new ArgumentException(
                    "Uri does not contain a URI-Fragment defining the object being looked for.",
                    nameof(uri));
            }

            // Queries the object
            var queryObjectId = fragment.Substring(1);

            // Now go through the list
            foreach (var element in AllDescendentsQuery.getDescendents(_extent))
            {
                var elementAsMofObject = element as IHasId;
                Debug.Assert(elementAsMofObject != null, "elementAsMofObject != null");

                if (elementAsMofObject.Id == queryObjectId)
                {
                    _cacheIds[uri] = elementAsMofObject;
                    return elementAsMofObject as T;
                }
            }

            // According to MOF Specification, return null, if not found
            return null;
        }

        public virtual string uri(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var elementAsObject = element as T;
            if (elementAsObject == null)
            {
                throw new InvalidOperationException($"element is not of type {typeof(T).Name}. Element is: " + element);
            }

            return _extent.contextURI() + "#" + elementAsObject.Id;
        }
    }
}