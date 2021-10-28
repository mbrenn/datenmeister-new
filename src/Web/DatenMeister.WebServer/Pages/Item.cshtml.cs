using System;
using System.Net;
using System.Text;
using System.Web;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.HtmlEngine;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Library.HtmlControls;
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
            if (ItemAndFormModel == null)
            {
                throw new InvalidOperationException($"Items not found: {Workspace}/{Extent}/{Item}");
            }

            if (ItemAndFormModel == null)
            {
                throw new InvalidOperationException($"Items not found: {Workspace}/{Extent}/{Item}");
            }
            
            FoundItem = XmiHelper.ConvertItemFromXmi(ItemAndFormModel.item)
                        ?? throw new InvalidOperationException("Items are null. They may not be null");
            Form = XmiHelper.ConvertItemFromXmi(ItemAndFormModel.form)
                   ?? throw new InvalidOperationException("Form is null. It may not be null");
        }
    }
}