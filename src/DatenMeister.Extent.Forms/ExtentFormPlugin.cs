using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Plugins;
using DatenMeister.Types;
using System.Reflection;

namespace DatenMeister.Extent.Forms;

// ReSharper disable once UnusedType.Global
/// <summary>
/// Defines the default form extensions which are used to navigate through the
/// items, extens and also offers the simple creation and deletion of items. 
/// </summary>
[PluginLoading]
public class ExtentFormPlugin(IScopeStorage scopeStorage, ExtentManager extentManager, IWorkspaceLogic workspaceLogic)
    : IDatenMeisterPlugin
{
    public const string NavigationExtentNavigateTo = "Extent.NavigateTo";
    public const string NavigationExtentDeleteExtent = "Extent.Delete";
    public const string NavigationExtentClear = "Extent.Clear";
    public const string NavigationExtentProperties = "Extent.Properties";
    public const string NavigationStore = "Extent.Store";
    public const string NavigationExportXmi = "Extent.ExportXmi.Navigate";
    public const string NavigationImportXmi = "Extent.ImportXmi.Navigate";
    public const string NavigationItemNew = "Item.New";

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:

                LoadXmi();

                AddActionButtons();
                break;
        }

        return Task.CompletedTask;
    }

    private void LoadXmi()
    {
        var localTypeSupport = new LocalTypeSupport(workspaceLogic, scopeStorage);
        var formMethods = new FormMethods(workspaceLogic, scopeStorage);
        var targetExtent = formMethods.GetInternalFormExtent();
        var localTypeExtent = localTypeSupport.GetInternalTypeExtent();
        PackageMethods.ImportByStream(
            GetXmiStreamForForms(), null, targetExtent, "DatenMeister.Extent.Forms");
        PackageMethods.ImportByStream(
            GetXmiStreamForTypes(), null, localTypeExtent, "DatenMeister.Extent.Forms");
    }


    public static Stream GetXmiStreamForForms()
    {
        var stream = typeof(ExtentFormPlugin).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Extent.Forms.Xmi.DatenMeister.Extent.Forms.Forms.xmi");
        return stream ?? throw new InvalidOperationException("Stream is not found");
    }

    public static Stream GetXmiStreamForTypes()
    {
        var stream = typeof(ExtentFormPlugin).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Extent.Forms.Xmi.DatenMeister.Extent.Forms.Types.xmi");
        return stream ?? throw new InvalidOperationException("Stream is not found");
    }

    private void AddActionButtons()
    {
        var formsPlugin = scopeStorage.Get<FormsPluginState>();

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin, new ActionButtonAdderParameter(NavigationExtentNavigateTo, "View Items in Extent")
            {
                FormType = _Forms.___FormType.Row,
                MetaClass = _Management.TheOne.__Extent
            });

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin, new ActionButtonAdderParameter(NavigationStore, "Store Extent")
            {
                FormType = _Forms.___FormType.Row,
                MetaClass = _Management.TheOne.__Extent,
                PredicateForElement =
                    element =>
                        extentManager.GetProviderLoaderAndConfiguration(
                                element.getOrDefault<string>(_Management._Extent.workspaceId),
                                element.getOrDefault<string>(_Management._Extent.uri))
                            .providerLoader?.ProviderLoaderCapabilities.AreChangesPersistant == true
            });

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin,
            new ActionButtonAdderParameter(NavigationExtentProperties, "View Extent Properties")
            {
                FormType = _Forms.___FormType.Row,
                MetaClass = _Management.TheOne.__Extent
            });

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin, new ActionButtonAdderParameter(NavigationExtentDeleteExtent, "Delete Extent")
            {
                FormType = _Forms.___FormType.Row,
                MetaClass = _Management.TheOne.__Extent
            });

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin, new ActionButtonAdderParameter(NavigationExtentClear, "Clear Extent")
            {
                FormType = _Forms.___FormType.Row,
                MetaClass = _Management.TheOne.__Extent
            });

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin, new ActionButtonAdderParameter(NavigationItemNew, "New Item")
            {
                MetaClass = _Management.TheOne.__Extent,
                FormType = _Forms.___FormType.Collection
            }
        );

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin,
            new ActionButtonAdderParameter(NavigationExtentNavigateTo, "Items")
            {
                ParentMetaClass = _Management.TheOne.__Workspace,
                FormType = _Forms.___FormType.Table,
                ParentPropertyName = _Management._Workspace.extents,
                ActionButtonPosition = 0
            });

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin,
            new ActionButtonAdderParameter(NavigationExportXmi, "Export Xmi")
            {
                FormType = _Forms.___FormType.Row,
                MetaClass = _Management.TheOne.__Extent
            });

        ActionButtonToFormAdder.AddActionButton(
            formsPlugin,
            new ActionButtonAdderParameter(NavigationImportXmi, "Import Xmi")
            {
                FormType = _Forms.___FormType.Row,
                MetaClass = _Management.TheOne.__Extent
            });
    }
}