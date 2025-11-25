using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Plugins;
using DatenMeister.Types;
using System.Reflection;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;

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
        var formMethods = new FormMethods(workspaceLogic);
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
        // TODO: Reactivate Constraints
        var formsPlugin = scopeStorage.Get<FormsState>();

        ActionButtonToFormAdder.AddRowActionButton(
            formsPlugin, new ActionButtonAdderParameterForRow(NavigationExtentNavigateTo, "View Items in Extent")
            {
                PredicateForParameter =
                    x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true
            });

        ActionButtonToFormAdder.AddTableActionButton(
            formsPlugin, new ActionButtonAdderParameterForTable(NavigationExtentNavigateTo, "View Items")
            {
                PredicateForParameter =
                    x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true,
                ActionButtonPosition = 0
            });

        ActionButtonToFormAdder.AddRowActionButton(
            formsPlugin, new ActionButtonAdderParameterForRow(NavigationStore, "Store Extent")
            {
                PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true,
                PredicateForElement =
                    element =>
                    {
                        if (element == null)
                            throw new InvalidOperationException("Element is null");

                        return extentManager.GetProviderLoaderAndConfiguration(
                                element.getOrDefault<string>(_Management._Extent.workspaceId),
                                element.getOrDefault<string>(_Management._Extent.uri))
                            .providerLoader?.ProviderLoaderCapabilities.AreChangesPersistant == true;
                    }
            });

        ActionButtonToFormAdder.AddRowActionButton(
            formsPlugin,
            new ActionButtonAdderParameterForRow(NavigationExtentProperties, "View Extent Properties")
            {
                PredicateForParameter =
                    x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true,
            });

        ActionButtonToFormAdder.AddRowActionButton(
            formsPlugin, new ActionButtonAdderParameterForRow(NavigationExtentDeleteExtent, "Delete Extent")
            {
                PredicateForParameter =
                    x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true,
            });

        ActionButtonToFormAdder.AddRowActionButton(
            formsPlugin, new ActionButtonAdderParameterForRow(NavigationExtentClear, "Clear Extent")
            {
                PredicateForParameter =
                    x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true,
            });

        ActionButtonToFormAdder.AddRowActionButton(
            formsPlugin, new ActionButtonAdderParameterForRow(NavigationItemNew, "New Item")
            {
                PredicateForParameter =
                    x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true,
            }
        );

        ActionButtonToFormAdder.AddTableActionButton(
            formsPlugin,
            new ActionButtonAdderParameterForTable(NavigationExtentNavigateTo, "Items")
            {
                PredicateForParameter = x => 
                    x.PropertyName == _Management._Workspace.extents
                    && x.ParentMetaClass?.equals(_Management.TheOne.__Workspace) == true,
                ActionButtonPosition = 0
            });

        ActionButtonToFormAdder.AddRowActionButton(
            formsPlugin,
            new ActionButtonAdderParameterForRow(NavigationExportXmi, "Export Xmi")
            {
                PredicateForParameter =
                    x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true,
            });

        ActionButtonToFormAdder.AddRowActionButton(
            formsPlugin,
            new ActionButtonAdderParameterForRow(NavigationImportXmi, "Import Xmi")
            {
                PredicateForParameter =
                    x => x.MetaClass?.equals(_Management.TheOne.__Extent) == true,
            });
    }
}