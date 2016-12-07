namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Defines a field to which subelements are allowed and to which new elements can be added
    /// </summary>
    public class SubElementFieldData : FieldData
    {
        /// <summary>
        /// Initializes a new element of the subelement field data and sets the field as an enumeration
        /// </summary>
        public SubElementFieldData() : base("subelements")
        {
            isEnumeration = true;
        }

        /// <summary>
        /// Initializes a new element of the subelement field data and sets the field as an enumeration
        /// </summary>
        /// <param name="name">Name of the field</param>
        /// <param name="title">Title of the field</param>
        public SubElementFieldData(string name, string title) : base("subelements", name, title)
        {
            isEnumeration = true;
        }
    }
}