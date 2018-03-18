using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ExtentList : ListViewControl, INavigationGuest
    {
        private string _workspaceId;

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="workspaceId">Id of the workspace whose extents shall be shown</param>
        public void SetContent(string workspaceId)
        {
            _workspaceId = workspaceId;
            var viewExtent = App.Scope.Resolve<ViewLogic>().GetViewExtent();
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(App.Scope);
            var workspace = workspaceExtent.elements().WhenPropertyIs("id", workspaceId).FirstOrDefault() as IElement;

            var extents = workspace?.get("extents") as IReflectiveSequence;
            SetContent(
                extents, 
                NamedElementMethods.GetByFullName(viewExtent, ManagementViewDefinitions.PathExtentListView));

            AddDefaultButtons();
            AddRowItemButton("Show Items", ShowItems);
            
            void ShowItems(IObject extentElement)
            {
                var uri = extentElement.get("uri").ToString();

                var events = NavigatorForItems.NavigateToItemsInExtent(
                    NavigationHost,
                    workspaceId,
                    uri);
                events.Closed += (x, y) => UpdateContent();
            }
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
                NavigatorForExcelHandling.ImportFromExcel(NavigationHost, _workspaceId);
                UpdateContent();
            }

            void NewXmiExtent()
            {
                var events = NavigatorForItems.NavigateToNewXmiExtentDetailView(NavigationHost, _workspaceId);
                events.Closed += (x, y) => UpdateContent();
            }

            void AddZipCodeExample()
            {
                var extentManager = App.Scope.Resolve<IExtentManager>();
                ZipExampleController.AddZipCodeExample(extentManager, _workspaceId);
                UpdateContent();
            }
        }

        public override void OnMouseDoubleClick(IObject element)
        {
            var uri = element.get("uri").ToString();

            var events = NavigatorForItems.NavigateToItemsInExtent(
                NavigationHost,
                _workspaceId,
                uri);
        }
    }
}