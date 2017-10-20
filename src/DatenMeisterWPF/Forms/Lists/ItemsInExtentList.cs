using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ElementListViewControl
    {
        public void SetContent(IDatenMeisterScope scope, string workspaceId, string extentUrl)
        {
            var workLogic = scope.Resolve<IWorkspaceLogic>();
            var extent = workLogic.FindExtent(workspaceId, extentUrl);
            if (extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            SetContent(scope, extent.elements(), null);
            AddGenericButton("New Item", () =>
            {
                var mofFactory = new MofFactory(extent);
                var newElement = mofFactory.create(null);
                var elements = Navigator.TheNavigator.NavigateToElementDetailView(
                    Window.GetWindow(this), scope, newElement);
                elements.Closed += (x, y) =>
                {
                    extent.elements().add(newElement);
                    RefreshViewDefinition();
                    UpdateContent();
                };
            });

            AddDefaultButtons();
        }

    }
}