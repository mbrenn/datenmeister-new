using System;

namespace DatenMeister.Web.Models.Fields
{
    public static class FieldTypes
    {
        public static Type[] GetAll()
        {
            return new[]
            {
                typeof(Form), 
                typeof(TextFieldData),
                typeof(DateTimeFieldData),
                typeof(DropDownFieldData)
            };
        }
    }
}