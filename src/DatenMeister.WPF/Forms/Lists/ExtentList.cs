using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Forms.Base;
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

            Extent = ManagementProviderHelper.GetExtentsForWorkspaces(GiveMe.Scope);
        }

        private void ExtentList_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetContent(WorkspaceId);
        }

        /// <summary>
        /// Gets or sets the id to be shown in the workspace
        /// </summary>
        public string WorkspaceId { get; set; }

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="workspaceId">Id of the workspace whose extents shall be shown</param>
        public void SetContent(string workspaceId)
        {
            WorkspaceId = workspaceId;
            var workspace =
                Extent.elements().WhenPropertyHasValue("id", WorkspaceId).FirstOrDefault() as IElement;

            SetRootItem(workspace);

            // Registers upon events
            var eventManager = GiveMe.Scope.Resolve<ChangeEventManager>();
            EventHandle = eventManager.RegisterFor(Extent, (x, y) =>
                Tabs.FirstOrDefault()?.ControlAsNavigationGuest.UpdateView());
        }

        protected override void OnRecreateViews()
        {
            if (SelectedItem == null)
                return;

            var overridingDefinition = OverridingViewDefinition;

            if (IsExtentSelectedInTreeview)
            {
                var viewDefinition = overridingDefinition ?? 
                                     WorkspaceExtentFormGenerator.RequestFormForExtents(Extent, WorkspaceId, NavigationHost);

                EvaluateForm(
                    SelectedItem,
                    viewDefinition);
            }
            else
            {
                var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
                var form = viewLogic.GetItemTreeFormForObject(SelectedPackage, ViewDefinitionMode.Default);
                var viewDefinition = overridingDefinition ?? 
                                     new ViewDefinition(form);

                EvaluateForm(
                    SelectedItem,
                    viewDefinition);
            }
        }

        public override void OnMouseDoubleClick(IObject element)
        {
            var uri = element.get("uri").ToString();

            NavigatorForItems.NavigateToItemsInExtent(
                NavigationHost,
                WorkspaceId,
                uri);
        }
    }
}