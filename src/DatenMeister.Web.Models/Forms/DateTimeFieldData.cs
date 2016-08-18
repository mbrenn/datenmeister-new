namespace DatenMeister.Models.Forms
{
    public class DateTimeFieldData : FieldData
    {
        public DateTimeFieldData() : base ("datetime")
        {
        }

        public DateTimeFieldData(string name, string title) : base("datetime", name, title)
        {

        }

        /// <summary>
        /// Gets or sets the value whether to show the date field.
        /// </summary>
        public bool showDate { get; set; } = true;

        /// <summary>
        /// Gets or sets the value whether to show the time field
        /// </summary>
        public bool showTime { get; set; } = true;

        /// <summary>
        /// Stores the value indicating whether the GUI shall 
        /// show buttons to forward by one day, one week, etc... 
        /// </summary>
        public bool showOffsetButtons { get; set; } = true;
    }
}