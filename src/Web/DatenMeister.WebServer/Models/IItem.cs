namespace DatenMeister.WebServer.Models
{
    public record IItem
    {
        public string item { get; set; }
        public ItemWithNameAndId? metaClass { get; set; }
    }
}