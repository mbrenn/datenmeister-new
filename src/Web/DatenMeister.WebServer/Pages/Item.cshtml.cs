using System;
using System.Net;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Models;
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
        
        private ExtentItemsController ExtentItemsController { get; set; }

        public IObject? FoundItem { get; set; }

        public IObject? Form { get; set; }

        public readonly StringBuilder ScriptLines = new();
        public ItemAndFormModel? ItemAndFormModel;

        public ItemModel(ExtentItemsController extentItemsController)
        {
            ExtentItemsController = extentItemsController;
        }

        public void OnGet(string workspace, string extent, string? item)
        {            
            Workspace = WebUtility.UrlDecode(workspace);
            Extent = WebUtility.UrlDecode(extent);
            Item = WebUtility.UrlDecode(item);

            ItemAndFormModel = ExtentItemsController.GetItemAndForm(Workspace, Extent, Item);
        }
    }
}