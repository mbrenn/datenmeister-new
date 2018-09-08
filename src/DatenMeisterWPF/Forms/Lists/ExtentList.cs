using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
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
            var workspace = workspaceExtent.elements().WhenPropertyHasValue("id", workspaceId).FirstOrDefault() as IElement;

            var extents = workspace?.get("extents") as IReflectiveSequence;

            var uiElement = AddTab(
                extents, 
                new ViewDefinition("Workspaces", ListRequests.RequestFormForExtents()));

            ListRequests.AddButtonsForExtents(uiElement.Control, workspaceId);
        }

        /// <summary>
        /// Adds the navigation control elements in the host
        /// </summary>
        public new void PrepareNavigation()
        {
            NavigationHost.AddNavigationButton(
                "New Xmi Extent",
                NewXmiExtent,
                null,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Zip-Code Example",
                AddZipCodeExample,
                null,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Import Excel",
                ImportFromExcel,
                Icons.ImportExcel,
                NavigationCategories.File + ".Import");

            base.PrepareNavigation();

            void ImportFromExcel()
            {
                NavigatorForExcelHandling.ImportFromExcel(NavigationHost, WorkspaceId);
                UpdateContent();
            }

            void NewXmiExtent()
            {
                var events = NavigatorForItems.NavigateToNewXmiExtentDetailView(NavigationHost, WorkspaceId);
                events.Closed += (x, y) => UpdateContent();
            }

            void AddZipCodeExample()
            {
                var zipCodeExampleManager = App.Scope.Resolve<ZipCodeExampleManager>();
                zipCodeExampleManager.AddZipCodeExample(WorkspaceId);
                UpdateContent();
            }

            // Adds the information line
            AddInfoLine(
                new TextBlock
                {
                    Inlines =
                    {
                        new Bold {Inlines = {new Run("Workspace: ")}},
                        new Run(WorkspaceId)
                    }
                });
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