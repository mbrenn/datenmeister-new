using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Reports
{
    /// <summary>
    /// Defines the report source, consisting of name and collection
    /// </summary>
    public class ReportSource
    {
        public ReportSource(string name, IReflectiveCollection collection)
        {
            Name = name;
            Collection = collection;
        }

        /// <summary>
        /// Gets or sets the name of the source
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the collection
        /// </summary>
        public IReflectiveCollection Collection { get; set; }
    }
}