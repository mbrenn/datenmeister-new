using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Forms.Specific;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ExtentList : ItemExplorerControl
    {
        /// <summary>
        /// Initializes a new instance of the ExtentList class
        /// </summary>
        public ExtentList()
        {
            Loaded += ExtentList_Loaded;
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
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(App.Scope);
            var workspace =
                workspaceExtent.elements().WhenPropertyHasValue("id", WorkspaceId).FirstOrDefault() as IElement;

            var extents = workspace?.get("extents") as IReflectiveSequence;
            SetItems(extents);
        }

        protected override void OnRecreateViews()
        {
            if (SelectedItems == null)
            {
                return;
            }

            var viewDefinition = ListRequests.RequestFormForExtents(this, WorkspaceId);
            PrepareNavigation(viewDefinition);

            var uiElement = AddTab(
                SelectedItems,
                viewDefinition);
        }

        /// <summary>
        /// Adds the navigation control elements in the host
        /// </summary>
        private void PrepareNavigation(ViewDefinition viewDefinition)
        {
        }

        public override void OnMouseDoubleClick(IObject element)
        {
            var uri = element.get("uri").ToString();

            var events = NavigatorForItems.NavigateToItemsInExtent(
                NavigationHost,
                WorkspaceId,
                uri);
        }
    }
}