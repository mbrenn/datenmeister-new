using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Web;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Core.Runtime;

/// <summary>
/// Caches the ids to improve navigation
/// </summary>
public class ExtentUrlNavigator(IUriExtent extent, IScopeStorage? scopeStorage)
{
    private static readonly ClassLogger Logger = new(typeof(ExtentUrlNavigator));

    private readonly ConcurrentDictionary<string, IHasId> _cacheIds = new();

    private ResolveHookContainer? GetResolveHooks()
    {
        return scopeStorage?.Get<ResolveHookContainer>();
    }
        
    /// <summary>
    /// Clears the complete resolve cache
    /// </summary>
    public void ClearResolveCache()
    {
        _cacheIds.Clear();
    }
        
    /// <summary>
    /// Removes the element with the given id from the resolve cache  
    /// </summary>
    /// <param name="id">Id of the element to be removed. This id of the element
    /// shall not contain the id of the extent</param>
    public void RemoveFromResolveCache(string id)
    {
        // This is the easy implementation
        ClearResolveCache();
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
        var extentUri = posExtentEnd == -1 ? uri : uri.Substring(0, posExtentEnd);

        // Check, if the given extent contains the alternative uri
        var matchesAlternativeUri =
            extent is IHasAlternativeUris hasAlternativeUris && hasAlternativeUris.AlternativeUris.Contains(extentUri);
            
        // Checks, if the extent itself is selected
        if (posQuestion == -1 && posHash == -1
                              && (uri == extent.contextURI() || matchesAlternativeUri))
        {
            return extent;
        }

        if (posQuestion == -1 && posHash == -1)
        {
            // If element is not an extent, support the user by trying to find
            // an element with the given id within this extent
            var tryAsHash = element("#" + uri);
            return tryAsHash;
        }

        // Verifies that the extent is working. Hash or question mark must be on first character, if there is no 
        // extent
        if (string.IsNullOrEmpty(extentUri) && posExtentEnd != 0) return null;

        // Verifies whether the context can be found in context uri or alternative Uris if extent uri is set
        if (!string.IsNullOrEmpty(extentUri) &&
            extentUri != extent.contextURI() &&
            !matchesAlternativeUri)
        {
            return null;
        }

        var queryString = ParseQueryString(uri, posQuestion, posHash);

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
                        + ("#" + resultAsMof!.Id);
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
                        _cacheIds[fragmentUri] = (MofElement)foundItem;
                    }
                }
            }
        }

        if (queryString.AllKeys.Length > 0)
        {
            var resolveHook = GetResolveHooks();
            if (resolveHook != null)
            {
                var parameters = new ResolveHookParameters(scopeStorage, queryString, foundItem ?? extent, extent);

                foreach (var hook in resolveHook.ResolveHooks)
                {
                    parameters.CurrentItem = hook.Resolve(parameters);
                }

                foundItem = parameters.CurrentItem;
            }
        }

        return foundItem;
    }

    private static NameValueCollection ParseQueryString(string uri, int posQuestion, int posHash)
    {
        try
        {
            // Parse the QueryString
            NameValueCollection parsedValue;
            if (posQuestion != -1)
            {
                var query =
                    posHash == -1
                        ? uri.Substring(posQuestion + 1)
                        : uri.Substring(posQuestion + 1, posHash - posQuestion - 1);
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
        var queryObjectId = HttpUtility.UrlDecode(fragment);
            
        // Check if the extent type already supports the direct querying of objects
        if (
            extent is MofUriExtent mofUriExtent && 
            mofUriExtent.Provider is IProviderSupportFunctions supportFunctions &&
            supportFunctions.ProviderSupportFunctions.QueryById != null)
        {
            var resultingObject = supportFunctions.ProviderSupportFunctions.QueryById(queryObjectId);
            if (resultingObject != null)
            {
#if DEBUG
                if (extent == null) throw new InvalidOperationException("_extent is null");
#endif
                var resultElement = new MofElement(resultingObject, mofUriExtent)
                    { Extent = mofUriExtent };
                return resultElement;
            }
        }
        else
        {
            // Now go through the list
            foreach (var element in 
                     AllDescendentsQuery.GetDescendents(extent))
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
        ArgumentNullException.ThrowIfNull(element);

        var elementAsObject = element as IHasId;
        if (elementAsObject == null)
        {
            throw new InvalidOperationException("element is not of type IHasId. Element is: " + element);
        }

        return extent.contextURI() + "#" + HttpUtility.UrlEncode(elementAsObject.Id);
    }
}