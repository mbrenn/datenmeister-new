﻿using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public class FieldFactory
    {
        /// <summary>
        /// Creates a specific field by the reading out the field type
        /// </summary>
        /// <param name="value">Value of the field being used</param>
        /// <param name="field">Field to be queried</param>
        /// <returns>Found field or exception if not found</returns>
        public static IDetailField CreateField(IElement value, IElement field)
        {
            var fieldType = field.get(_FormAndFields._FieldData.fieldType)?.ToString();
            var isEnumeration = DotNetHelper.AsBoolean(field.get(_FormAndFields._FieldData.isEnumeration));

            if (isEnumeration)
            {
                return new ReadOnlyListField();
            }
            else
            {
                return new TextboxField();
            }
        }

        /// <summary>
        /// Gets the UI Element for the given field and value
        /// </summary>
        /// <param name="value">Value of the field being used</param>
        /// <param name="field">Field to be queried</param>
        /// <param name="formControl">Form Control in which the UI Element will be hosted</param>
        /// <returns>The created element</returns>
        public static UIElement GetUIElementFor(IElement value, IElement field, DetailFormControl formControl)
        {
            return CreateField(value, field).CreateElement(value, field, formControl);
        }
    }
}