using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.HelpingExtents;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ExtentList : ElementListViewControl
    {

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="scope">Scope of the DatenMeister</param>
        /// <param name="id">Id of the workspace whose extents shall be shown</param>
        public void SetContent(IDatenMeisterScope scope, string id)
        {
            var viewDefinitions = scope.Resolve<ViewDefinitions>();

            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(scope);
            var workspace = workspaceExtent.elements().WhenPropertyIs("id", id).FirstOrDefault() as IElement;

            var extents = workspace?.get("extents") as IReflectiveSequence;
            SetContent(scope, extents, viewDefinitions.GetExtentListForm());

            AddDefaultButtons();

        }
    }
}