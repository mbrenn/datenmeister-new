using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Functions.Queries;

public class ColumnFilterIncludeOnly : ProxyReflectiveCollection
{
    /// <summary>
    /// Stores the excluded columns
    /// </summary>
    public HashSet<string> IncludeColumns { get; set; } = [];
    
    public ColumnFilterIncludeOnly(IReflectiveCollection collection) : base(collection)
    {
    }

    public override IEnumerator<object?> GetEnumerator()
    {
        IFactory? factory = null;
        foreach (var value in Collection)
        {
            if (value is IElement element and IObjectAllProperties allProperties)
            {
                factory ??= new MofFactory(element);
                var memoryObject = factory.create(element.getMetaClass());
                foreach (var property in allProperties.getPropertiesBeingSet())
                {
                    if (IncludeColumns.Contains(property))
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