using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.DataViews;
using DatenMeister.Provider;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Runtime
{
    /// <summary>
    /// Caches the ids to improve navigation
    /// </summary>
    public class ExtentUrlNavigator
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(ExtentUrlNavigator));

        private readonly ConcurrentDictionary<string, IHasId> _cacheIds = new ConcurrentDictionary<string, IHasId>();

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
        public virtual object? element(string uri)
        {
            // Checks, if hash or question. Unfortunately, we can't use the Uri since it 
            // Fragment only is accepted in relative Uris
            var posQuestion = uri.IndexOf('?');
            var posHash = uri.IndexOf('#');
            var posExtentEnd = posQuestion == -1 ? posHash : posQuestion;
            var extentUri = posExtentEnd == -1 ? string.Empty : uri.Substring(0, posExtentEnd);

            // Verifies that the extent is working. Hash or question mark must be on first character, if there is no 
            // extent
            if (string.IsNullOrEmpty(extentUri) && posExtentEnd != 0) return null;

            // Verifies whether the context can be found in context uri or alternative Uris if extent uri is set
            if (!string.IsNullOrEmpty(extentUri) &&
                extentUri != _extent.contextURI() &&
                !_extent.AlternativeUris.Contains(extentUri))
            {
                return null;
            }

            var queryString = ParseQueryString(uri, posQuestion);

            object? foundItem = null;
            // Ok, not found, try to find it
            if (posHash == -1 && posQuestion == -1)
            {
                Logger.Error("No hash and no question mark");
                throw new NotImplementedException("No hash and no question mark");
            }

            // Querying by hash
            if (posHash != -1)
            {
                // Gets the fragment
                var fragment = uri.Substring(posHash + 1);
                if (string.IsNullOrEmpty(fragment))
                {
                    Logger.Info(
                        $"Uri does not contain a URI-Fragment defining the object being looked for. {nameof(uri)}");
                }
                else
                {
                    var fragmentUri = 
                        extentUri 
                        + "#" + fragment;
                    
                    // Check, if the element is in the cache and if yes, return it
                    if (_cacheIds.TryGetValue(fragmentUri, out var result))
                    {
                        var resultAsMof = result as MofElement;
                        var comparedUri =
                            (string.IsNullOrEmpty(extentUri)
                                ? ""
                                : (resultAsMof!.GetUriExtentOf()?.contextURI() ?? string.Empty))
                            + ("#" + resultAsMof!.Id ?? string.Empty);
                        if (comparedUri == uri)
                        {
                            foundItem = resultAsMof;
                        }
                    }

                    if (foundItem == null)
                    {
                        foundItem = ResolveByFragment(fragment);

                        // Adds the found item to the cache
                        if (foundItem != null)
                        {
                            // Caching is only useful for fragments since the cache lookup 
                            // Tries to find an item by the element
                            _cacheIds[fragmentUri] = (MofElement) foundItem;
                        }
                    }
                }
            }

            // Querying by query
            var fullName = queryString.Get("fn");
            if (fullName != null)
            {
                foundItem = NamedElementMethods.GetByFullName(_extent, fullName);
            }

            // Checks whether we have a property
            var property = queryString.Get("prop");
            if (property != null && foundItem is MofElement mofElement)
            {
                foundItem = mofElement.getOrDefault<IReflectiveCollection>(property);
            }
            
            // Now check whether we have a dataview
            var dataview = queryString.Get("dataview");
            if (dataview != null && foundItem is IReflectiveCollection reflectiveCollection)
            {
                if (_extent.ScopeStorage == null)
                {
                    Logger.Error("Dataview queried but extent does not have Scope Storage set");
                    throw new InvalidOperationException("Dataview queried but extent does not have Scope Storage set");
                }
                var dataviewElement = _extent.ResolveElement(dataview, ResolveType.Default);
                if (dataviewElement == null)
                {
                    Logger.Warn($"Dataview was not found: {dataview}");
                }
                else
                {
                    var dataViewEvaluation = new DataViewEvaluation(_extent.ScopeStorage.Get<DataViewNodeFactories>()); 
                    dataViewEvaluation.AddDynamicSource("input", reflectiveCollection);
                    foundItem = dataViewEvaluation.GetElementsForViewNode(dataviewElement);
                }
            }
            
            return foundItem;
        }

        private static NameValueCollection ParseQueryString(string uri, int posQuestion)
        {
            try
            {
                // Parse the QueryString
                NameValueCollection parsedValue;
                if (posQuestion != -1)
                {
                    var query = uri.Substring(posQuestion + 1);
                    if (query.IndexOf('=') == -1)
                    {
                        // If there is no real query string, create one with full name
                        Logger.Info("Legacy query without fullname: " + uri);

                        query = "fn=" + query;
                    }

                    parsedValue = HttpUtility.ParseQueryString(query);
                }
                else
                {
                    parsedValue = new NameValueCollection();
                }

                return parsedValue;
            }
            catch (UriFormatException exc)
            {
                Logger.Error(
                    $"Exception while parsing URI {nameof(uri)}: {exc.Message}");
                return new NameValueCollection();
            }
        }

        /// <summary>
        /// Resolves the item by using the fragment (#)
        /// </summary>
        /// <param name="fragment">Fragment being required for the query</param>
        /// <returns>The found element</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private MofElement? ResolveByFragment(string fragment)
        {
            // Queries the object
            var queryObjectId = WebUtility.UrlDecode(fragment);

            // Check if the extent type already supports the direct querying of objects
            if (_extent.Provider is IProviderSupportFunctions supportFunctions &&
                supportFunctions.ProviderSupportFunctions.QueryById != null)
            {
                var resultingObject = supportFunctions.ProviderSupportFunctions.QueryById(queryObjectId);
                if (resultingObject != null)
                {
#if DEBUG
                    if (_extent == null) throw new InvalidOperationException("_extent is null");
#endif
                    var resultElement = new MofElement(resultingObject, _extent)
                        {Extent = _extent};
                    return resultElement;
                }
            }
            else
            {
                // Now go through the list
                foreach (var element in AllDescendentsQuery.GetDescendents(_extent))
                {
                    var elementAsMofObject =
                        element as IHasId ?? throw new ArgumentException("elementAsMofObject");
                    if (elementAsMofObject.Id == queryObjectId)
                    {
                        return elementAsMofObject as MofElement;
                    }
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

            var elementAsObject = element as IHasId;
            if (elementAsObject == null)
            {
                throw new InvalidOperationException($"element is not of type IHasId. Element is: " + element);
            }

            return _extent.contextURI() + "#" + WebUtility.UrlEncode(elementAsObject.Id);
        }
    }
}