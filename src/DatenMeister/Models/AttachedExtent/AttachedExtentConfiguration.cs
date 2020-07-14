using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.AttachedExtent
{
    /// <summary>
    /// Defines the configuration being used to allow the extension of extents
    /// </summary>
    public class AttachedExtentConfiguration
    {
        public string? name { get; set; }
        public string? referencedWorkspace { get; set; }
        public string? referencedExtent { get; set; }
        public IElement? referenceType { get; set; }
        public string? referenceProperty { get; set; }
    }
}