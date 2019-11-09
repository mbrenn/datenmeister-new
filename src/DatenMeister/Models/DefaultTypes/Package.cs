#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.DefaultTypes
{
    public class Package
    {
        /// <summary>
        /// Gets or sets the packaged elements of the extent
        /// </summary>
        public IEnumerable<IElement>? packagedElement { get; set; }
    }
}