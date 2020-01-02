using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.WPF.Navigation
{
    public class CreatableTypeNavigator : ControlNavigation
    {
        /// <summary>
        /// Sets the selected types
        /// </summary>
        public IElement SelectedType { get; private set; }

        /// <summary>
        /// Shows a dialog in which the user can select one type out of the list of createable types
        /// </summary>
        /// <param name="window">Window to tbe used</param>
        /// <param name="extent">Extent to whom the type shall be created</param>
        /// <param name="buttonName">Name of the button</param>
        /// <returns>The control navigation</returns>
        public async Task<NavigateToElementDetailResult> NavigateToSelectCreateableType(
            INavigationHost window,
            IExtent extent,
            string buttonName = "Create")
        {
            var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
            var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
            var viewDefinitions = GiveMe.Scope.Resolve<ManagementViewDefinitions>();


            var defaultTypePackage = extent.GetDefaultTypePackages()?.ToList();
            IWorkspace metaWorkspace = null;
            IExtent metaExtent = null;
            if (defaultTypePackage == null || !defaultTypePackage.Any())
            {
                // Selects the type workspace, if the current extent is in data workspace or some other workspace whose meta level is of type
                // Otherwise, select the first meta workspace and extent
                var typeWorkspace = workspaceLogic.GetTypesWorkspace();
                var workspace = workspaceLogic.GetWorkspaceOfExtent(extent);
                if (workspace?.MetaWorkspaces?.Contains(typeWorkspace) == true)
                {
                    metaWorkspace = workspaceLogic.GetTypesWorkspace();
                    metaExtent = metaWorkspace.FindExtent(WorkspaceNames.UriUserTypesExtent);
                }
                else
                {
                    metaWorkspace = workspace?.MetaWorkspaces?.FirstOrDefault();
                    metaExtent = metaWorkspace?.extent?.FirstOrDefault();
                }
            }

            var element = InMemoryObject.CreateEmpty().SetReferencedExtent(viewLogic.GetInternalFormExtent());
            //var items = extentFunctions.GetCreatableTypes(extent).CreatableTypes;
            var formPathToType = viewDefinitions.GetFindTypeForm(defaultTypePackage?.FirstOrDefault(), metaWorkspace, metaExtent);

            var navigateToItemConfig = new NavigateToItemConfig()
            {
                DetailElement = element,
                FormDefinition = formPathToType
            };

            var result = await Navigator.CreateDetailWindow(window, navigateToItemConfig);
            if (result.Result == NavigationResult.Saved)
            {
                var metaClass = result.DetailElement.getOrDefault<IElement>("selectedType");
                if (metaClass != null)
                {
                    SelectedType = metaClass;
                }

                OnClosed();
            }

            return result;
        }
    }
}