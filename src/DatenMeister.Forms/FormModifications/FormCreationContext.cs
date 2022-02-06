﻿using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Forms.FormModifications
{
    /// <summary>
    /// The additional information for which the form was created
    /// </summary>
    public record FormCreationContext
    {
        public _DatenMeister._Forms.___FormType? FormType { get; set; }

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
        public string? ExtentType { get; set; } = string.Empty;

        public string? ParentPropertyName { get; set; } = string.Empty;

        public IElement? ParentMetaClass { get; set; }

        /// <summary>
        /// The view mode to which the action button shall be added
        /// </summary>
        public string? ViewMode { get; set; }

        /// <summary>
        /// Checks whether two form creation contexts are missing.
        /// If a value is set in instance, then the value in template must be fitting
        /// </summary>
        /// <param name="template">Template which is used to evaluate whether an instance if fitting</param>
        /// <param name="instance">The specific instance to which the template shall be applied</param>
        /// <returns>true, if the two elements are fitting</returns>
        public static bool EvaluateMatching(FormCreationContext template, FormCreationContext instance)
        {
            return instance.FormType == null || template.FormType == instance.FormType &&
                instance.ExtentType == null || template.ExtentType == instance.ExtentType &&
                instance.DetailElement == null || template.DetailElement?.@equals(instance.DetailElement) == true &&
                instance.ParentPropertyName == null || template.ParentPropertyName == instance.ParentPropertyName &&
                instance.MetaClass == null || template.MetaClass?.@equals(instance.MetaClass) == true &&
                instance.ViewMode == null || template.ViewMode == instance.ViewMode &&
                instance.ParentMetaClass == null || template.ParentMetaClass?.@equals(instance.ParentMetaClass) == true;
        }
    }
    
    
}