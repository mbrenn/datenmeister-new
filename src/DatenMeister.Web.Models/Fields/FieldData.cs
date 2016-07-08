namespace DatenMeister.Web.Models.Fields
{
    public class FieldData
    {
        public FieldData(string fieldType)
        {
            this.fieldType = fieldType;
        }

        public FieldData(string fieldType, string name, string title) : this(fieldType)
        {
            this.name = name;
            this.title = title;
        }
        
        public string fieldType { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public bool isEnumeration { get; set; }

        public object defaultValue { get; set; }
    }
}