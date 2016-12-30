using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.InMemory;

namespace DatenMeister.Provider.ManualMapping
{
    public class MMReflectiveCollection : InMemoryReflectiveSequence{
        /// <summary>
        /// Initializes a new instance of the MMReflectiveCollection and sets the extent
        /// </summary>
        /// <param name="localExtent"></param>
        public MMReflectiveCollection(IUriExtent localExtent) : base(localExtent, null)
        {
        }
    }
}