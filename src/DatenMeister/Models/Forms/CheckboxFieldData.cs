namespace DatenMeister.Modules.Forms.Model
{
    public class CheckboxFieldData : FieldData
    {
        public const string FieldType = "checkbox";

        public CheckboxFieldData() : base(FieldType)
        {

        }

        public CheckboxFieldData(string name, string title) : base(FieldType, name, title)
        {
            
        }

        public int lineHeight { get; set; }
    }
}