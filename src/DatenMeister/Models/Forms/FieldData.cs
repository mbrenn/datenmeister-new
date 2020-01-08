namespace DatenMeister.Models.Forms
{
    public class FieldData
    {
        public FieldData()
        {
        }

        public FieldData(string name, string title) : this()
        {
            this.name = name;
            this.title = title;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field is attached to an attache object or
        /// whether the field will be attached to the detail element
        /// </summary>
        public bool isAttached { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public bool isEnumeration { get; set; }

        public object defaultValue { get; set; }

        /// <summary>
        /// Gets or sets the infgormation whether the regarded field shall be
        /// considered as readonyl
        /// </summary>
        public bool isReadOnly { get; set; }
    }
}