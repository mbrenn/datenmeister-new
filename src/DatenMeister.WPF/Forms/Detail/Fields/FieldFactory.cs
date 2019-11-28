using System;
using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.Model;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Detail.Fields
{
    public static class FieldFactory
    {
        /// <summary>
        /// Creates a specific field by the reading out the field type
        /// </summary>
        /// <param name="value">Value of the field being used</param>
        /// <param name="field">Field to be queried</param>
        /// <returns>Found field or exception if not found</returns>
        public static IDetailField CreateField(IObject value, IElement field)
        {
            var fieldType = field.getOrDefault<string>(_FormAndFields._FieldData.fieldType);
            var isEnumeration = field.getOrDefault<bool>(_FormAndFields._FieldData.isEnumeration);
            if (fieldType != null)
            {
                switch (fieldType)
                {
                    case SubElementFieldData.FieldType:
                        return new SubElementsField();
                    case DropDownFieldData.FieldType:
                        return new DropdownField();
                    case DateTimeFieldData.FieldType:
                        return new DateTimeField();
                    case ReferenceFieldData.FieldType:
                        return new ReferenceField();
                    case CheckboxFieldData.FieldType:
                        return new CheckboxField();
                    case MetaClassElementFieldData.FieldType:
                        return new MetaClassElementField();
                    case TextFieldData.FieldType:
                        return new TextboxField();
                    default:
                        if (isEnumeration)
                        {
                            return new ReadOnlyListField();
                        }
                        else
                        {
                            return new TextboxField();
                        }
                }
            }
            else
            {
                // Get by field type
                var metaClass = field?.getMetaClass();
                if (metaClass == null)
                {
                    throw new ArgumentException("value does not have metaclass and no field type", nameof(value));
                }

                var id = (metaClass as IHasId)?.Id;
                switch (id)
                {
                    case "DatenMeister.Models.Forms.SeparatorLineFieldData":
                        return new SeparatorLineField();
                    case "DatenMeister.Models.Forms.SubElementFieldData":
                        return new SubElementsField();
                    case "DatenMeister.Models.Forms.DropDownFieldData":
                        return  new DropdownField();
                    case "DatenMeister.Models.Forms.DateTimeFieldData":
                        return new DateTimeField();
                    case "DatenMeister.Models.Forms.ReferenceFieldData":
                        return new ReferenceField();
                    case "DatenMeister.Models.Forms.CheckboxFieldData":
                        return new CheckboxField();
                    case "DatenMeister.Models.Forms.TextFieldData":
                        return new TextboxField();
                    case "DatenMeister.Models.Forms.MetaClassElementFieldData":
                        return new MetaClassElementField();
                    default:
                        if (isEnumeration)
                        {
                            return new ReadOnlyListField();
                        }
                        else
                        {
                            return new TextboxField();
                        }
                }
            }
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
        public static (IDetailField detailField, UIElement element)
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
    }
}