using System;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.EMOF;
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
        private readonly NamedElementMethods _namedElementMethods;

        public ViewLogic(IWorkspaceLogic workspaceLogic, NamedElementMethods namedElementMethods)
        {
            _workspaceLogic = workspaceLogic;
            _namedElementMethods = namedElementMethods;
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
        public void Add(DefaultViewForMetaclass defaultView, string id = null)
        {
            var viewExtent = GetViewExtent();
            var factory = new MofFactory(viewExtent);
            GetViewExtent().elements().add(factory.createFrom(defaultView, id));
        }

        /// <summary>
        /// Adds a default view for a certain meta class
        /// </summary>
        /// <param name="defaultView">Default view to be used</param>
        /// <param name="id">Id of the element that shall be created</param>
        public void Add(DefaultViewForExtentType defaultView, string id = null)
        {
            var viewExtent = GetViewExtent();
            var factory = new MofFactory(viewExtent);
            GetViewExtent().elements().add(factory.createFrom(defaultView, id));
        }
        

        private IUriExtent GetViewExtent()
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
                .GetAllDescendants()
                .WhenMetaClassIs(formAndFields.__Form);
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

            var viewExtent = GetViewExtent();
            var formAndFields = GetFormAndFieldInstance(viewExtent);
            var metaClassUri = metaClass.GetUri();
            var typeAsString = type.ToString();

            foreach (
                var element in viewExtent.elements().
                WhenMetaClassIs(formAndFields.__DefaultViewForMetaclass).
                Select(x=> x as IElement))
            {
                Debug.Assert(element != null, "element != null");

                var innerMetaClass = element.get(_FormAndFields._DefaultViewForMetaclass.metaclass);
                var innerType = element.get(_FormAndFields._DefaultViewForMetaclass.viewType).ToString();

                if (innerMetaClass.Equals(metaClassUri) && innerType.Equals(typeAsString))
                {
                    return element.getFirstOrDefault(_FormAndFields._DefaultViewForMetaclass.view) as IElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Looks in the view extent and checks for all elements, where the type of the extent is fitting to the view
        /// </summary>
        /// <param name="extentType">Type of the extent</param>
        /// <param name="list"></param>
        /// <returns>The found view</returns>
        public IElement FindViewForExtentType(string extentType, ViewType type)
        {
            if (string.IsNullOrEmpty(extentType))
            {
                return null;
            }

            var viewExtent = GetViewExtent();
            var formAndFields = GetFormAndFieldInstance(viewExtent);
            foreach (
                var element in viewExtent.elements().
                WhenMetaClassIs(formAndFields.__DefaultViewForExtentType).
                Select(x => x as IElement))
            {
                Debug.Assert(element != null, "element != null");

                var innerExtentType = element.get(_FormAndFields._DefaultViewForExtentType.extentType);

                if (innerExtentType.Equals(extentType))
                {
                    return element.getFirstOrDefault(_FormAndFields._DefaultViewForMetaclass.view) as IElement;
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the form and field instance which contains the references to 
        /// the metaclasses
        /// </summary>
        /// <param name="viewExtent">Extent of the view</param>
        /// <returns></returns>
        private _FormAndFields GetFormAndFieldInstance(IExtent viewExtent)
        {
            return _workspaceLogic.GetWorkspaceOfExtent(viewExtent).MetaWorkspace.Get<_FormAndFields>();
        }
    }
}