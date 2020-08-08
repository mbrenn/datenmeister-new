namespace DatenMeister.Models.Forms
{
    public class DateTimeFieldData : FieldData
    {
        public DateTimeFieldData()
        {
        }

        public DateTimeFieldData(string name, string title) : base(name, title)
        {

        }

        /// <summary>
        /// Gets or sets the value whether to show the date field.
        /// </summary>
        public bool hideDate { get; set; } = false;

        /// <summary>
        /// Gets or sets the value whether to show the time field
        /// </summary>
        public bool hideTime { get; set; } = false;

        /// <summary>
        /// Stores the value indicating whether the GUI shall
        /// show buttons to forward by one day, one week, etc...
        /// </summary>
        public bool showOffsetButtons { get; set; } = true;
    }
}