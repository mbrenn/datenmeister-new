using System.Collections.Generic;

namespace DatenMeister.Core.DataLayer
{
    /// <summary>
    /// Defines the datalayers being available. 
    /// </summary>
    public class DataLayer : IDataLayer
    {
        /// <summary>
        /// Gets the name of the datalayer
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a list the cache which stores the filled types
        /// </summary>
        internal List<object> FilledTypeCache { get; } = new List<object>();

        /// <summary>
        /// Initializes a new instance of the element including the name
        /// </summary>
        /// <param name="name">Name of the datalayer</param>
        public DataLayer(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}