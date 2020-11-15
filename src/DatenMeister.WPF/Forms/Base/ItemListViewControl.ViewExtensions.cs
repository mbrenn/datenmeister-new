using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Provider.CSV;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Commands;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Windows;
using Microsoft.Win32;

namespace DatenMeister.WPF.Forms.Base
{
    public partial class ItemListViewControl
    {
        /// <summary>
        ///     Prepares the navigation of the host. The function is called by the navigation
        ///     host.
        /// </summary>
        public IEnumerable<ViewExtension> GetViewExtensions()
        {
            void ViewCollection(IReflectiveCollection reflectiveCollection)
            {
                var dlg = new ItemXmlViewWindow
                {
                    Owner = Window.GetWindow(this)
                };

                dlg.UpdateContent(reflectiveCollection);
                dlg.ShowDialog();
            }

            void ExportToCSV(IReflectiveCollection items)
            {
                try
                {
                    if (Items == null) throw new InvalidOperationException("Items == null");

                    var dlg = new SaveFileDialog
                    {
                        DefaultExt = "csv",
                        Filter = "CSV-Files|*.csv|All Files|*.*"
                    };

                    if (dlg.ShowDialog(Window.GetWindow(this)) == true)
                    {
                        var loader = new CsvLoader(GiveMe.Scope.Resolve<IWorkspaceLogic>());
                        var memoryProvider = new InMemoryProvider();
                        var temporary = new MofUriExtent(memoryProvider, "dm:///temp");
                        var copier = new ExtentCopier(new MofFactory(temporary));
                        copier.Copy(Items, temporary.elements());

                        loader.Save(
                            memoryProvider,
                            dlg.FileName,
                            InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__CsvSettings));

                        MessageBox.Show($"CSV Export completed. \r\n{temporary.elements().Count()} Items exported.");
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"Export failed\r\n{exc}");
                }
            }

            void CopyContent(IReflectiveCollection items)
            {
                CopyToClipboardCommand.Execute(this, CopyType.Default);
            }

            void CopyContentAsXmi(IReflectiveCollection items)
            {
                CopyToClipboardCommand.Execute(this, CopyType.AsXmi);
            }

            if (EffectiveForm.getOrDefault<bool>(_DatenMeister._Forms._ListForm.inhibitEditItems) == false)
            {
                yield return
                    new RowItemButtonDefinition(
                        "Edit",
                        NavigateToElement,
                        ButtonPosition.Before);
            }

            if (EffectiveForm.getOrDefault<bool>(_DatenMeister._Forms._ListForm.inhibitDeleteItems) == false)
            {
                yield return
                    new RowItemButtonDefinition(
                        "Delete",
                        (guest, item) =>
                        {
                            if (Items != null)
                            {
                                var name = NamedElementMethods.GetName(item);
                                if (MessageBox.Show(
                                        $"Are you sure to delete the item '{name}'?",
                                        "Confirmation",
                                        MessageBoxButton.YesNo) ==
                                    MessageBoxResult.Yes)
                                {
                                    Items?.remove(item);
                                }
                            }
                        });
            }

            yield return
                new CollectionMenuButtonDefinition(
                    "View as Xmi",
                    ViewCollection,
                    null,
                    "Collection");

            yield return
                new CollectionMenuButtonDefinition(
                    "Export CSV",
                    ExportToCSV,
                    Icons.ExportCSV,
                    "Collection");

            yield return
                new CollectionMenuButtonDefinition(
                    "Copy",
                    CopyContent,
                    null,
                    "Selection");

            yield return
                new CollectionMenuButtonDefinition(
                    "Copy as XMI",
                    CopyContentAsXmi,
                    null,
                    "Selection");

            // 2) Get the view extensions by the plugins
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
            var extentData = new ViewExtensionInfoCollection(NavigationHost, this)
            {
                Collection = Items
            };

            foreach (var plugin in viewExtensionPlugins)
            {
                foreach (var extension in plugin.GetViewExtensions(extentData))
                {
                    yield return extension;
                }
            }
        }

        public void EvaluateViewExtensions(ICollection<ViewExtension> viewExtensions)
        {
        }
    }
}
