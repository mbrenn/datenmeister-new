using System;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    /// <summary>
    /// Returns a collection that only returns the distinct elements of a property
    /// </summary>
    public class DistinctReflectiveCollection : TemporaryReflectiveCollection
    {
        public DistinctReflectiveCollection(IReflectiveCollection collection, string property)
        {
            foreach (var element in collection.OfType<IObject>().Select(x => x.get(property)).Distinct())
            {
                add(element);
            }
        }
    }
}