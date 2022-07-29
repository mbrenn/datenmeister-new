#nullable enable

using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Forms.FormFinder
{
    public class FindFormQuery
    {
        public _DatenMeister._Forms.___FormType FormType { get; set; }

        public IElement? metaClass { get; set; }

        public string extentType { get; set; } = string.Empty;

        public IObject? parentMetaClass { get; set; }

        public string parentProperty { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the id of the view mode being used for the query
        /// </summary>
        public string viewModeId { get; set; } = string.Empty;

        public override string ToString()
        {
            var result = $"{FormType}";
            if (metaClass != null)
            {
                result += $", MetaClass: {NamedElementMethods.GetName(metaClass)}";
            }
            
            if (!string.IsNullOrEmpty(extentType))
            {
                result += $", ExtentType: {extentType}";
            }

            return result;
        }
    }
}