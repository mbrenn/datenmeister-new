using System;

namespace DatenMeister.Models.Forms
{
    public static class FieldTypes
    {
        public static Type[] GetAll()
        {
            return new[]
            {
                typeof(Form),
                typeof(FieldData), 
                typeof(TextFieldData),
                typeof(DateTimeFieldData),
                typeof(DropDownFieldData),
                typeof(SubElementFieldData),
                typeof(DefaultViewForMetaclass),
                typeof(DefaultViewForExtentType)
            };
        }
    }
}