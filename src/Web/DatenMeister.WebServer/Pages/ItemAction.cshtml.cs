using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class CreateItemModel : PageModel
    {
        public string? MetaClass { get; set; } = string.Empty;

        [Parameter] public string ActionName { get; set; } = string.Empty;

        public void OnGet(string actionName, string? metaclass = null)
        {
            ActionName = actionName;
            MetaClass = metaclass;
        }
    }
}