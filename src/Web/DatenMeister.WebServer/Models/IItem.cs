using DatenMeister.Web.Json;

namespace DatenMeister.WebServer.Models
{
    [Obsolete]
    public class IItem
    {
        public string item { get; set; } = string.Empty;
        
        public ItemWithNameAndId? metaClass { get; set; }

    }
}