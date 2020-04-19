using System;
using System.Collections.Generic;
using DatenMeister.Models.Forms.ViewModes;

namespace DatenMeister.Models.Forms
{
    public static class FieldTypes
    {
        public static IEnumerable<Type> GetAll()
        {
            return new[]
            {
                typeof(FieldData),
                typeof(AnyDataFieldData),
                typeof(CheckboxFieldData),
                typeof(DateTimeFieldData),
                typeof(FormAssociation),
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
                typeof(FormType),
                typeof(Form),
                typeof(DetailForm),
                typeof(ListForm),
                typeof(ExtentForm), 
                
                // Now the view modes
                typeof(ViewMode)
            };
        }
    }
}