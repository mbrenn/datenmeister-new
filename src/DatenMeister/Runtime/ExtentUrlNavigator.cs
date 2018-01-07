using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Runtime
{
    /// <summary>
    /// Caches the ids to improve navigation
    /// </summary>
    /// <typeparam name="T">Type of the elements that are abstracted</typeparam>
    public class ExtentUrlNavigator<T> where T : class, IElement, IHasId
    {
        private readonly Dictionary<string, IHasId> _cacheIds = new Dictionary<string, IHasId>();

        private readonly MofUriExtent _extent;

        public ExtentUrlNavigator(MofUriExtent extent)
        {
            _extent = extent;
        }

        /// <summary>
        /// Gets the sub element by the uri or null, if the extent does not contain the uri, null is returned
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public virtual T element(string uri)
        {
            // Check, if the element is in the cache and if yes, return it
            if (_cacheIds.TryGetValue(uri, out var result))
            {
                var resultAsMof = result as T;
                if (this.uri(resultAsMof) == uri)
                {
                    return resultAsMof;
                }
            }

            // Checks, if hash or question
            var posQuestion = uri.IndexOf('?');
            var posHash = uri.IndexOf('#');
            var posExtentEnd = posQuestion == -1 ? posHash : posQuestion;
            var context = posExtentEnd == -1 ? string.Empty : uri.Substring(0, posExtentEnd);

            // Verifies that the extent is working
            if (string.IsNullOrEmpty(context)) 
            {
                return null;
            }

            // Verifies whether the context can be found in context uri or alternative Uris
            if (context != _extent.contextURI() && !_extent.AlternativeUris.Contains(context))
            {
                return null;
            }

            // Ok, not found, try to find it
            try
            {
                if (posHash != -1)
                {
                    var uriAsUri = new Uri(uri);

                    // Gets the fragment
                    var fragment = uriAsUri.Fragment;
                    if (string.IsNullOrEmpty(uriAsUri.Fragment))
                    {
                        Debug.WriteLine(
                            $"Uri does not contain a URI-Fragment defining the object being looked for. {nameof(uri)}");

                        return null;
                    }

                    // Queries the object
                    var queryObjectId = WebUtility.UrlDecode(fragment.Substring(1));

                    // Now go through the list
                    foreach (var element in AllDescendentsQuery.GetDescendents(_extent))
                    {
                        var elementAsMofObject = element as IHasId ?? throw new ArgumentException("elementAsMofObject");
                        if (elementAsMofObject.Id == queryObjectId)
                        {
                            _cacheIds[uri] = elementAsMofObject;
                            return elementAsMofObject as T;
                        }
                    }

                    // According to MOF Specification, return null, if not found
                    return null;
                }
                else if (posQuestion != -1)
                {
                    var fullName = uri.Substring(posQuestion + 1);
                    var found = NamedElementMethods.GetByFullName(_extent, fullName);
                    if (found != null)
                    {
                        _cacheIds[uri] = (IHasId) found;
                        return found as T;
                    }

                    return null;
                }
                else
                {
                    throw new NotImplementedException("No Hash and no Question");
                }
            }
            catch (UriFormatException exc)
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