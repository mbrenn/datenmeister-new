using System;
using System.Collections.Generic;

namespace DatenMeister.Modules.Forms.Model
{
    public static class FieldTypes
    {
        public static IEnumerable<Type> GetAll()
        {
            return new[]
            {
                typeof(FieldData),
                typeof(CheckboxFieldData),
                typeof(DateTimeFieldData),
                typeof(ViewAssociation),
                typeof(DropDownFieldData),
                typeof(DropDownFieldData.ValuePair),
                typeof(MetaClassElementFieldData),
                typeof(ReferenceFieldData),
                typeof(SubElementFieldData),
                typeof(TextFieldData),
                typeof(SeparatorLineFieldData),
                typeof(FileSelectionFieldData),
                typeof(DefaultTypeForNewElement),

                // Now the forms
                typeof(ViewType),
                typeof(Form),
                typeof(DetailForm),
                typeof(ListForm),
                typeof(ExtentForm)
            };
        }
    }
}