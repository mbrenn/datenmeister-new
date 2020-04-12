namespace DatenMeister.Runtime.Extents.Configuration
{
    /// <summary>
    /// Stores the setting of the extent
    /// </summary>
    public class ExtentTypeSetting
    {
        public ExtentTypeSetting(string name)
        {
            Name = name;
        }
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
    }
}