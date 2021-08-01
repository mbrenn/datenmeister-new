using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Json
{
    /// <summary>
    /// Defines some standard information about name, extentUri, fullName and id
    /// to have the most relevant information available for the web interface
    /// </summary>
    public record ItemWithNameAndId
    {
        public string name { get; set; } = string.Empty;

        public string extentUri { get; set; } = string.Empty;

        public string fullName { get; set; } = string.Empty;

        public string id { get; set; } = string.Empty;
        
        /// <summary>
        /// Creates the element out of the attached object 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ItemWithNameAndId? Create(IObject? value)
        {
            if (value == null)
            {
                return null;
            }
            
            var result = new ItemWithNameAndId
            {
                name = NamedElementMethods.GetName(value),
                extentUri = (value.GetExtentOf() as IUriExtent)?.contextURI() ?? string.Empty,
                fullName = NamedElementMethods.GetFullName(value),
                id = (value as IHasId)?.Id ?? string.Empty
            };

            return result;
        }

        public override string ToString()
        {
            return name;
        }
    }
}