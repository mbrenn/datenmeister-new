namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Defines the field data for the meta class
    /// </summary>
    public class MetaClassElementFieldData : FieldData
    {
        /// <summary>
        /// Defines the field type for the metaclass
        /// </summary>
        public const string FieldType = "metaclass";

        public MetaClassElementFieldData() : base(FieldType)
        {
        }

        public MetaClassElementFieldData(string name, string title) : base(FieldType, name, title)
        {
        }
    }
}