namespace DatenMeister.Web.Models.Fields
{
    public class TextFieldData : FieldData
    {
        public TextFieldData()
        {
                
        }

        public TextFieldData(string name, string title) : base(name, title)
        {
            
        }

        public int lineHeight { get; set; }
    }
}