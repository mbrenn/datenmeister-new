using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;

namespace DatenMeister.Provider.CSV.EMOF
{
    public class CSVExtent : InMemoryUriExtent
    {
        /// <summary>
        /// Gets or sets the metaclass being used to create the elements within the extent
        /// </summary>
        public IElement MetaClass { get; set; }

        public CSVExtent(string uri) : base(uri)
        {
        }
    }
}