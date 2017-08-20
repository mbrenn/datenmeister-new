using Autofac;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms
{
    public class WorkspaceViewControl : ListViewControl
    {
        public void Show(IDatenMeisterScope scope)
        {
            var workspaceView = scope.Resolve<WorkspaceView>();
            FormDefinition = workspaceView.CreateForm();

            UpdateContent();
        }
    }
}