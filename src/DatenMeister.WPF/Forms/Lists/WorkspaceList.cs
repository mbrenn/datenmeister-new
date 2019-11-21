using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.GuiElements;
using DatenMeister.WPF.Navigation;

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

            var eventManager = GiveMe.Scope.Resolve<ChangeEventManager>();
            EventHandle = eventManager.RegisterFor(Extent,
                (x,y) =>
                    Tabs.FirstOrDefault()?.ControlAsNavigationGuest.UpdateView());
        }

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        protected override void OnRecreateViews()
        {
            ViewDefinition view = null;
            var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Get<_FormAndFields>();

            if (OverridingViewDefinition?.Mode == ViewDefinitionMode.Specific)
            {
                view = OverridingViewDefinition;
                
                // Checks, if the given form is correct
                if (!ClassifierMethods.IsSpecializedClassifierOf(
                    (OverridingViewDefinition.Element as IElement)?.getMetaClass(), 
                    formAndFields.__ExtentForm))
                {
                    MessageBox.Show("Overriding form is not of type ExtentForm.");
                    view = null;
                }
            }

            if (view == null)
            {
                var selectedItemMetaClass = (SelectedPackage as IElement)?.getMetaClass();
                if (selectedItemMetaClass != null
                    && NamedElementMethods.GetFullName(selectedItemMetaClass)?.Contains("Workspace") == true)
                {
                    var workspaceId = SelectedPackage.get("id")?.ToString();
                    view = WorkspaceExtentFormGenerator.RequestFormForExtents(Extent, workspaceId, NavigationHost);
                }
                else
                {
                    view = WorkspaceExtentFormGenerator.RequestFormForWorkspaces(Extent, NavigationHost);
                }
            }

            PrepareNavigation(view);

            // Sets the workspaces
            EvaluateForm(SelectedItem, view);
        }

        /// <summary>
        /// Prepares the navigation
        /// </summary>
        /// <param name="viewDefinition">Definition of the view</param>
        private void PrepareNavigation(ViewDefinition viewDefinition)
        {
            viewDefinition.ViewExtensions.Add(
                new InfoLineDefinition(() => new TextBlock
                {
                    Inlines =
                    {
                        new Bold {Inlines = {new Run("All Workspaces")}}
                    }
                }));
        }

        public override void OnMouseDoubleClick(IObject element)
        {
            var workspaceId = element.get("id").ToString();
            NavigatorForExtents.NavigateToExtentList(NavigationHost, workspaceId);
        }
    }
}