﻿using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.XMI.EMOF;

namespace DatenMeister.Models.Modules.ViewFinder
{
    public class ViewLogic
    {
        /// <summary>
        /// Defines the uri of the view to the view extents
        /// </summary>
        public const string UriViewExtent = "dm:///management/views";

        private readonly IWorkspaceCollection _workspaceCollection;
        private readonly IWorkspaceLogic _workspaceLogic;

        public ViewLogic(IWorkspaceCollection workspaceCollection, IWorkspaceLogic workspaceLogic)
        {
            _workspaceCollection = workspaceCollection;
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Integrates the the view logic into the workspace. 
        /// </summary>
        public void Integrate()
        {
            var mgmtWorkspace = _workspaceCollection.GetWorkspace(WorkspaceNames.Management);

            var dotNetUriExtent = new XmlUriExtent(_workspaceCollection, UriViewExtent);
            mgmtWorkspace.AddExtent(dotNetUriExtent);
        }


        /// <summary>
        /// Adds a view to the system
        /// </summary>
        /// <param name="view">View to be added</param>
        public void Add(IObject view)
        {
            var foundExtent = GetViewExtent();

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
        /// <param name="url">The Url to be queried</param>
        /// <returns>The found view or null if not found</returns>
        public IObject GetViewByUrl(string url)
        {
            return GetViewExtent().element(url);
        }

        /// <summary>
        /// Gets all forms and returns them as an enumeration
        /// </summary>
        /// <returns>Enumeration of forms</returns>
        public IReflectiveCollection GetAllViews()
        {
            var viewExtent = GetViewExtent();
            var formAndFields = GetFormAndFieldInstance(viewExtent);

            return viewExtent.elements()
                .WhenMetaClassIs(formAndFields.__Form);
        }

        /// <summary>
        /// Finds the association view for the given element in the detail view
        /// </summary>
        /// <param name="extent">Extent where the given type is located</param>
        /// <param name="metaClass">Metaclass to be queried</param>
        /// <param name="type">View type</param>
        /// <returns>The found element</returns>
        public IElement FindViewFor(IUriExtent extent, IElement metaClass, ViewType type)
        {
            var viewExtent = GetViewExtent();
            var formAndFields = GetFormAndFieldInstance(viewExtent);

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

        /// <summary>
        /// Gets the form and field instance which contains the references to 
        /// the metaclasses
        /// </summary>
        /// <param name="viewExtent">Extent of the view</param>
        /// <returns></returns>
        private _FormAndFields GetFormAndFieldInstance(IUriExtent viewExtent)
        {
            return  _workspaceLogic.Get<_FormAndFields>(
                _workspaceLogic.GetDataLayerOfExtent(viewExtent));
        }
    }
}