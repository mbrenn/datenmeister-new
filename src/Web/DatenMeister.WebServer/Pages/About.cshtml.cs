using Autofac;
using DatenMeister.Actions;
using DatenMeister.Core;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.TypeIndexAssembly;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Integration.DotNet;
using DatenMeister.Plugins;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages;

public class AboutModel : PageModel
{
    public void OnGet()
    {
    }

    public string WorkspacePath
    {
        get
        {
            var integrationSettings = GiveMe.Scope.Resolve<IntegrationSettings>();
            return integrationSettings.DatabasePath;
        }
    }

    public static List<string> KnownExtentTypes
    {
        get
        {
            var extentSettings = GiveMe.Scope.ScopeStorage.Get<ExtentSettings>();
            return extentSettings.extentTypeSettings.Select(x => x.name).ToList();
        }
    }

    public static List<string> ActionHandlers
    {
        get
        {
            var actionLogicState = GiveMe.Scope.ScopeStorage.Get<ActionLogicState>();
            return actionLogicState.ActionHandlers
                .Select(x => x.ToString())
                .Where(x=> !string.IsNullOrEmpty(x))
                .ToList()!;
        }
    }

    public static List<string> LoadedPlugins
    {
        get
        {
            var pluginLoader = GiveMe.Scope.ScopeStorage.Get<PluginManager>();
            return  pluginLoader.PluginTypes!.Select(x => x.FullName!).OrderBy(x => x).ToList();
        }
    }

    /// <summary>
    /// Returns a list of strings containing the names of the types of the Form Modification Plugins
    /// </summary>
    public static List<string> FormModificationTypes
    {
        get
        {
            var formPlugin = GiveMe.Scope.ScopeStorage.Get<FormsState>();
            return formPlugin.FormModificationPlugins
                .Select(x => $"{x} ({x.Name})")
                .ToList();
        }
    }

    public static List<string> WorkspaceDependencies
    {
        get
        {
            var typeIndexLogic = new TypeIndexLogic(GiveMe.Scope.WorkspaceLogic);
            var indexStore = typeIndexLogic.TypeIndexStore.Current;
            if (indexStore == null)
            {
                return ["Index Store in build up"];
            }

            return indexStore.Workspaces
                .Select(
                    x => $"{x.WorkspaceId}: {x.MetaclassWorkspaces.Aggregate(
                        (y, z) => $"{y}, {z}")}")
                .ToList();
        }
    }

    public static Dictionary<string, Func<string>> InstrumentationPoints { get; } = new()
    {
        {
            "ExtentUrlNavigator: Cache Hits", () =>
                $"{ExtentUrlNavigator.CacheHit} (" +
                $"{Math.Round((double)ExtentUrlNavigator.CacheHit / (ExtentUrlNavigator.CacheHit + ExtentUrlNavigator.CacheMiss) * 100)}%)"
        },
        {
            "ExtentUrlNavigator: Cache Misses", () =>
                $"{ExtentUrlNavigator.CacheMiss} (" +
                $"{Math.Round((double)ExtentUrlNavigator.CacheMiss / (ExtentUrlNavigator.CacheHit + ExtentUrlNavigator.CacheMiss) * 100)}%)"
        }
    };
}