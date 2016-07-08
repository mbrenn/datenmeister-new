namespace DatenMeister.Web.Models.Fields
{
    public class TextFieldData : FieldData
    {
        public TextFieldData() : base("text")
        {

        }

        public TextFieldData(string name, string title) : base("text", name, title)
        {
            
        }

        public int lineHeight { get; set; }
    }
}