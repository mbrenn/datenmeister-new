namespace DatenMeister.Web.Models.Fields
{
    public class TextDataField : DataField
    {
        public TextDataField()
        {
                
        }

        public TextDataField(string name, string title) : base(name, title)
        {
            
        }

        public int LineHeights { get; set; }
    }
}