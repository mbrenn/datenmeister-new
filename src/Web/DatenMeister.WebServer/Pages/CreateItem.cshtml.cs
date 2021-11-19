using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class CreateItemModel : PageModel
    {
        public string MetaClass { get; } = string.Empty;
        
        [Parameter] public string Action { get; set; } = string.Empty;

        public CreateItemModel()
        {
            
        }
        
        public CreateItemModel(string metaClass)
        {
            MetaClass = metaClass;
        }

        public void OnGet()
        {
        }
    }
}
