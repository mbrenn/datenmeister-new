using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Extents.Configuration
{
    /// <summary>
    /// Stores the setting of the extent
    /// </summary>
    public class ExtentTypeSetting
    {
        public ExtentTypeSetting(string name)
        {
            this.name = name;
        }
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// Stores the metaclasses of the root elements which are preferred to get added
        /// to the extent. Other metaclasses are also possible, but these one are actively offered
        /// by the tool
        /// </summary>
        public List<IElement> rootElementMetaClasses { get; } = new List<IElement>(); 
    }
}