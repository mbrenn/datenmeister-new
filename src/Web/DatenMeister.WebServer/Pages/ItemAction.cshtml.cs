using System;
using System.Web;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.ExtentManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class CreateItemModel : PageModel
    {
        public CreateItemModel()
        {
        }

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

        public ActionResult OnGet(string actionName, string? formUri, string? metaclass = null)
        {
            FormUri = formUri ?? string.Empty;
            ActionName = actionName;
            MetaClass = metaclass;

            if (ActionName == "Extent.Properties.Update")
            {
                var workspaceId = (string) Request.Query["workspace"];
                var extentName = (string) Request.Query["extent"];
                var workspace = GiveMe.Scope.WorkspaceLogic.GetWorkspace(workspaceId)
                                ?? throw new InvalidOperationException($"Workspace '{workspaceId}' not found");

                var extent = GiveMe.Scope.WorkspaceLogic.FindExtent(workspaceId, extentName)
                             ?? throw new InvalidOperationException(
                                 $"Extent '{extentName}' in '{workspaceId}' not found");
                var uri = ExtentManagementUrlHelper.GetUrlOfExtentsProperties(workspace, extent);

                return Redirect(
                    "~/Item/Management/" + HttpUtility.UrlEncode(ManagementProviderPlugin.UriExtentWorkspaces) + "/" +
                    HttpUtility.UrlEncode(uri));
            }

            return Page();
        }
    }
}