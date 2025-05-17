#nullable enable

namespace DatenMeister.Forms.FormCreator
{
    /// <summary>
    /// Stores the creation mode
    /// </summary>
    [Flags]
    public enum CreationMode
    {
        /// <summary>
        /// Allows the creation of forms by the metaclass of the given extent
        /// </summary>
        ByMetaClass = 0x01,

        /// <summary>
        /// Allows the creation of forms by going through the properties
        /// </summary>
        ByPropertyValues = 0x02,

        /// <summary>
        /// Allows the creation of forms by going through the properties only if
        /// the element does not have a metaclass
        /// </summary>
        OnlyPropertiesIfNoMetaClass = 0x04,

        /// <summary>
        /// Adds the metaclass itself to the form
        /// </summary>
        AddMetaClass = 0x08,

        /// <summary>
        /// Creates only fields that are usable in a list form.
        /// So most of the time only 'TextFields'.
        /// </summary>
        ForTableForms = 0x10,
            
        /// <summary>
        /// This flag is evaluated by the list form reportCreator.
        /// Only properties which are common in all child classes will be included
        /// into the view
        /// </summary>
        OnlyCommonProperties = 0x20,
        
        /// <summary>
        /// Defines a flag whether all the fields shall be included as read only fields
        /// </summary>
        ReadOnly = 0x40,
        
        /// <summary>
        /// Used by the FormCreator in the method AddToFormByUmlElement.
        /// If there is already a field included containing the given property, then the new field is not added
        /// </summary>
        NoDuplicate = 0x80,

        /// <summary>
        /// Creates all properties that are possible
        /// </summary>
        All = ByMetaClass | ByPropertyValues | AddMetaClass,
    }

    public static class CreationModeExtensions
    {
        public static bool HasFlagFast(this CreationMode value, CreationMode flag)
        {
            return (value & flag) != 0;
        }
    }
}