using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms.Detail
{
    public class NewWorkspaceControl : DetailFormControl
    {
        public void SetContent(IDatenMeisterScope scope)
        {
            var viewLogic = scope.Resolve<ViewLogic>();
            var viewExtent = viewLogic.GetViewExtent();

            var formElement = NamedElementMethods.GetByFullName(viewExtent, ViewDefinitions.PathNewWorkspaceForm);

            SetContentForNewObject(scope, formElement);
            AddDefaultButtons("Create");

            ElementSaved += (x, y) =>
            {
                var workspaceId = DetailElement.get("id").ToString();
                var annotation = DetailElement.get("annotation").ToString();

                var workspace = new Workspace(workspaceId, annotation);
                var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
                workspaceLogic.AddWorkspace(workspace);
            };
        }
    }
}