using System;
using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
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
            var fieldType = field.get(_FormAndFields._FieldData.fieldType)?.ToString();
            var isEnumeration = field.getOrDefault<bool>(_FormAndFields._FieldData.isEnumeration);
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
            GetUIElementFor(IObject value, IElement field, DetailFormControl formControl, ref FieldFlags flags)
        {
            var fieldElement = CreateField(value, field);
            var element = fieldElement.CreateElement(value, field, formControl, ref flags);

            return (fieldElement, element);
        }
    }

    [Flags]
    public enum FieldFlags
    {
        Zero = 0x00,
        Focussed = 0x01
    }
}