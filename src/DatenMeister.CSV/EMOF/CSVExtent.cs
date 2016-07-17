using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.CSV.EMOF
{
    public class CSVExtent : MofUriExtent
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