using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.ChangeEvents;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Provider.ManagementProviders.Workspaces;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.GuiElements;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;
using MessageBox = System.Windows.MessageBox;

namespace DatenMeister.WPF.Forms.Lists
{
    public class WorkspaceList : ItemExplorerControl
    {
        public WorkspaceList()
        {
            Loaded += WorkspaceList_Loaded;
        }

        private void WorkspaceList_Loaded(object sender, RoutedEventArgs e)
        {
            Extent = ManagementProviderHelper.GetExtentsForWorkspaces(GiveMe.Scope);
            SetRootItem(Extent);

            var eventManager = GiveMe.Scope.ScopeStorage.Get<ChangeEventManager>();
            EventHandle = eventManager.RegisterFor(Extent,
                (x,y) =>
                    Tabs.FirstOrDefault()?.ControlAsNavigationGuest.UpdateForm());
        }

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        protected override void OnRecreateForms()
        {
            FormDefinition? form = null;

            if (OverridingViewDefinition?.Mode == FormDefinitionMode.Specific)
            {
                form = OverridingViewDefinition;
                
                // Checks, if the given form is correct
                if (!ClassifierMethods.IsSpecializedClassifierOf(
                    (OverridingViewDefinition.Element as IElement)?.getMetaClass(), 
                    _DatenMeister.TheOne.Forms.__ExtentForm))
                {
                    MessageBox.Show("Overriding form is not of type ExtentForm.");
                    form = null;
                }
            }

            if (form == null)
            {
                var selectedItemMetaClass = (SelectedItem as IElement)?.getMetaClass();
                var extent = Extent ?? throw new InvalidOperationException("Extent == null");
                if (selectedItemMetaClass != null && SelectedItem != null
                    && NamedElementMethods.GetFullName(selectedItemMetaClass)?.Contains("Workspace") == true)
                {
                    var workspaceId = SelectedItem.getOrDefault<string>("id");
                    form = WorkspaceExtentFormGenerator.RequestFormForExtents(extent, workspaceId, NavigationHost);
                }
                else
                {
                    form = WorkspaceExtentFormGenerator.RequestFormForWorkspaces(extent, NavigationHost);
                }
            }

            // Sets the workspaces
            if (SelectedItem == null)
            {
                MessageBox.Show("None");
                return;
            }
            
            EvaluateForm(SelectedItem, form);
        }

        /// <summary>
        /// Prepares the navigation
        /// </summary>
        public override IEnumerable<ViewExtension> GetViewExtensions()
        {
            yield return new InfoLineDefinition(() => new TextBlock
            {
                Inlines =
                {
                    new Bold {Inlines = {new Run("All Workspaces")}}
                }
            });
            
            foreach (var extension in base.GetViewExtensions()) yield return extension;

        }

        public virtual void OnMouseDoubleClick(IObject element)
        {
            var workspaceId = element.getOrDefault<string>("id");
            NavigatorForExtents.NavigateToExtentList(NavigationHost, workspaceId);
        }

        /// <inheritdoc />
        public override ViewExtensionInfo GetViewExtensionInfo()
        {
            return new ViewExtensionInfoExploreWorkspace(NavigationHost, this)
            {
                RootElement = RootItem,
                SelectedElement = SelectedItem
            };
        }
    }
}