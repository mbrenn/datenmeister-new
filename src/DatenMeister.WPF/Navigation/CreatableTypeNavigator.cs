using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.Forms;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders.View;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Navigation
{
    public class CreatableTypeNavigator : ControlNavigation
    {
        /// <summary>
        /// Sets the selected types
        /// </summary>
        public IElement? SelectedType { get; private set; }

        /// <summary>
        /// Shows a dialog in which the user can select one type out of the list of createable types
        /// </summary>
        /// <param name="window">Window to tbe used</param>
        /// <param name="extent">Extent to whom the type shall be created</param>
        /// <param name="buttonName">Name of the button</param>
        /// <param name="workspaceName">Name of the workspace</param>
        /// <param name="extentUri">Name of the workspace</param>
        /// <returns>The control navigation</returns>
        public async Task<NavigateToElementDetailResult?> NavigateToSelectCreateableType(
            INavigationHost window,
            IExtent? extent,
            string buttonName = "Create Instance", 
            string? workspaceName = null,
            string? extentUri = null)
        {
            var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
            var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
            var viewDefinitions = GiveMe.Scope.Resolve<ManagementViewDefinitions>();

            // Gets metaworkspace and metaextent 
            var defaultTypePackage = extent?.GetConfiguration().GetDefaultTypePackages().ToList();
            IWorkspace? metaWorkspace = null;
            IUriExtent? metaExtent = null;
            if (defaultTypePackage == null || !defaultTypePackage.Any())
            {
                // Selects the type workspace, if the current extent is in data workspace or some other workspace whose meta level is of type
                // Otherwise, select the first meta workspace and extent
                var typeWorkspace = workspaceLogic.GetTypesWorkspace();
                var workspace = extent == null ? null : workspaceLogic.GetWorkspaceOfExtent(extent);
                if (workspace?.MetaWorkspaces?.Contains(typeWorkspace) == true)
                {
                    metaWorkspace = workspaceLogic.GetTypesWorkspace();
                    metaExtent = metaWorkspace.FindExtent(WorkspaceNames.UriExtentUserTypes);
                }
                else
                {
                    metaWorkspace = workspace?.MetaWorkspaces?.FirstOrDefault();
                    metaExtent = metaWorkspace?.extent.FirstOrDefault() as IUriExtent;
                }
            }

            var element = InMemoryObject.CreateEmpty().SetReferencedExtent(viewLogic.GetInternalFormExtent());
            
            //var items = extentFunctions.GetCreatableTypes(extent).CreatableTypes;
            var parameter = new FindTypeFormParameter
            {
                PreSelectedPackage = defaultTypePackage?.FirstOrDefault()
            };

            if (metaWorkspace != null) parameter.Workspace = metaWorkspace;
            if (workspaceName != null) parameter.WorkspaceName = workspaceName;
            if (metaExtent != null) parameter.Extent = metaExtent;
            if (extentUri != null) parameter.ExtentUri = extentUri;
            
            var formPathToType = viewDefinitions.GetFindTypeForm(parameter, buttonName);

            var navigateToItemConfig = new NavigateToItemConfig(element)
            {
                Form = new FormDefinition(formPathToType),
                Title = "Select Type"
            };

            var result = await Navigator.CreateDetailWindow(window, navigateToItemConfig);
            if (result.Result == NavigationResult.Saved)
            {
                var detailElement = result.DetailElement;
                if (result.Result == NavigationResult.Saved && detailElement != null)
                {
                    var metaClass = detailElement.getOrDefault<IElement>("selectedType");
                    if (metaClass != null)
                    {
                        SelectedType = metaClass;
                    }

                    OnClosed();
                }
            }

            return result;
        }
    }
}