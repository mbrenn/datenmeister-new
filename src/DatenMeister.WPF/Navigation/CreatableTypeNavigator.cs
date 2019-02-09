using System.Linq;
using Autofac;
using Autofac.Features.Metadata;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Navigation
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
        public IControlNavigation NavigateToSelectCreateableType(
            INavigationHost window,
            IExtent extent,
            string buttonName = "Create")
        {
            var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
            var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
            var viewDefinitions = GiveMe.Scope.Resolve<ManagementViewDefinitions>();
           
            return window.NavigateTo(
                () =>
                {
                    var defaultTypePackage = extent.GetDefaultTypePackage();
                    IWorkspace metaWorkspace = null;
                    IExtent metaExtent = null;
                    if (defaultTypePackage == null)
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

                    var element = InMemoryObject.CreateEmpty().SetReferencedExtent(viewLogic.GetInternalViewExtent());
                    //var items = extentFunctions.GetCreatableTypes(extent).CreatableTypes;
                    var formPathToType = viewDefinitions.GetFindTypeForm(defaultTypePackage, metaWorkspace, metaExtent);

                    var control = new DetailFormControl();
                    control.Title = "Select Type";
                    control.SetContent(element, formPathToType);
                    control.AddDefaultButtons(buttonName);
                    control.ElementSaved += (x, y) =>
                    {
                        if (control.DetailElement.GetOrDefault("selectedType") is IElement metaClass)
                        {
                            SelectedType = metaClass;
                        }

                        OnClosed();
                    };

                    return control;
                },
                NavigationMode.ForceNewWindow);
        }
    }
}