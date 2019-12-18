using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Gets or sets a structure defining the type of the new element
    /// but also the property to which the new element is associated
    /// </summary>
    public class DefaultTypeForNewElement
    {
        public string name { get; set; }
        
        public IElement metaClass { get; set; }

        public string parentProperty { get; set; }
    }
}