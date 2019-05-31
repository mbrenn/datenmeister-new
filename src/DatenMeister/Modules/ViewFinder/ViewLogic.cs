using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Plugins;
using DatenMeister.Models.Forms;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.Modules.ViewFinder
{
    /// <summary>
    /// Defines the access to the view logic and abstracts the access to the view extent
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterInitialization)]
    public class ViewLogic : IDatenMeisterPlugin
    {
        /// <summary>
        /// Stores the type of the extent containing the views 
        /// </summary>
        public const string ViewExtentType = "DatenMeister.Views";
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentCreator _extentCreator;

        public ViewLogic(IWorkspaceLogic workspaceLogic, ExtentCreator extentCreator)
        {
            _workspaceLogic = workspaceLogic;
            _extentCreator = extentCreator;
        }

        /// <summary>
        /// Integrates the the view logic into the workspace. 
        /// </summary>
        public void Start(PluginLoadingPosition position)
        {
            var mgmtWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameManagement);

            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    // Creates the internal views for the DatenMeister
                    var dotNetUriExtent =
                        new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriInternalViewExtent);
                    dotNetUriExtent.SetExtentType(ViewExtentType);
                    _workspaceLogic.AddExtent(mgmtWorkspace, dotNetUriExtent);
                    break;

                case PluginLoadingPosition.AfterInitialization:
                    _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                        WorkspaceNames.NameManagement,
                        WorkspaceNames.UriUserViewExtent,
                        "DatenMeister.Views_User",
                        ViewExtentType
                    );
                    break;
            }
        }

        /// <summary>
        /// Adds a view to the system
        /// </summary>
        /// <param name="type">Location Type to which the element shall be added</param>
        /// <param name="view">View to be added</param>
        public void Add(ViewLocationType type, IObject view)
        {
            GetViewExtent(type).elements().add(view);
        }

        /// <summary>
        /// Adds the form
        /// </summary>
        /// <param name="type">Location Type to which the element shall be added</param>
        /// <param name="form">Default view to be used</param>
        /// <param name="id">Id of the element that shall be created</param>
        public void Add(ViewLocationType type, Form form, string id = null)
        {
            var viewExtent = GetInternalViewExtent();
            var factory = new MofFactory(viewExtent);
            GetViewExtent(type).elements().add(factory.createFrom(form, id));
        }

        /// <summary>
        /// Adds a default view for a certain meta class
        /// </summary>
        /// <param name="type">Location Type to which the element shall be added</param>
        /// <param name="defaultView">Default view to be used</param>
        /// <param name="id">Id of the element that shall be created</param>
        public void Add(ViewLocationType type, ViewAssociation defaultView, string id = null)
        {
            var viewExtent = GetInternalViewExtent();
            var factory = new MofFactory(viewExtent);
            GetViewExtent(type).elements().add(factory.createFrom(defaultView, id));
        }

        /// <summary>
        /// Gets the internal view extent being empty at each start-up
        /// </summary>
        /// <returns></returns>
        public IUriExtent GetInternalViewExtent()
        {
            if (!(_workspaceLogic.FindExtent(WorkspaceNames.UriInternalViewExtent) is IUriExtent foundExtent))
            {
                throw new InvalidOperationException("The view extent is not found in the management");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the extent of the user being stored on permanent storage
        /// </summary>
        /// <returns></returns>
        public IUriExtent GetUserViewExtent()
        {
            if (!(_workspaceLogic.FindExtent(WorkspaceNames.UriUserViewExtent) is IUriExtent foundExtent))
            {
                throw new InvalidOperationException("The view extent is not found in the management");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the view extent.. Whether the default internal or the default external
        /// </summary>
        /// <param name="locationType">Type of the location to be used</param>
        /// <returns>The found extent of the given location</returns>
        public IUriExtent GetViewExtent(ViewLocationType locationType)
        {
            switch (locationType)
            {
                case ViewLocationType.Internal:
                    return GetInternalViewExtent();
                case ViewLocationType.User:
                    return GetUserViewExtent();
                default:
                    throw new ArgumentOutOfRangeException(nameof(locationType), locationType, null);
            }
        }

        /// <summary>
        /// Gets the view as given by the url of the view
        /// </summary>
        /// <param name="url">The Url to be queried</param>
        /// <returns>The found view or null if not found</returns>
        public IObject GetViewByUrl(string url)
        {
            if (url.StartsWith(WorkspaceNames.UriInternalViewExtent))
            {
                return GetUserViewExtent().element(url);
            }
            else
            {
                return GetInternalViewExtent().element(url);
            }
        }

        /// <summary>
        /// Gets all forms and returns them as an enumeration
        /// </summary>
        /// <returns>Enumeration of forms</returns>
        public IReflectiveCollection GetAllForms()
        {
            var internalViewExtent = GetInternalViewExtent();
            var userViewExtent = GetUserViewExtent();
            var formAndFields = GetFormAndFieldInstance(internalViewExtent);

            return internalViewExtent.elements()
                    .Union(userViewExtent.elements())
                .GetAllDescendants(new[] {_UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement })
                .WhenMetaClassIsOneOf(formAndFields.__Form, formAndFields.__DetailForm, formAndFields.__ListForm);
        }

        /// <summary>
        /// Gets all view associations and returns them as an enumeration
        /// </summary>
        /// <returns>Enumeration of assocations</returns>
        public IReflectiveCollection GetAllViewAssociations()
        {
            var internalViewExtent = GetInternalViewExtent();
            var userViewExtent = GetUserViewExtent();
            var formAndFields = GetFormAndFieldInstance(internalViewExtent);

            return internalViewExtent.elements()
                .Union(userViewExtent.elements())
                .GetAllDescendants(new[] { _UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement })
                .WhenMetaClassIsOneOf(formAndFields.__ViewAssociation);
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

        public IElement GetDetailForm(IObject element, IExtent extent, ViewDefinitionMode viewDefinitionMode )
        {
            throw new InvalidOperationException();
        }

        public IElement GetExtentForm(IUriExtent extent, ViewDefinitionMode viewDefinitionMode)
        {
            throw new InvalidOperationException();
        }

        public IElement GetExtentForm(IReflectiveCollection collection, ViewDefinitionMode viewDefinitionMode)
        {
            if (viewDefinitionMode != ViewDefinitionMode.AllProperties)
            {
                // Try to find the view, but very improbable
            }

            var factory = new MofFactory(GetUserViewExtent());


            throw new InvalidOperationException();
        }

        public IElement GetItemTreeFormForObject(IObject element, IExtent extent, ViewDefinitionMode viewDefinitionMode)
        {
            throw new InvalidOperationException();
        }

        protected IElement GetItemTreeFormForObjectsProperties(
            IObject element, 
            string property, 
            IElement metaClass,
            IUriExtent extent, 
            ViewDefinitionMode viewDefinitionMode)
        {
            throw new InvalidOperationException();
        }
    }
}