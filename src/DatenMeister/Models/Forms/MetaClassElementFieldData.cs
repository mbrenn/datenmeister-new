namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Defines the field data for the meta class
    /// </summary>
    public class MetaClassElementFieldData : FieldData
    {
        public MetaClassElementFieldData() : base("metaclass")
        {
        }

        public MetaClassElementFieldData(string name, string title) : base("metaclass", name, title)
        {
        }
    }
}