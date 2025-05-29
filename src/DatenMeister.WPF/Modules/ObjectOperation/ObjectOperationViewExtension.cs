using System.Windows;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Provider.Xmi;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.TreeView;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Modules.ObjectOperation;

public class ObjectOperationViewExtension : IViewExtensionFactory
{
    /// <summary>
    /// Defines the logger for 
    /// </summary>
    private static ILogger Logger = new ClassLogger(typeof(ObjectOperationViewExtension));

    public IEnumerable<ViewExtension> GetViewExtensions(
        ViewExtensionInfo viewExtensionInfo)
    {
        if (viewExtensionInfo.GetItemExplorerControl() != null)
        {
            yield return new TreeViewItemCommandDefinition(
                "New Object...",
                async x => { await NewItem(viewExtensionInfo.NavigationHost, x.Element); }
            ) {CategoryName = "Item"};
                
            yield return new TreeViewItemCommandDefinition(
                "Move Object...",
                async x => { await MoveItem(viewExtensionInfo.NavigationHost, x.Element); }
            ) {CategoryName = "Item"};

            yield return new TreeViewItemCommandDefinition(
                "Copy Object...",
                async x => { await CopyItem(viewExtensionInfo.NavigationHost, x.Element); }
            ) {CategoryName = "Item"};

            yield return new TreeViewItemCommandDefinition(
                "Edit Object...", x => { EditItem(viewExtensionInfo.NavigationHost, x.Element); }
            ) {CategoryName = "Item"};

            yield return new TreeViewItemCommandDefinition(
                "Delete Object...", DeleteItem
            ) {CategoryName = "Item"};

            yield return new TreeViewItemCommandDefinition(
                "Export as Xmi...", x => { CopyAsXmi(viewExtensionInfo.NavigationHost, x.Element); }
            ) {CategoryName = "Item"};

            yield return new TreeViewItemCommandDefinition(
                    "Import By Xmi...", 
                    async x =>
                    {
                        await ImportByXmi(viewExtensionInfo.NavigationHost, x.Element);
                    }
                )
                { CategoryName = "Item" };
        }
    }
        

    private async Task NewItem(INavigationHost navigationHost, IObject? o)
    {
        if (o == null)
        {
            return;
        }

        await NavigatorForItems.NavigateToNewItemForItem(
            navigationHost,
            o,
            DefaultClassifierHints.GetDefaultPackagePropertyName(o),
            null);
    }

    private async Task CopyItem(INavigationHost navigationHost, IObject? o)
    {
        if (o == null)
        {
            return;
        }

        var extent = o.GetExtentOf();
        var found = await NavigatorForDialogs.Locate(
            navigationHost,
            new NavigatorForDialogs.NavigatorForDialogConfiguration
            {
                DefaultWorkspace = extent?.GetWorkspace(),
                DefaultExtent = extent
            });

        if (found == null)
        {
            // Nothing selected
            return;
        }

        ObjectOperations.CopyObject(o, found);
    }

    private async Task MoveItem(INavigationHost navigationHost, IObject? o)
    {
        if (o == null)
        {
            return;
        }

        var extent = o.GetExtentOf();
        var found = await NavigatorForDialogs.Locate(
            navigationHost,
            new NavigatorForDialogs.NavigatorForDialogConfiguration
            {
                DefaultWorkspace = extent?.GetWorkspace(),
                DefaultExtent = extent
            });

        if (found == null)
        {
            // Nothing selected
            return;
        }
            
        ObjectOperations.MoveObject(o, found);
    }

    private void DeleteItem(TreeViewItemParameter o)
    {
        if (o == null)
        {
            return;
        }

        if (
            MessageBox.Show(
                $"Shall the item '{NamedElementMethods.GetName(o.Element)}' be deleted?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            if (o.ParentElement is IExtent extent)
            {
                extent.elements().remove(o.Element);
            }
            else if (o.ParentProperty != null)
            {
                o.ParentElement.getOrDefault<IReflectiveCollection>(o.ParentProperty)?.remove(o.Element);
            }
            else
            {
                MessageBox.Show("Don't know how to delete the item...");
            }
        }
    }

    private async void EditItem(INavigationHost navigationHost, IObject? o)
    {
        if (o == null)
        {
            return;
        }

        try
        {
            await NavigatorForItems.NavigateToElementDetailView(navigationHost, o);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void CopyAsXmi(INavigationHost navigationHost, IObject? o)
    {
        if (o == null)
        {
            return;
        }

        var itemDialog = new ItemXmlViewWindow
        {
            IgnoreIDs = true,
            Owner = navigationHost.GetWindow(),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        itemDialog.UpdateContent(o);
        itemDialog.Show();
        itemDialog.CopyToClipboard();
        MessageBox.Show(
            itemDialog,
            "Content copied to clipboard",
            "Done",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    /// <summary>
    /// Creates a dialog in which the user can define the xmi
    /// which is afterwards imported
    /// </summary>
    /// <param name="navigationHost">Navigation host to be included</param>
    /// <param name="container">Element to which the parsed items shall be added</param>
    /// <returns>Task used for awaiter</returns>
    private async Task ImportByXmi(INavigationHost navigationHost, IObject? container)
    {
        if (container == null)
        {
            return;
        }

        // 1) 
        // Asks the user to include the Xmi
        var managementWorkspace = GiveMe.Scope.WorkspaceLogic.GetManagementWorkspace();
        var form =
            managementWorkspace.Resolve("#DatenMeister.ContextMenu.ImportByXmi", ResolveType.NoMetaWorkspaces)
                as IElement;
        var element = InMemoryObject.CreateEmpty();

        var result = await NavigatorForItems.NavigateToElementDetailView(
            navigationHost,
            new NavigateToItemConfig
            {
                Form = new FormDefinition(form),
                DetailElement = element
            });

        if (result.Result == NavigationResult.Saved)
        {
            try
            {
                // 2) 
                // Includes the result into the current item
                var xmlText = result.DetailElement.getOrDefault<string>("importXmi");
                ByXmlImporter.ImportByXml(container, xmlText);

                MessageBox.Show("OK" + xmlText);
            }
            catch (Exception exc)
            {
                var message = "Inclusion of object failed: " + exc;
                Logger.Error(message);
                MessageBox.Show(message);
            }
        }
    }
}