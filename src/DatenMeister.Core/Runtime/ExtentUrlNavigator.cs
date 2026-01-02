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

    private static int _cacheHit = 0;

    private static int _cacheMiss = 0;
    
    public static long CacheHit => _cacheHit;

    public static long CacheMiss => _cacheMiss;

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
        // For whatever reason, that single tag is not Uri compliant
        if (uri == "http://www.omg.org/spec/MOF/20131001#Tag") 
            return null;
        
        // Now perform the actual execution
        var posQuestion = uri.IndexOf('?');
        var posHash = uri.IndexOf('#');
        var posExtentEnd = posQuestion == -1 ? posHash : posQuestion;
        var extentUri = posExtentEnd == -1 ? uri : uri.Substring(0, posExtentEnd);

        var isThisExtent = extentUri == extent.contextURI() ||
                           (extent is IHasAlternativeUris hasAlt && hasAlt.AlternativeUris.Contains(extentUri));

        // Case 1: Only the extent itself is requested (no # or ?)
        if (posQuestion == -1 && posHash == -1)
        {
            if (isThisExtent) return extent;

            // Fallback for convenience: try to find an element with the given ID within this extent
            return element("#" + uri);
        }

        // Case 2: Extent URI is provided but does not match this extent
        if (!string.IsNullOrEmpty(extentUri) && !isThisExtent)
        {
            return null;
        }

        // Case 3: Extent URI is empty, but it's not a relative URI starting with # or ?
        if (string.IsNullOrEmpty(extentUri) && posExtentEnd != 0)
        {
            return null;
        }

        var queryString = ParseQueryString(uri, posQuestion, posHash);
        object? foundItem = null;

        // Querying by hash
        if (posHash != -1)
        {
            var fragment = uri.Substring(posHash + 1);
            if (!string.IsNullOrEmpty(fragment))
            {
                var fragmentKey = extentUri + "#" + fragment;

                // Check cache
                if (_cacheIds.TryGetValue(fragmentKey, out var result))
                {
                    foundItem = result;
                    Interlocked.Increment(ref _cacheHit);
                }
                else
                {
                    foundItem = ResolveByFragment(fragment);
                    if (foundItem is MofElement mofFound)
                    {
                        _cacheIds[fragmentKey] = mofFound;
                        Interlocked.Increment(ref _cacheMiss);
                    }
                }
            }
        }

        // Handle Query String (Hooks)
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

    /// <summary>
    /// Parses the query string from a given URI, extracting key-value pairs based on the positions
    /// of the question mark and hash within the URI. It just returns the string between the question
    /// mark and the hash symbol
    /// </summary>
    /// <param name="uri">The URI containing the query string to be parsed.</param>
    /// <param name="posQuestion">The position of the question mark ('?') in the URI.</param>
    /// <param name="posHash">The position of the hash ('#') in the URI.</param>
    /// <returns>
    /// A <see cref="NameValueCollection"/> containing the parsed key-value pairs from the query string.
    /// If no query string is present, an empty collection is returned.
    /// </returns>
    /// <remarks>
    /// If the query string does not contain key-value pairs (e.g., no '=' character is present),
    /// a default key "fn" is assigned to the entire query string value.
    /// </remarks>
    /// <exception cref="UriFormatException">
    /// Thrown if the URI format is invalid during parsing. In such cases, an empty collection is returned.
    /// </exception>
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
    /// <param name="fragment">Fragment being required for the query. This fragment is without the '#'</param>
    /// <returns>The found element</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private MofElement? ResolveByFragment(string fragment)
    {
        // Queries the object
        var queryObjectId = HttpUtility.UrlDecode(fragment);

        // Check if the extent type already supports the direct querying of objects
        if (extent is MofExtent mofExtent &&
            mofExtent.Provider is IProviderSupportFunctions supportFunctions &&
            supportFunctions.ProviderSupportFunctions.QueryById != null)
        {
            var resultingObject = supportFunctions.ProviderSupportFunctions.QueryById(queryObjectId);
            if (resultingObject != null)
            {
                var resultElement = new MofElement(resultingObject, mofExtent)
                    { Extent = mofExtent };
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

    /// <summary>
    /// Gets the uri of the element
    /// </summary>
    /// <param name="element">Element to be queried</param>
    /// <returns>The uri of the element</returns>
    public virtual string uri(IElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        if (element is not IHasId elementAsObject)
        {
            throw new InvalidOperationException("element is not of type IHasId. Element is: " + element);
        }

        return extent.contextURI() + "#" + HttpUtility.UrlEncode(elementAsObject.Id);
    }
}