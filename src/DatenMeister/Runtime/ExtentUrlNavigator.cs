using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;

namespace DatenMeister.Runtime
{
    /// <summary>
    /// Caches the ids to improve navigation
    /// </summary>
    /// <typeparam name="T">Type of the elements that are abstracted</typeparam>
    public class ExtentUrlNavigator<T> where T : class, IElement, IHasId
    {
        private readonly Dictionary<string, IHasId> _cacheIds = new Dictionary<string, IHasId>();

        private readonly IUriExtent _extent;

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
            try
            {
                var uriAsUri = new Uri(uri);

                var fragment = uriAsUri.Fragment;
                if (string.IsNullOrEmpty(uriAsUri.Fragment))
                {
                    Debug.WriteLine(
                        $"Uri does not contain a URI-Fragment defining the object being looked for. {nameof(uri)}");

                    return null;
                }

                var posHash = uri.IndexOf('#');
                var left = uri.Substring(0, posHash);
                if (left != _extent.contextURI())
                {
                    return null;
                }


                // Queries the object
                var queryObjectId = WebUtility.UrlDecode(fragment.Substring(1));

                // Now go through the list
                foreach (var element in AllDescendentsQuery.GetDescendents(_extent))
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
            catch(UriFormatException exc)
            {
                Debug.WriteLine(
                    $"Exception while parsing URI {nameof(uri)}: {exc.Message}");

                return null;

            }
        }

        public virtual string uri(IElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var elementAsObject = element as IHasId;
            if (elementAsObject == null)
            {
                throw new InvalidOperationException($"element is not of type IHasId. Element is: " + element);
            }

            return _extent.contextURI() + "#" + WebUtility.UrlEncode(elementAsObject.Id);
        }
    }
}