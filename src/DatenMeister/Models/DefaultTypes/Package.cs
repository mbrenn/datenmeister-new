#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.DefaultTypes.Model
{
    public class Package
    {
        /// <summary>
        /// Gets or sets the name of the packaged element
        /// </summary>
        public string? name { get; set; }
        
        /// <summary>
        /// Gets or sets the packaged elements of the extent
        /// </summary>
        public IEnumerable<IElement>? packagedElement { get; set; }
    }
}