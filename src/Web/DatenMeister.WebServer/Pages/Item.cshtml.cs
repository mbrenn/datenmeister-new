﻿using System.Net;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
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
            Workspace = WebUtility.UrlDecode(workspace);
            ItemUrl = WebUtility.UrlDecode(itemUrl);
        }
    }
}