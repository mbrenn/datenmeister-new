using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class ColumnFilterExclude : ProxyReflectiveCollection
{
    /// <summary>
    /// Stores the excluded columns
    /// </summary>
    public HashSet<string> ExcludedColumns { get; set; } = [];
    
    public ColumnFilterExclude(IReflectiveCollection collection) : base(collection)
    {
    }

    public override IEnumerator<object?> GetEnumerator()
    {
        var factory = new MofFactory(Collection);
        foreach (var value in Collection)
        {
            if (value is IElement element  && value is IObjectAllProperties allProperties)
            {
                var memoryObject = factory.create(element.getMetaClass());
                foreach (var property in allProperties.getPropertiesBeingSet())
                {
                    if (!ExcludedColumns.Contains(property))
                    {
                        memoryObject.set(property, element.get(property));
                    }
                }

                yield return memoryObject;
            }
            else
            {
                yield return value;
            }
        }
    }

    public override int size()
    {
        return Collection.size();
    }
}