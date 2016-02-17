namespace DatenMeister.DataLayer
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