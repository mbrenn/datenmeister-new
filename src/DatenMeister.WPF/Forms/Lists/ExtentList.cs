using System;
using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.ChangeEvents;
using DatenMeister.Forms;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.ManagementProviders.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Lists
{
    public class ExtentList : ItemExplorerControl
    {
        /// <summary>
        /// Initializes a new instance of the ExtentList class
        /// </summary>
        public ExtentList()
        {
            Loaded += ExtentList_Loaded;

            Extent = ManagementProviderPlugin.GetExtentForWorkspaces(GiveMe.Scope.WorkspaceLogic);
        }

        /// <summary>
        /// Gets or sets the id to be shown in the workspace
        /// </summary>
        public string WorkspaceId { get; set; } = string.Empty;

        private void ExtentList_Loaded(object sender, RoutedEventArgs e)
        {
            SetContent(WorkspaceId);
        }

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="workspaceId">Id of the workspace whose extents shall be shown</param>
        public void SetContent(string workspaceId)
        {
            WorkspaceId = workspaceId;

            if (Extent.elements().WhenPropertyHasValue("id", WorkspaceId).FirstOrDefault() is IElement workspace)
            {
                SetRootItem(workspace);

                // Registers upon events
                var eventManager = GiveMe.Scope.ScopeStorage.Get<ChangeEventManager>();
                EventHandle = eventManager.RegisterFor(Extent, (x, y) =>
                    Dispatcher?.Invoke(() =>
                        Tabs.FirstOrDefault()?.ControlAsNavigationGuest.UpdateForm()));
            }
        }

        protected override void OnRecreateForms()
        {
            if (SelectedItem == null)
                return;

            var overridingDefinition = OverridingViewDefinition;

            if (IsExtentSelectedInTreeview ||
                SelectedItem is IElement selectedElement &&
                selectedElement.metaclass?.equals(_DatenMeister.TheOne.Management.__Workspace) == true)
            {
                var formDefinition =
                    overridingDefinition ??
                    WorkspaceExtentFormGenerator.RequestFormForExtents(Extent, WorkspaceId,
                        NavigationHost);

                EvaluateForm(
                    SelectedItem,
                    formDefinition);
            }
            else if (SelectedItem != null)
            {
                var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
                var form = viewLogic.GetItemTreeFormForObject(
                               SelectedItem,
                               FormDefinitionMode.Default,
                               CurrentViewModeId)
                           ?? throw new InvalidOperationException("form == null");
                var formDefinition = overridingDefinition ??
                                     new FormDefinition(form);

                EvaluateForm(
                    SelectedItem,
                    formDefinition);
            }
        }

        public virtual void OnMouseDoubleClick(IObject element)
        {
            var uri = element.getOrDefault<string>("uri") ?? string.Empty;

            _ = NavigatorForItems.NavigateToItemsInExtent(
                NavigationHost,
                WorkspaceId,
                uri);
        }

        /// <inheritdoc />
        public override ViewExtensionInfo GetViewExtensionInfo()
        {
            return new ViewExtensionInfoExploreExtents(NavigationHost, this)
            {
                WorkspaceId = WorkspaceId,
                RootElement = RootItem,
                SelectedElement = SelectedItem
            };
        }
    }
}