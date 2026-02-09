using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Plugins;
using DatenMeister.Types.Actions;

namespace DatenMeister.Types.Plugin;

// ReSharper disable once UnusedMember.Global
[PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
public class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{

    public const string PackageName = "Uml";

    /// <summary>
    /// Stores the name of the extent type
    /// </summary>
    public const string ExtentType = "Uml.Classes";

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterBootstrapping:
                var extentSettings = scopeStorage.Get<ExtentSettings>();
                extentSettings.extentTypeSettings.Add(
                    new ExtentType(ExtentType)
                    {
                        rootElementMetaClasses =
                        {
                            _UML.TheOne.Packages.__Package,
                            _UML.TheOne.StructuredClassifiers.__Class,
                            _UML.TheOne.SimpleClassifiers.__Enumeration
                        }
                    });
                break;
            case PluginLoadingPosition.AfterLoadingOfExtents:
                _ = new MigrateAlternativeTypeReferencesActionLogic(workspaceLogic).MigrateAsync();
                break;
        }

        return Task.CompletedTask;
    }
}