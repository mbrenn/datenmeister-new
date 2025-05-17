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

        [Parameter] public string? MetaClass { get; set; } = string.Empty;

        [Parameter] public string ActionName { get; set; } = string.Empty;

        [Parameter] public string? FormUri { get; set; } = string.Empty;


        public ActionResult OnGet(string actionName, string? formUri, string? metaclass = null)
        {
            FormUri = formUri ?? string.Empty;
            ActionName = actionName;
            MetaClass = metaclass;

            if (ActionName == "Extent.Properties.Navigate")
            {
                var workspaceId = (string) Request.Query["workspace"]!;
                var extentName = (string) Request.Query["extent"]!;
                var workspace = GiveMe.Scope.WorkspaceLogic.GetWorkspace(workspaceId)
                                ?? throw new InvalidOperationException($"Workspace '{workspaceId}' not found");

                var extent = GiveMe.Scope.WorkspaceLogic.FindExtent(workspaceId, extentName)
                             ?? throw new InvalidOperationException(
                                 $"Extent '{extentName}' in '{workspaceId}' not found");
                var itemUri =
                    HttpUtility.UrlEncode(ManagementProviderPlugin.UriExtentWorkspaces + "#" +
                                          ExtentManagementHelper.GetIdOfExtentsProperties(workspace, extent));

                return Redirect(
                    $"~/Item/Management/{itemUri}");
            }

            return Page();
        }
    }
}