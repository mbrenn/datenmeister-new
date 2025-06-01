using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.EMOF.Implementation.DotNet;

/// <summary>
/// Performs an abstraction of the workspace to have an access to the types of the extents
/// </summary>
public class WorkspaceDotNetTypeLookup(Workspace workspace) : IDotNetTypeLookup
{
    /// <summary>
    /// Defines a cache between all objects and their id
    /// </summary>
    private readonly Dictionary<object, string> _cacheObjectToId = new();

    private readonly Dictionary<string, Type> _elementsToTypes = new();

    /// <summary>
    /// Stores a mapping between a .Net Type and the guid being used within the extent
    /// </summary>
    private readonly Dictionary<Type, string> _typesToElements = new();

    /// <summary>
    /// Adds an association between type and metaclassUri
    /// </summary>
    /// <param name="metaclassUri">Element to be added</param>
    /// <param name="type">Type to be added</param>
    public void Add(string metaclassUri, Type type)
    {
        if (_elementsToTypes.ContainsKey(metaclassUri))
        {
            throw new InvalidOperationException($"MetaclassUri was already associated: {metaclassUri}");
        }

        if (_typesToElements.ContainsKey(type))
        {
            throw new InvalidOperationException("Type was already associated: {type}");
        }

        _elementsToTypes[metaclassUri] = type;
        _typesToElements[type] = metaclassUri;
    }

    /// <summary>
    /// Gets the .Net type of the element and converts it
    /// to a EML Type information
    /// </summary>
    /// <param name="type">Type to be converted</param>
    /// <returns>The converted type or null, if the corresponding element is not found.
    /// Returns the metaclass to the given element</returns>
    public string? ToElement(Type type)
    {
        _typesToElements.TryGetValue(type, out var result);

        if (result == null)
        {
            return WorkspaceDotNetHelper.GetMetaClassUriOfDotNetType(workspace, type);
        }
            
        return result;
    }

    /// <inheritdoc />
    public Type? ToType(string metaclassUri)
    {
        _elementsToTypes.TryGetValue(metaclassUri, out var result);

        if (result == null)
        {
            return WorkspaceDotNetHelper.GetDotNetTypeOfMetaClassUri(workspace, metaclassUri);
        }
            
        return result;
    }

    /// <summary>
    /// Gets the id of a certain metaclassUri
    /// </summary>
    /// <param name="value">Value to be queried</param>
    /// <returns>The returned id</returns>
    public string GetId(object value)
    {
        lock (_cacheObjectToId)
        {
            if (!_cacheObjectToId.TryGetValue(value, out var id))
            {
                id = Guid.NewGuid().ToString();
                _cacheObjectToId[value] = id;
            }

            return id;
        }
    }
}