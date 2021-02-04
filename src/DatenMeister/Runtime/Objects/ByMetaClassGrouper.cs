using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.Xml;

namespace DatenMeister.Runtime.Objects
{
    /// <summary>
    /// Defines the metaclass 
    /// </summary>
    public class MetaClassGroup
    {
        public MetaClassGroup(IElement? metaClass)
        {
            MetaClass = metaClass;
        }

        /// <summary>
        /// Gets the metaclass
        /// </summary>
        public IElement? MetaClass { get; }

        /// <summary>
        /// Gets the elements
        /// </summary>
        public HashSet<IObject> Elements { get; } = new();
    }

    /// <summary>
    /// Supports the grouping by metaclass
    /// </summary>
    public class ByMetaClassGrouper
    {
        public static List<MetaClassGroup> Group(IReflectiveCollection collection)
        {
            var result = new List<MetaClassGroup>();

            foreach (var item in collection.OfType<IObject>())
            {
                var metaClass = (item as IElement)?.getMetaClass();
                var foundGroup = result.FirstOrDefault(x => x.MetaClass == metaClass);

                if (foundGroup == null)
                {
                    foundGroup = new MetaClassGroup(metaClass);
                    result.Add(foundGroup);
                }

                foundGroup.Elements.Add(item);
            }

            return result;
        }
    }
}