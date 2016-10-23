namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Defines a field to which subelements are allowed and to which new elements can be added
    /// </summary>
    public class SubElementFieldData : FieldData
    {
        public SubElementFieldData() : base("subelements")
        {
        }

        public SubElementFieldData(string name, string title) : base("subelements", name, title)
        {
        }
    }
}