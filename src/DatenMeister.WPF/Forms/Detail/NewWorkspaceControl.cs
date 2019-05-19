using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Detail
{
    public class NewWorkspaceControl : DetailFormControl
    {
        public void SetContent()
        {
            var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
            var viewExtent = viewLogic.GetInternalViewExtent();

            var formElement = NamedElementMethods.GetByFullName(
                viewExtent, 
                ManagementViewDefinitions.PathNewWorkspaceForm);

            AddDefaultButtons("Create");

            ElementSaved += (x, y) =>
            {
                var workspaceId = DetailElement.get("id").ToString();
                var annotation = DetailElement.get("annotation").ToString();

                var workspace = new Workspace(workspaceId, annotation);
                var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                workspaceLogic.AddWorkspace(workspace);
            };

        }
    }
}