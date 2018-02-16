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
                typeof(Form),
                typeof(DetailForm),
                typeof(ListForm),
                typeof(FieldData), 
                typeof(TextFieldData),
                typeof(DateTimeFieldData),
                typeof(DropDownFieldData),
                typeof(DropDownFieldData.ValuePair),
                typeof(SubElementFieldData),
                typeof(DefaultViewForMetaclass),
                typeof(DefaultViewForExtentType),
                typeof(MetaClassElementFieldData),
                typeof(ReferenceFieldData)
            };
        }
    }
}