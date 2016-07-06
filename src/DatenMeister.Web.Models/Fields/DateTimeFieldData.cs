namespace DatenMeister.Web.Models.Fields
{
    public class DateTimeFieldData : FieldData
    {
        public DateTimeFieldData() : base ("datetime")
        {
        }

        public DateTimeFieldData(string name, string title) : base("datetime", name, title)
        {

        }

        public bool showDate { get; set; } = true;
        public bool showTime { get; set; } = true;

        /// <summary>
        /// Stores the value indicating whether the GUI shall 
        /// show buttons to forward by one day, one week, etc... 
        /// </summary>
        public bool showOffsetButtons { get; set; } = true;
    }
}