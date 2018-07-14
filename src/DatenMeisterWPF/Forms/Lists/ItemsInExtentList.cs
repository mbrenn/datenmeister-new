using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ListViewControl, INavigationGuest
    {
        private IExtent _extent;

        public string WorkspaceId { get; set; }
        public string ExtentUrl { get; set; }

        public ItemsInExtentList()
        {
            Loaded += ItemsInExtentList_Loaded;
        }

        private void ItemsInExtentList_Loaded(object sender, RoutedEventArgs e)
        {
            SetContent(WorkspaceId, ExtentUrl);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the enumeration of all views that may match to the shown items
        /// </summary>
        protected override IEnumerable<IElement> GetFormsForView()
        {
            return App.Scope.Resolve<IViewFinder>().FindViews((Items as IHasExtent)?.Extent as IUriExtent, null);
        }

        protected override IElement RequestForm()
        {
            var result = GetActualView();

            // Sets the generic buttons to create the new types
            if (result?.getOrDefault(_FormAndFields._ListForm.defaultTypesForNewElements)
                is IReflectiveCollection defaultTypesForNewItems)
            {
                foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                {
                    var typeName = type.get(_UML._CommonStructure._NamedElement.name);
                    AddGenericButton($"New {typeName}", () =>
                    {
                        var elements = NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, _extent, type);
                        elements.Closed += (x, y) =>
                        {
                            UpdateContent();
                        };
                    });
                }
            }
            
            // Sets the button for the new item
            AddGenericButton("New Item", () =>
            {
                var elements = NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, _extent);
                elements.Closed += (x, y) =>
                {
                    UpdateContent();
                };
            });

            // Adds the default button
            AddDefaultButtons();

            // Allows the deletion of an item
            AddRowItemButton(
                "Delete",
                item =>
                {
                    if (MessageBox.Show(
                            "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                        MessageBoxResult.Yes)
                    {
                        _extent.elements().remove(item);
                        SetContent(_extent.elements());
                    }
                });

            return result;
        }

        private IElement GetActualView()
        {
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            IElement result = null;

            if (Items == DetailItems)
            {
                // Uses the default view. 
                if (ViewDefinition.Mode == ViewDefinitionMode.Default)
                {
                    if (Items == DetailItems)
                    {
                        // Finds the view by the extent type
                        result = viewFinder.FindView((Items as IHasExtent)?.Extent as IUriExtent);
                    }
                    else
                    {
                        //ActualFormDefinition = viewFinder.FindView(DetailItems as IReflectiveCollection);
                    }
                }

                // Creates the view by creating the 'all Properties' view by parsing all the items
                if (ViewDefinition.Mode == ViewDefinitionMode.AllProperties
                    || (ViewDefinition.Mode == ViewDefinitionMode.Default && result == null))
                {
                    result = viewFinder.CreateView(DetailItems);
                }

                // Used, when an external function requires a specific view mode
                if (ViewDefinition.Mode == ViewDefinitionMode.Specific)
                {
                    result = ViewDefinition.Element;
                }
            }
            else
            {
                // User has selected a sub element. 
                result =
                    viewFinder.FindListViewFor((DetailItems as MofReflectiveSequence)?.MofObject);

                if (result == null)
                {
                    result = viewFinder.CreateView(DetailItems);
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the items of the given extent
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUrl">Url of the extent to be used</param>
        public void SetContent(string workspaceId, string extentUrl)
        {
            WorkspaceId = workspaceId;
            ExtentUrl = extentUrl;
            var workLogic = App.Scope.Resolve<IWorkspaceLogic>();
            workLogic.FindExtentAndWorkspace(workspaceId, extentUrl, out var workspace, out _extent);
            if (_extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            SetContent(_extent.elements());
        }

        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation 
        /// host. 
        /// </summary>
        public new void PrepareNavigation()
        {
            base.PrepareNavigation();

            NavigationHost.AddNavigationButton(
                "To Extents",
                () => NavigatorForExtents.NavigateToExtentList(NavigationHost, WorkspaceId),
                Icons.ExtentsShow,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Extent Info",
                () => NavigatorForExtents.OpenExtent(NavigationHost, WorkspaceId, ExtentUrl),
                null,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Show as tree",
                () =>
                {
                    if (_extent != null)
                    {
                        var window = new TreeViewWindow();
                        window.SetDefaultProperties();
                        window.SetCollection(_extent.elements());
                        window.ItemSelected += (x, y) =>
                        {
                            NavigatorForItems.NavigateToElementDetailView(NavigationHost, y.Item);
                        };
                        window.Show();
                    }
                }, 
                null,
                NavigationCategories.File + ".Views");
        }
    }
}