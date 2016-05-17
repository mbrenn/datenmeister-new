namespace DatenMeister.Web.Models.Fields
{
    public class DateTimeDataField : DataField
    {
        public DateTimeDataField()
        {

        }

        public DateTimeDataField(string name, string title) : base(name, title)
        {

        }

        public bool showDate { get; set; } = true;
        public bool showTime { get; set; } = true;
    }
}