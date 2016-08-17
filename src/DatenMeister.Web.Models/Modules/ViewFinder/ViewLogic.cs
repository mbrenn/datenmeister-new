using System;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Web.Models.Modules.ViewFinder
{
    public class ViewLogic
    {
        /// <summary>
        /// Defines the uri of the view to the view extents
        /// </summary>
        public const string UriViewExtent = "dm:///management/views";

        private readonly IWorkspaceCollection _workspaceCollection;
        private readonly IDotNetTypeLookup _dotNetTypeLookup;

        public ViewLogic(IWorkspaceCollection workspaceCollection, IDotNetTypeLookup dotNetTypeLookup)
        {
            _workspaceCollection = workspaceCollection;
            _dotNetTypeLookup = dotNetTypeLookup;
        }

        /// <summary>
        /// Integrates the the view logic into the workspace. 
        /// </summary>
        public void Integrate()
        {
            var mgmtWorkspace = _workspaceCollection.GetWorkspace(WorkspaceNames.Management);
            var dotNetUriExtent = new DotNetExtent(UriViewExtent, _dotNetTypeLookup);

            mgmtWorkspace.AddExtent(dotNetUriExtent);
        }


        /// <summary>
        /// Adds a view to the system
        /// </summary>
        /// <param name="view">View to be added</param>
        public void AddView(IObject view)
        {
            var foundExtent = GetViewExtent();

            if (!(view is DotNetElement))
            {
                throw new ArgumentException("Currently only views as DotNetElements are supported");
            }

            foundExtent.elements().add(view);
        }

        private IUriExtent GetViewExtent()
        {
            var mgmtWorkspace = _workspaceCollection.GetWorkspace(WorkspaceNames.Management);

            var foundExtent = mgmtWorkspace.FindExtent(UriViewExtent);
            if (foundExtent == null)
            {
                throw new InvalidOperationException("The view extent is not found in the management");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the view as given by the name of the view
        /// </summary>
        /// <param name="viewname">Name of the view to be queried</param>
        /// <returns>The found view or null if not found</returns>
        public IObject GetView(string viewname)
        {
            var viewExtent = GetViewExtent();
            return viewExtent.elements().WhenPropertyIs("name", viewname).FirstOrDefault() as IObject;
        }
    }
}