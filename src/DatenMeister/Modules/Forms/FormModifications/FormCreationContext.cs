using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormFinder;

namespace DatenMeister.Modules.Forms.FormModifications
{
    /// <summary>
    /// The additional information for which the form was created
    /// </summary>
    public class FormCreationContext
    {
        public FormType FormType { get; set; }

        public FormDefinitionMode DefinitionMode { get; set; }

        /// <summary>
        /// Gets or sets the view mode.
        /// </summary>
        public string ViewMode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the metaclass that was used to create the form.
        /// For detail form, it is the metaclass of the element itself. 
        /// For the list form, it is the metaclass of the enumeration, as long as it is given
        /// For extents, it is not set
        /// For an object within an extent, it is the metaclass of the selected object
        /// </summary>
        public IElement? MetaClass { get; set; }

        /// <summary>
        /// Gets or sets the name of the extent
        /// </summary>
        public string ExtentType { get; set; } = string.Empty;

        public string ParentPropertyName { get; set; } = string.Empty;

        public IElement? ParentMetaClass { get; set; }
    }
}