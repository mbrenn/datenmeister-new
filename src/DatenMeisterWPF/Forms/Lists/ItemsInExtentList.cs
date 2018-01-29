﻿using System.Collections.Generic;
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
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ListViewControl, INavigationGuest
    {
        private string _workspaceId;
        private string _extentUrl;
        private IExtent _extent;

        /// <inheritdoc />
        /// <summary>
        /// Gets the enumeration of all views that may match to the shown items
        /// </summary>
        public override IEnumerable<IElement> GetFormsForView()
        {
            return App.Scope.Resolve<IViewFinder>().FindViews((Items as IHasExtent)?.Extent as IUriExtent, null);
        }

        /// <summary>
        /// Sets the items of the given extent
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUrl">Url of the extent to be used</param>
        public void SetContent(string workspaceId, string extentUrl)
        {
            _workspaceId = workspaceId;
            _extentUrl = extentUrl;
            var workLogic = App.Scope.Resolve<IWorkspaceLogic>();
            workLogic.FindExtentAndWorkspace(workspaceId, extentUrl, out var workspace, out _extent);
            if (_extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            SetContent(_extent.elements(), null);

            // First, sets the first buttons upon the priorities
            if (ActualFormDefinition?.get(_FormAndFields._ListForm.defaultTypesForNewElements) is IReflectiveCollection defaultTypesForNewItems)
            {
                foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                {
                    var typeName = type.get(_UML._CommonStructure._NamedElement.name);
                    AddGenericButton($"New {typeName}", () =>
                    {
                        var elements = NavigatorForItems.NavigateToNewItemForExtent(NavigationHost, extent, type);
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
                var elements = Navigator.TheNavigator.NavigateToNewItem(NavigationHost, _extent.elements());
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
                        SetContent(_extent.elements(), null);
                    }
                });
        }

        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation 
        /// host. 
        /// </summary>
        public new void PrepareNavigation()
        {
            base.PrepareNavigation();

            NavigationHost.AddNavigationButton(
                "To Extent",
                () => NavigatorForExtents.NavigateToExtentList(NavigationHost, _workspaceId),
                Icons.ExtentsShow,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Extent Info",
                () => NavigatorForExtents.OpenExtent(NavigationHost, _workspaceId, _extentUrl),
                null,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "ShowAsTree",
                () =>
                {
                    if (_extent != null)
                    {
                        var window = new TreeViewWindow();
                        window.SetDefaultProperties();
                        window.SetCollection(_extent.elements());
                        window.ItemSelected += (x, y) => { MessageBox.Show(y.Item.ToString()); };
                        window.Show();
                    }
                }, 
                null,
                NavigationCategories.File + ".Views");
        }
    }
}