#nullable enable

using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Modules.Forms.FormFinder
{
    public class FindFormQuery
    {
        public FormType FormType { get; set; }

        public IElement? metaClass { get; set; }

        public string extentType { get; set; } = string.Empty;

        public IElement? parentMetaClass { get; set; }

        public string parentProperty { get; set; } = string.Empty;
    }
}