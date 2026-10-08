using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Plugins;
using DatenMeister.Plugins.Helper;
using DatenMeister.Web.StaticPages.HtmlElements;

namespace DatenMeister.Web.StaticPages;

[PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
// ReSharper disable once UnusedType.Global
public class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    private const string DmTypesUriReference = "dm:///intern.types.staticwebpages.datenmeister/";
    
    public Task Start(PluginLoadingPosition position)
    {
        // Defines the pluginhelper
        var pluginHelper = new PluginHelper(
            new PluginHelperConfiguration
            {
                PluginType = typeof(Plugin),
                Position = position,
                ScopeStorage = scopeStorage,
                WorkspaceLogic = workspaceLogic
            });
        
        var staticPagesData = scopeStorage.Get<StaticPagesData>();
        staticPagesData.HtmlElements.Add(new HtmlHeadline());
        staticPagesData.HtmlElements.Add(new HtmlParagraph());
        staticPagesData.HtmlElements.Add(new HtmlDataTable(workspaceLogic, scopeStorage));
        
        // Adds the extent
        pluginHelper.AddExtentForTypesFromManifest("Xmi.Types.xmi", DmTypesUriReference);
        
        // Adds the action handler
        pluginHelper.AddActionHandler(new CreateStaticWebPageActionHandler(scopeStorage));
        return Task.CompletedTask;
    }
}