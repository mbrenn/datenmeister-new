using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;

namespace DatenMeister.Runtime.Functions.Aggregation
{
    /// <summary>
    /// Performs a grouping by metaclass
    /// </summary>
    public class GroupByMetaclass : TemporaryReflectiveCollection
    {
        /// <summary>
        /// Stores the extent being used to provide the information
        /// </summary>
        private readonly MofUriExtent _extent = new MofUriExtent(new InMemoryProvider(), "datenmeister:///temp");

        private readonly IReflectiveCollection _reflectiveCollection;

        public GroupByMetaclass(
            IReflectiveCollection reflectiveCollection)
        {
            _reflectiveCollection = reflectiveCollection;

            PerformGrouping();
        }

        /// <summary>
        /// Performs the grouping
        /// </summary>
        private void PerformGrouping()
        {
            var factory = new MofFactory(_extent);

            var dictionary = new Dictionary<IElement, List<object>>();
            var rest = new List<object>();

            // Perform the grouping
            foreach (var value in _reflectiveCollection)
            {
                if (value == null)
                {
                    continue;
                }
                
                var element = value as IElement;
                var metaClass = element?.getMetaClass();
                if (metaClass == null)
                {
                    rest.Add(value);
                }
                else
                {
                    if (dictionary.TryGetValue(metaClass, out var list))
                    {
                        list.Add(value!);
                    }
                    else
                    {
                        list = new List<object> {value!};
                        dictionary[metaClass] = list;
                    }
                }
            }

            // Now convert to MOF
            foreach (var pair in dictionary)
            {
                var mofElement = factory.create(null);
                mofElement.set("metaclass", pair.Key);
                mofElement.set("elements", pair.Value);

                add(mofElement);
            }

            if (rest.Count > 0)
            {
                var mofElement = factory.create(null);
                mofElement.set("metaclass", null);
                mofElement.set("elements", rest);

                add(mofElement);
            }
        }
    }
}