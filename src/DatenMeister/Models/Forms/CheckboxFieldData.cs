namespace DatenMeister.Models.Forms
{
    public class CheckboxFieldData : FieldData
    {
        public CheckboxFieldData()
        {

        }

        public CheckboxFieldData(string name, string title) : base(name, title)
        {
            
        }

        public int lineHeight { get; set; }
    }
}