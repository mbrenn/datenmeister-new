using System.Text;
using System.Web;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WebServer.Library.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class ItemModel : PageModel
    {
        public readonly StringBuilder ScriptLines = new();

        [Parameter] public string Workspace { get; set; } = string.Empty;

        [Parameter] public string ItemUrl { get; set; } = string.Empty;

        public IObject? FoundItem { get; set; }

        public IObject? Form { get; set; }

        public string ExtentUrl
        {
            get
            {
                var posHash = ItemUrl.IndexOf('#');
                if (posHash == -1)
                    return ItemUrl;
                return ItemUrl.Substring(0, posHash);
            }
        }

        public void OnGet(string workspace, string itemUrl)
        {
            Workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            ItemUrl = MvcUrlEncoder.DecodePathOrEmpty(itemUrl);
        }
    }
}