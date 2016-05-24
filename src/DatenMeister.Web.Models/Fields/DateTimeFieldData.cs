namespace DatenMeister.Web.Models.Fields
{
    public class DateTimeFieldData : FieldData
    {
        public DateTimeFieldData()
        {

        }

        public DateTimeFieldData(string name, string title) : base(name, title)
        {

        }

        public bool showDate { get; set; } = true;
        public bool showTime { get; set; } = true;
    }
}