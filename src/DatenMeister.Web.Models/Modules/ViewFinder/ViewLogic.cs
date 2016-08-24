using System;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Models.Modules.ViewFinder
{
    public class ViewLogic
    {
        /// <summary>
        /// Defines the uri of the view to the view extents
        /// </summary>
        public const string UriViewExtent = "dm:///management/views";

        private readonly IWorkspaceCollection _workspaceCollection;
        private readonly IDotNetTypeLookup _dotNetTypeLookup;
        private readonly IDataLayerLogic _dataLayerLogic;

        public ViewLogic(IWorkspaceCollection workspaceCollection, IDotNetTypeLookup dotNetTypeLookup, IDataLayerLogic dataLayerLogic)
        {
            _workspaceCollection = workspaceCollection;
            _dotNetTypeLookup = dotNetTypeLookup;
            _dataLayerLogic = dataLayerLogic;
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
        public void Add(IObject view)
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

        /// <summary>
        /// Finds the association view for the given element in the detail view
        /// </summary>
        /// <param name="metaClass">Metaclass to be queried</param>
        /// <param name="detail">View type</param>
        /// <returns>The found element</returns>
        public IElement FindViewFor(IElement metaClass, ViewType type)
        {
            var viewExtent = GetViewExtent();
            var formAndFields = _dataLayerLogic.Get<_FormAndFields>(
                _dataLayerLogic.GetDataLayerOfExtent(viewExtent));

            foreach (
                var element in viewExtent.elements().
                WhenMetaClassIs(formAndFields.__DefaultViewForMetaclass).
                Select(x=> x as IElement))
            {
                var innerMetaClass = element.get(_FormAndFields._DefaultViewForMetaclass.metaclass);
                var innerType = element.get(_FormAndFields._DefaultViewForMetaclass.viewType);

                if (innerMetaClass.Equals(metaClass) && innerType.Equals(type))
                {
                    return element.get(_FormAndFields._DefaultViewForMetaclass.view) as IElement;
                }
            }

            return null;
        }
    }
}