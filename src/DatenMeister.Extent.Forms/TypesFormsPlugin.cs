using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms;

// ReSharper disable once UnusedType.Global
/// <summary>
/// Defines the default form extensions which are used to navigate through the
/// items, extents and also offers the simple creation and deletion of items. 
/// </summary>
public class TypesFormsPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    private readonly ExtentSettings _extentSettings = scopeStorage.Get<ExtentSettings>();

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:

                var formsPlugin = scopeStorage.Get<FormsState>();
                formsPlugin.FormModificationPlugins.Add(
                    new FormModificationPlugin
                    {
                        CreateContext = context => context.Global.CollectionFormFactories.Add(
                            new ExtentTypeFormModification.IncludeJumpToExtentButtonModification()),
                        Name = "IncludeJumpToExtentButtonModification"
                    });

                formsPlugin.FormModificationPlugins.Add(
                    new FormModificationPlugin
                    {
                        CreateContext = context => context.Global.RowFormFactories.Add(
                            new ExtentTypeFormModification.IncludeExtentTypesForTableFormExtent(_extentSettings)),
                        Name = "IncludeExtentTypesForTableFormExtent"
                    });

                formsPlugin.FormModificationPlugins.Add(
                    new FormModificationPlugin
                    {
                        CreateContext = context => context.Global.CollectionFormFactories.Add(
                            new ExtentTypeFormModification.IncludeCreationButtonsInTableFormForClassifierOfExtentType(
                                workspaceLogic, _extentSettings)),
                        Name = "IncludeCreationButtonsInTableFormForClassifierOfExtentType"
                    });

                formsPlugin.FormModificationPlugins.Add(
                    new FormModificationPlugin
                    {
                        CreateContext = context => context.Global.ObjectFormFactories.Add(
                            new ExtentTypeFormModification.
                                IncludeCreationButtonsInDetailFormOfPackageForClassifierOfExtentType(
                                    workspaceLogic, _extentSettings)),
                        Name = "IncludeCreationButtonsInTableFormForClassifierOfExtentType"
                    });

                break;
        }

        return Task.CompletedTask;
    }
}