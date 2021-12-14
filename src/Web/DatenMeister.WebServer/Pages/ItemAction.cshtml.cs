using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class CreateItemModel : PageModel
    {
        public string? MetaClass { get; set; } = string.Empty;

        
        [Parameter] public string ActionName { get; set; } = string.Empty;

        [Parameter] public string? FormUri { get; set; } = string.Empty;

        /// <summary>
        /// The javascript equivalent for the FormUri
        /// </summary>
        public string JsFormUri =>
            string.IsNullOrEmpty(FormUri)
                ? "undefined"
                : $"\"{HttpUtility.JavaScriptStringEncode(FormUri)}\"";

        public CreateItemModel()
        {
        }

        public void OnGet(string actionName, string? formUri, string? metaclass = null)
        {
            FormUri = formUri ?? string.Empty;
            ActionName = actionName;
            MetaClass = metaclass;
        }
    }
}