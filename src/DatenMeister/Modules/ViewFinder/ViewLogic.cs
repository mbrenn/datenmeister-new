using System;
using System.Diagnostics;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
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
        private static readonly ClassLogger Logger = new ClassLogger(typeof(ViewLogic));

        /// <summary>
        /// Stores a debug variable that can be used to extent the debugging of view retrieval process.
        /// </summary>
        private const bool ActivateDebuggingForViewRetrieval = false;
        
        /// <summary>
        /// Stores the type of the extent containing the views 
        /// </summary>
        public const string ViewExtentType = "DatenMeister.Views";
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentCreator _extentCreator;
        private readonly NamedElementMethods _namedElementMethods;

        public ViewLogic(IWorkspaceLogic workspaceLogic, ExtentCreator extentCreator, NamedElementMethods namedElementMethods)
        {
            _workspaceLogic = workspaceLogic;
            _extentCreator = extentCreator;
            _namedElementMethods = namedElementMethods;
        }

        /// <summary>
        /// Integrates the the view logic into the workspace. 
        /// </summary>
        public void Integrate()
        {
            var mgmtWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.NameManagement);

            // Creates the internal views for the DatenMeister
            var dotNetUriExtent = new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriInternalViewExtent);
            dotNetUriExtent.SetExtentType(ViewExtentType);
            _workspaceLogic.AddExtent(mgmtWorkspace, dotNetUriExtent);

            _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                WorkspaceNames.NameManagement,
                WorkspaceNames.UriUserViewExtent,
                "DatenMeister.Views_User",
                ViewExtentType
            );
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
        /// <param name="metaClass">Metaclass of the elements being shown</param>
        /// <param name="type">Type of the view</param>
        /// <returns>The found view</returns>
        public IElement FindViewForExtentType(string extentType, IElement metaClass, ViewType type)
        {
            if (string.IsNullOrEmpty(extentType) && metaClass == null)
            {
                return null;
            }

            return FindViewFor(
                type, 
                extentType, 
                metaClass == null ? null : NamedElementMethods.GetFullName(metaClass),
                metaClass);
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
            var foundPoints = 0;
            IElement foundView = null;
            var viewAssociations = GetAllViewAssociations().Select(x=> x as IElement).ToList();
            InternalDebug("---");
            InternalDebug("# of ViewAssociations: " + viewAssociations.Count);

            foreach (
                var element in viewAssociations)
            {
                InternalDebug("-");
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
                        InternalDebug("-- MATCH: ExtentType: " + extentType + ", ViewAssociation ExtentType: " + innerExtentType );
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: ExtentType: " + extentType + ", ViewAssociation ExtentType: " + innerExtentType );
                        isMatching = false;
                    }
                }

                if (metaClassName != null && innerMetaClassName != null)
                {
                    if (metaClassName == innerMetaClassName)
                    {
                        InternalDebug("-- MATCH: metaClassName: " + metaClassName + ", ViewAssociation innerMetaClassName: " + innerMetaClassName);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: metaClassName: " + metaClassName + ", ViewAssociation innerMetaClassName: " + innerMetaClassName);
                        isMatching = false;
                    }
                }

                if (metaClass != null && innerMetaClass != null)
                {
                    if (metaClass == innerMetaClass)
                    {
                        InternalDebug("-- MATCH: metaClass: " + UmlNameResolution.GetName(metaClass) +
                                      ", ViewAssociation innerMetaClass: " + UmlNameResolution.GetName(innerMetaClass));
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: metaClass: " + UmlNameResolution.GetName(metaClass) +
                                      ", ViewAssociation innerMetaClass: " + UmlNameResolution.GetName(innerMetaClass));
                        isMatching = false;
                    }
                }

                if (viewType != innerViewType)
                {
                    InternalDebug("-- NO MATCH: viewType: " + viewType + ", ViewAssociation viewType: " + innerViewType);
                    isMatching = false;
                }
                else
                {
                    InternalDebug("-- MATCH: viewType: " + viewType + ", ViewAssociation viewType: " + innerViewType);
                }

                InternalDebug("-- Points: " + points + ", Matched" + isMatching);
                
                // The matching view with the maximum points win
                if (isMatching)
                {
                    if (points > foundPoints)
                    {
                        foundPoints = points;
                        foundView = innerView;

                        InternalDebug("-- Selected!");
                    }
                }
            }

            return foundView;
        }

        /// <summary>
        /// Writes the information to the debugger, if the ActivateDebuggingForViewRetrieval is configured as true
        /// </summary>
        /// <param name="s"></param>
        private void InternalDebug(string s)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ActivateDebuggingForViewRetrieval)
#pragma warning disable 162
            {
                Logger.Trace(s);
            }
#pragma warning restore 162
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