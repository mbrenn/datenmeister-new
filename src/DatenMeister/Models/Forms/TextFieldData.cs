namespace DatenMeister.Models.Forms
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

        /// <summary>
        /// Gets or sets the width in guessed number of characters
        /// </summary>
        public int width { get; set; }
    }
}