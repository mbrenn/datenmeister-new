using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Forms.FormModifications
{
    /// <summary>
    /// The additional information for which the form was created
    /// </summary>
    public record FormCreationContext
    {
        public _DatenMeister._Forms.___FormType FormType { get; set; }

        /// <summary>
        /// Defines the configuration including the view mode
        /// </summary>
        public FormFactoryConfiguration? Configuration { get; set; }

        /// <summary>
        /// Gets or sets the element being used to create the form.
        /// This property is only filled, if the FormType is a detailform
        /// or a TreeItemDetail Form.
        /// For the ObjectList interface, the element containing the property is set within this field
        /// </summary>
        public IObject? DetailElement
        {
            get;
            set;
        }

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