using Autofac;
using DatenMeister.BootStrap;
using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages;

public class ResetDatenMeister : PageModel
{
    /// <summary>
    ///     Gets or sets the information whether the restart has been performed
    /// </summary>
    public bool IsRestarted { get; set; }

    public void OnGet()
    {
    }

    public async Task OnPost(string action)
    {
        if (action != "reset") return;

        // Ok... what to do now?
        // Collect all files...
        var files = new List<string>();
        var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
        var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
        var integrationSettings = GiveMe.Scope.ScopeStorage.Get<IntegrationSettings>();
        foreach (var workspace in workspaceLogic.Workspaces)
        {
            if (workspace.id == WorkspaceNames.WorkspaceData) continue;

            foreach (var extent in workspaceLogic.GetExtentsForWorkspace(workspace))
            {
                var loadConfiguration = extentManager.GetLoadConfigurationFor(extent);
                if (loadConfiguration != null)
                {
                    var extentStoragePath =
                        loadConfiguration.getOrDefault<string>(_ExtentLoaderConfigs
                            ._ExtentFileLoaderConfig.filePath);

                    if (extentStoragePath != null) files.Add(extentStoragePath);
                }
            }
        }

        await GiveMe.Scope.UnuseDatenMeister();
        GiveMe.Scope = null!;

        foreach (var file in files) System.IO.File.Delete(file);

        System.IO.File.Delete(Integrator.GetPathToWorkspaces(integrationSettings));
        System.IO.File.Delete(Integrator.GetPathToExtents(integrationSettings));

        // Reloads the DatenMeister
        IsRestarted = true;
        await Program.Stop(true);
    }
}