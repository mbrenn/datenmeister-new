﻿using System;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.ViewFinder
{
    /// <summary>
    /// Defines the access to the view logic and abstracts the access to the view extent
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ViewLogic
    {
        /// <summary>
        /// Defines the uri of the view to the view extents
        /// </summary>
        public const string UriViewExtent = "dm:///management/views";

        private readonly IWorkspaceLogic _workspaceLogic;

        public ViewLogic(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Integrates the the view logic into the workspace. 
        /// </summary>
        public void Integrate()
        {
            var mgmtWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameManagement);

            var dotNetUriExtent = new MofUriExtent(new InMemoryProvider(), UriViewExtent);
            _workspaceLogic.AddExtent(mgmtWorkspace, dotNetUriExtent);
        }

        /// <summary>
        /// Adds a view to the system
        /// </summary>
        /// <param name="view">View to be added</param>
        public void Add(IObject view)
        {
            GetViewExtent().elements().add(view);
        }

        /// <summary>
        /// Adds the form
        /// </summary>
        /// <param name="form">Default view to be used</param>
        /// <param name="id">Id of the element that shall be created</param>
        public void Add(Form form, string id = null)
        {
            var viewExtent = GetViewExtent();
            var factory = new MofFactory(viewExtent);
            GetViewExtent().elements().add(factory.createFrom(form, id));
        }

        /// <summary>
        /// Adds a default view for a certain meta class
        /// </summary>
        /// <param name="defaultView">Default view to be used</param>
        /// <param name="id">Id of the element that shall be created</param>
        public void Add(ViewAssociation defaultView, string id = null)
        {
            var viewExtent = GetViewExtent();
            var factory = new MofFactory(viewExtent);
            GetViewExtent().elements().add(factory.createFrom(defaultView, id));
        }

        public IUriExtent GetViewExtent()
        {
            var mgmtWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameManagement);

            var foundExtent = mgmtWorkspace.FindExtent(UriViewExtent);
            if (foundExtent == null)
            {
                throw new InvalidOperationException("The view extent is not found in the management");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the view as given by the url of the view
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
                .GetAllDescendants(new[] {_UML._CommonStructure._Namespace.member})
                .WhenMetaClassIsOneOf(formAndFields.__Form, formAndFields.__DetailForm, formAndFields.__ListForm);
        }

        /// <summary>
        /// Finds the association view for the given element in the detail view
        /// </summary>
        /// <param name="metaClass">Metaclass to be queried</param>
        /// <param name="type">View type</param>
        /// <returns>The found element</returns>
        public IElement FindViewFor(IElement metaClass, ViewType type)
        {
            if (metaClass == null)
            {
                // No Metaclass, so return null
                return null;
            }

            return FindViewFor(type, null, NamedElementMethods.GetFullName(metaClass), metaClass);
        }

        /// <summary>
        /// Looks in the view extent and checks for all elements, where the type of the extent is fitting to the view
        /// </summary>
        /// <param name="extentType">Type of the extent</param>
        /// <param name="type">Type of the view</param>
        /// <returns>The found view</returns>
        public IElement FindViewForExtentType(string extentType, ViewType type)
        {
            if (string.IsNullOrEmpty(extentType))
            {
                return null;
            }

            return FindViewFor(type, extentType, null, null);
        }


        /// <summary>
        /// Finds the view for the given item.
        /// First, it looks for the specific instance
        /// Second, it looks by the given type
        /// </summary>
        /// <param name="value">Value, whose view is currently is requested</param>
        /// <param name="viewType">Type of the view being queried</param>
        /// <returns>The found view or null, if not found</returns>
        public IElement FindViewForValue(IObject value, ViewType viewType)
        {
            return FindViewFor((value as IElement)?.metaclass, viewType);
        }

        /// <summary>
        /// Find st
        /// </summary>
        /// <param name="viewType">The view type to be found</param>
        /// <param name="extentType">The extent type whose view is queried. May be null, if not relevant</param>
        /// <param name="metaClassName">The uri of the metaclass whose view is queried. May be null, if not relevant</param>
        /// <param name="metaClass">The element of the metaclass whose view is queried. May be null, if not relevant</param>
        /// <returns>The found view or null, if not found</returns>
        public IElement FindViewFor(
            ViewType viewType,
            string extentType,
            string metaClassName,
            IElement metaClass)
        {
            var viewExtent = GetViewExtent();
            var formAndFields = GetFormAndFieldInstance(viewExtent);

            var foundPoints = -1;
            IElement foundView = null;

            foreach (
                var element in viewExtent.elements().
                    WhenMetaClassIs(formAndFields.__ViewAssociation).
                    Select(x => x as IElement))
            {
                var points = 0;
                if (element == null) throw new NullReferenceException("element");

                var innerExtentType = element.getOrDefault(_FormAndFields._ViewAssociation.extentType)?.ToString();
                var innerMetaClass = element.getOrDefault(_FormAndFields._ViewAssociation.metaclass) as IElement;
                var innerMetaClassName = element.getOrDefault(_FormAndFields._ViewAssociation.metaclassName)?.ToString();
                var innerViewType = (ViewType) (element.getOrDefault(_FormAndFields._ViewAssociation.viewType) ?? ViewType.Detail);

                var innerView = element.getOrDefault(_FormAndFields._ViewAssociation.view) as IElement;

                var isMatching = true;

                // Now go through each property and get the points
                if (extentType != null && innerExtentType != null)
                {
                    if (extentType == innerExtentType)
                    {
                        points++;
                    }
                    else
                    {
                        isMatching = false;
                    }
                }

                if (metaClassName != null && innerMetaClassName != null)
                {
                    if (metaClassName == innerMetaClassName)
                    {
                        points++;
                    }
                    else
                    {
                        isMatching = false;
                    }
                }

                if (metaClass != null && innerMetaClass != null)
                {
                    if (metaClass == innerMetaClass)
                    {
                        points++;
                    }
                    else
                    {
                        isMatching = false;
                    }
                }

                if (viewType != innerViewType)
                {
                    isMatching = false;
                }
                
                // The matching view with the maximum points win
                if (isMatching)
                {
                    if (points > foundPoints)
                    {
                        foundPoints = points;
                        foundView = innerView;
                    }
                }
            }

            return foundView;
        }

        /// <summary>
        /// Gets the form and field instance which contains the references to 
        /// the metaclasses
        /// </summary>
        /// <param name="viewExtent">Extent of the view</param>
        /// <returns></returns>
        private _FormAndFields GetFormAndFieldInstance(IExtent viewExtent)
        {
            return _workspaceLogic.GetWorkspaceOfExtent(viewExtent).GetFromMetaWorkspace<_FormAndFields>();
        }
    }
}