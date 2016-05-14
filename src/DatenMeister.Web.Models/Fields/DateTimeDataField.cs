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

        public bool ShowDate { get; set; }
        public bool ShowTime { get; set; }
    }
}