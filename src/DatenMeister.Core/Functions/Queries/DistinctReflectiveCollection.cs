using System.Collections;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Functions.Queries
{
    /// <summary>
    /// Returns a collection that only returns the distinct elements of a property
    /// </summary>
    public sealed class DistinctReflectiveCollection : TemporaryReflectiveCollection
    {
        public DistinctReflectiveCollection(IEnumerable collection, string property)
        {
            foreach (var element in collection.OfType<IObject>().Select(x => x.get(property)).Distinct())
            {
                if (element != null)
                {
                    add(element);
                }
            }
        }
    }
}