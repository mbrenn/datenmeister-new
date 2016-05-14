namespace DatenMeister.Web.Models.Fields
{
    public class DataField
    {
        public DataField()
        {
            this.fieldType = "text";
        }

        public DataField(string name, string title) : this()
        {
            this.name = name;
            this.title = title;
        }
        
        public string fieldType { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public bool isEnumeration { get; set; }
    }
}