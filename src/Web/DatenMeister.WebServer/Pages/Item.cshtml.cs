using System.Net;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class ItemModel : PageModel
    {
        [Parameter] public string Workspace { get; set; } = string.Empty;

        [Parameter] public string Extent { get; set; } = string.Empty;

        [Parameter] public string? Item { get; set; } = string.Empty;

        /// <summary>
        /// Gets the item url of the currently selected item
        /// </summary>
        public string ItemUrl => Item?.Contains('#') == true ? Item : $"{Extent}#{Item}";

        public IObject? FoundItem { get; set; }

        public IObject? Form { get; set; }

        public readonly StringBuilder ScriptLines = new();

        public ItemModel()
        {
        }

        public void OnGet(string workspace, string extent, string? item)
        {            
            Workspace = WebUtility.UrlDecode(workspace);
            Extent = WebUtility.UrlDecode(extent);
            Item = WebUtility.UrlDecode(item);
        }
    }
}