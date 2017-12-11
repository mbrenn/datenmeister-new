namespace DatenMeister.Models.Forms
{
    public class TextFieldData : FieldData
    {
        public const string FieldType = "text";

        public TextFieldData() : base(FieldType)
        {

        }

        public TextFieldData(string name, string title) : base(FieldType, name, title)
        {
            
        }

        public int lineHeight { get; set; }
    }
}