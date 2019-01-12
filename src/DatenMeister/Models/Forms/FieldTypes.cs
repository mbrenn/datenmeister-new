using System;
using System.Collections.Generic;

namespace DatenMeister.Models.Forms
{
    public static class FieldTypes
    {
        public static IEnumerable<Type> GetAll()
        {
            return new[]
            {
                typeof(CheckboxFieldData),
                typeof(DateTimeFieldData),
                typeof(ViewAssociation),
                typeof(DetailForm),
                typeof(DropDownFieldData),
                typeof(DropDownFieldData.ValuePair),
                typeof(FieldData),
                typeof(Form),
                typeof(ListForm),
                typeof(MetaClassElementFieldData),
                typeof(ReferenceFieldData),
                typeof(SubElementFieldData),
                typeof(TextFieldData),
                typeof(SeparatorLineFieldData),
                typeof(ViewType)
            };
        }
    }
}