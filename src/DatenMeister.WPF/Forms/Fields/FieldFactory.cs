using System.Windows;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields
{
    /// <summary>
    /// The field factory creates the corresponding fields out of the associated field data
    /// </summary>
    public static class FieldFactory
    {
        /// <summary>
        /// Stores the logger for the field factory
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(FieldFactory));

        /// <summary>
        /// Creates a specific field by the reading out the field type
        /// </summary>
        /// <param name="value">Value of the field being used</param>
        /// <param name="field">Field to be queried</param>
        /// <returns>Found field or exception if not found</returns>
        public static IDetailField CreateField(IObject value, IElement field)
        {
            var isEnumeration = field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration);
            // Get by field type
            var metaClass = field?.getMetaClass();
            if (metaClass == null)
            {
                throw new ArgumentException("Value does not have metaclass", nameof(value));
            }

            var id = (metaClass as IHasId)?.Id;
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__AnyDataFieldData))
                return new AnyDataField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__SeparatorLineFieldData))
                return new SeparatorLineField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__SubElementFieldData))
                return new SubElementsField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__DropDownFieldData))
                return new DropdownField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__CheckboxFieldData))
                return new CheckboxField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__DateTimeFieldData))
                return new DateTimeField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__ReferenceFieldData))
                return new ReferenceField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__TextFieldData))
                return new TextboxField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData))
                return new MetaClassElementField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__FileSelectionFieldData))
                return new FileSelectionField();
            if (metaClass.equals(_DatenMeister.TheOne.Forms.__CheckboxListTaggingFieldData))
                return new CheckboxListTaggingField();

            Logger.Warn("Unknown FieldData type for field creation: " + metaClass);

            if (isEnumeration)
            {
                return new ReadOnlyListField();
            }

            return new TextboxField();
        }

        /// <summary>
        /// Gets the UI Element for the given field and value
        /// </summary>
        /// <param name="value">Value of the field being used</param>
        /// <param name="field">Field to be queried</param>
        /// <param name="formControl">Form Control in which the UI Element will be hosted</param>
        /// <param name="flags">Flags of the field dependent on the view. For example a flag that specifies
        /// whether the element shall be focussed</param>
        /// <returns>The created element</returns>
        public static (IDetailField detailField, UIElement? element)
            GetUIElementFor(IObject value, IElement field, DetailFormControl formControl, FieldParameter flags)
        {
            var fieldElement = CreateField(value, field);
            var element = fieldElement.CreateElement(value, field, formControl, flags);

            return (fieldElement, element);
        }
    }

    /// <summary>
    /// Defines possible field parameter for the creation
    /// </summary>
    public class FieldParameter
    {
        /// <summary>
        /// Defines whether the current field could be focussed.
        /// That flag shall be erased by the detail control, if the flag is not focussed
        /// </summary>
        public bool CanBeFocused { get; set; }

        /// <summary>
        /// Gets or sets the information whether the element is spanned through the complete row
        /// </summary>
        public bool IsSpanned { get; set; }
        
        /// <summary>
        /// Gets or sets the information whether the field shall be readonly despite the form settings
        /// </summary>
        public bool IsReadOnly { get; set; }
    }

    /// <summary>
    /// Stores the form parameter
    /// </summary>
    public class FormParameter
    {
        /// <summary>
        /// Gets or sets the flag whether the fields are read-only
        /// </summary>
        public bool IsReadOnly { get; set; }
    }
}