using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.HelpingExtents;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ExtentList : ElementListViewControl
    {

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="scope">Scope of the DatenMeister</param>
        /// <param name="workspaceId">Id of the workspace whose extents shall be shown</param>
        public void SetContent(IDatenMeisterScope scope, string workspaceId)
        {
            var viewDefinitions = scope.Resolve<ViewDefinitions>();

            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(scope);
            var workspace = workspaceExtent.elements().WhenPropertyIs("id", workspaceId).FirstOrDefault() as IElement;

            var extents = workspace?.get("extents") as IReflectiveSequence;
            SetContent(scope, extents, viewDefinitions.GetExtentListForm());

            AddDefaultButtons();
            AddRowItemButton("Open Extent", extentElement =>
            {
                Navigator.TheNavigator.NavigateTo(
                    Window.GetWindow(this),
                    () =>
                    {
                        var workLogic = scope.Resolve<IWorkspaceLogic>();
                        var uri = extentElement.get("uri").ToString();
                        var extent = workLogic.FindExtent(workspaceId, uri);
                        if (extent == null)
                        {
                            return null;
                        }


                        var control = new ElementListViewControl();
                        control.SetContent(
                            scope,
                            extent.elements(),
                            null);
                        control.AddDefaultButtons();

                        return control;
                    });
            });

        }
    }
}