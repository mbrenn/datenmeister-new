using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Json
{
    /// <summary>
    /// This enumeration method is a helper for the GetContainer method and supports to
    /// describe the type of the element which is reflected in ItemWithNameAndId.
    /// Default is the Item, but if the calling method knows that there is an extent
    /// or a workspace described, the corresponding type can be set. 
    /// </summary>
    public enum EntentType
    {
        Item, 
        Extent,
        Workspace
    }
    
    /// <summary>
    /// Defines some standard information about name, extentUri, fullName and id
    /// to have the most relevant information available for the web interface
    /// </summary>
    public record ItemWithNameAndId
    {
        public string? name { get; set; } = string.Empty;

        public string? uri { get; set; } = string.Empty;

        public string? extentUri { get; set; } = string.Empty;

        public string? fullName { get; set; } = string.Empty;

        public string? id { get; set; } = string.Empty;

        public string? workspace { get; set; } = string.Empty;

        public string? typeName { get; set; } = string.Empty;

        public EntentType ententType { get; set; } = EntentType.Item;
        

        /// <summary>
        /// Creates the element out of the attached object 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ItemWithNameAndId? Create(IObject? value, EntentType ententType = EntentType.Item)
        {
            if (value == null)
            {
                return null;
            }

            var extent = value.GetExtentOf() as IUriExtent;
            var metaClass = (value as IElement)?.metaclass;

            var result = new ItemWithNameAndId
            {
                name = NamedElementMethods.GetName(value),
                extentUri = extent?.contextURI() ?? string.Empty,
                fullName = NamedElementMethods.GetFullName(value),
                id = (value as IHasId)?.Id ?? string.Empty,
                uri = value.GetUri() ?? string.Empty,
                workspace = extent?.GetWorkspace()?.id ?? string.Empty,
                typeName = metaClass == null ? null :  NamedElementMethods.GetName(metaClass),
                ententType = ententType
            };

            return result;
        }

        public override string ToString()
        {
            return name ?? string.Empty;
        }
    }
}