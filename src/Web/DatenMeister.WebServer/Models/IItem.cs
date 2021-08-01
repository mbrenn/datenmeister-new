using DatenMeister.Json;

namespace DatenMeister.WebServer.Models
{
    public record IItem
    {
        public string item { get; set; } = string.Empty;
        
        public ItemWithNameAndId? metaClass { get; set; }
    }
}