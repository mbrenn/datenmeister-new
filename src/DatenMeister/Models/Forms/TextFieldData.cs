namespace DatenMeister.Modules.Forms.Model
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

        /// <summary>
        /// Gets or sets the width in guessed number of characters
        /// </summary>
        public int width { get; set; }
    }
}