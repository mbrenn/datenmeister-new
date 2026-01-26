using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class RowFilterOnMetaClass : ProxyReflectiveCollection
{
    private readonly IElement[]? _filteredMetaClass;
    private readonly string[]? _filteredMetaClassUris;

    public RowFilterOnMetaClass(IReflectiveCollection collection, IElement? filteredMetaClass)
        : base(collection)
    {
        _filteredMetaClass = filteredMetaClass == null ? null : [filteredMetaClass];

        if (filteredMetaClass is MofObject mofObject
            && mofObject.GetUri() != null)
        {
            _filteredMetaClassUris = [mofObject.GetUri()!];
        }

        if (filteredMetaClass is MofObjectShadow shadow)
        {
            _filteredMetaClassUris = [shadow.Uri];
        }
    }

    public RowFilterOnMetaClass(IReflectiveCollection collection, IElement[] filteredMetaClass)
        : base(collection)
    {
        _filteredMetaClass = filteredMetaClass;
        if (filteredMetaClass.All(x => x is MofObject { ProviderObject.MetaclassUri: not null } || x is MofObjectShadow))
        {
            _filteredMetaClassUris =
                _filteredMetaClass.Select(x =>
                {
                    if (x is MofObjectShadow shadow) return shadow.Uri;
                    return (x as MofObject)?.ProviderObject.MetaclassUri ?? string.Empty;
                }).ToArray();
        }
    }

    public override IEnumerator<object> GetEnumerator()
    {
        foreach (var value in Collection)
        {
            if (value is IElement valueAsObject && IsInList(valueAsObject))
            {
                yield return valueAsObject;
            }
        }
    }

    public override int size()
    {
        var result = 0;
        foreach (var value in Collection)
        {
            if (value is not IElement valueAsObject) 
                continue;
                
            if (IsInList(valueAsObject))
            {
                result++;
            }
        }

        return result;
    }

    /// <summary>
    /// Verifies whether the element shall be given in the list
    /// </summary>
    /// <param name="valueAsObject">Value to be shown</param>
    /// <returns>true, if value is in</returns>
    private bool IsInList(IElement valueAsObject)
    {
        var isIn = false;
        if (_filteredMetaClassUris != null && valueAsObject is MofObject valueAsMofObject)
        {
            isIn = _filteredMetaClassUris.Any(x => x.Equals(valueAsMofObject.ProviderObject.MetaclassUri));
        }
        else
        {
            var metaClass = valueAsObject.getMetaClass();

            if (metaClass == null && (_filteredMetaClass == null || _filteredMetaClass.Length == 0)
                || metaClass != null && _filteredMetaClass?.Any(x => x.equals(metaClass)) == true)
            {
                isIn = true;
            }
        }

        return isIn;
    }
}