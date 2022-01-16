#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Plugins;

namespace DatenMeister.Forms
{
    /// <summary>
    /// Defines the access to the view logic and abstracts the access to the view extent
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class FormsPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Stores the type of the extent containing the views
        /// </summary>
        public const string FormExtentType = "DatenMeister.Forms";

        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(FormsPlugin));

        private readonly ExtentCreator _extentCreator;
        private readonly ExtentSettings _extentSettings;
        private readonly IntegrationSettings _integrationSettings;
        private readonly IScopeStorage _scopeStorage;

        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Initializes a new instance of the FormLogic class
        /// </summary>
        /// <param name="workspaceLogic">The workspace being used</param>
        /// <param name="extentCreator">The support class to create extents</param>
        /// <param name="scopeStorage">The settings that had been used for integration</param>
        public FormsPlugin(IWorkspaceLogic workspaceLogic,
            ExtentCreator extentCreator,
            IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _extentCreator = extentCreator;
            _scopeStorage = scopeStorage;
            _integrationSettings = scopeStorage.Get<IntegrationSettings>();
            _extentSettings = scopeStorage.Get<ExtentSettings>();
        }

        /// <summary>
        /// Initializes a new instance of the FormLogic class
        /// </summary>
        /// <param name="workspaceLogic">The workspace being used</param>
        /// <param name="scopeStorage">The settings that had been used for integration</param>
        public FormsPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
            : this(workspaceLogic, new ExtentCreator(workspaceLogic, scopeStorage), scopeStorage)
        {
        }

        /// <summary>
        /// Gets the workspace logic of the view logic
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic => _workspaceLogic;

        /// <summary>
        /// Integrates the the view logic into the workspace.
        /// </summary>
        public void Start(PluginLoadingPosition position)
        {
            var mgmtWorkspace = _workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceManagement)
                                ?? throw new InvalidOperationException("Management Workspace is not found");

            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    // Creates the internal views for the DatenMeister
                    var dotNetUriExtent =
                        new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentInternalForm, _scopeStorage);
                    dotNetUriExtent.GetConfiguration().ExtentType = FormExtentType;
                    _workspaceLogic.AddExtent(mgmtWorkspace, dotNetUriExtent);
                    _extentSettings.extentTypeSettings.Add(new ExtentType(FormExtentType));
                    break;

                case PluginLoadingPosition.AfterLoadingOfExtents:
                    var extent = _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                        WorkspaceNames.WorkspaceManagement,
                        WorkspaceNames.UriExtentUserForm,
                        "DatenMeister.Forms_User",
                        FormExtentType,
                        _integrationSettings.InitializeDefaultExtents
                            ? ExtentCreationFlags.CreateOnly
                            : ExtentCreationFlags.LoadOrCreate
                    );

                    if (extent == null)
                        throw new InvalidOperationException("Extent for users is not found");

                    extent.GetConfiguration()
                        .AddDefaultTypes(new[]
                            {_DatenMeister.TheOne.Forms.__Form, _DatenMeister.TheOne.Forms.__FormAssociation});

                    break;
            }
        }

        /// <summary>
        /// Adds a view to the system
        /// </summary>
        /// <param name="type">Location Type to which the element shall be added</param>
        /// <param name="view">View to be added</param>
        public void Add(FormLocationType type, IObject view)
        {
            GetFormExtent(type).elements().add(view);
        }

        /// <summary>
        /// Gets the internal view extent being empty at each start-up
        /// </summary>
        /// <returns></returns>
        public IUriExtent GetInternalFormExtent()
        {
            if (_workspaceLogic.FindExtent(WorkspaceNames.UriExtentInternalForm) is not IUriExtent foundExtent)
            {
                throw new InvalidOperationException(
                    $"The form extent is not found in the management: {WorkspaceNames.UriExtentInternalForm}");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the internal view extent being empty at each start-up
        /// </summary>
        /// <returns></returns>
        public IUriExtent? GetInternalFormExtent(bool mayFail)
        {
            if (_workspaceLogic.FindExtent(WorkspaceNames.UriExtentInternalForm) is not IUriExtent foundExtent)
            {
                if (mayFail)
                {
                    return null;
                }

                throw new InvalidOperationException(
                    $"The form extent is not found in the management: {WorkspaceNames.UriExtentInternalForm}");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the extent of the user being stored on permanent storage
        /// </summary>
        /// <returns></returns>
        public IUriExtent GetUserFormExtent()
        {
            if (_workspaceLogic.FindExtent(WorkspaceNames.UriExtentUserForm) is not IUriExtent foundExtent)
            {
                throw new InvalidOperationException(
                    $"The form extent is not found in the management: {WorkspaceNames.UriExtentUserForm}");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the extent of the user being stored on permanent storage
        /// </summary>
        /// <returns></returns>
        public IUriExtent? GetUserFormExtent(bool mayFail)
        {
            if (_workspaceLogic.FindExtent(WorkspaceNames.UriExtentUserForm) is not IUriExtent foundExtent)
            {
                if (mayFail)
                {
                    return null;
                }

                throw new InvalidOperationException(
                    $"The form extent is not found in the management: {WorkspaceNames.UriExtentUserForm}");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the view extent.. Whether the default internal or the default external
        /// </summary>
        /// <param name="locationType">Type of the location to be used</param>
        /// <returns>The found extent of the given location</returns>
        public IUriExtent GetFormExtent(FormLocationType locationType)
        {
            return locationType switch
            {
                FormLocationType.Internal => GetInternalFormExtent(),
                FormLocationType.User => GetUserFormExtent(),
                _ => throw new ArgumentOutOfRangeException(nameof(locationType), locationType, null)
            };
        }

        /// <summary>
        /// Gets the view as given by the url of the view
        /// </summary>
        /// <param name="url">The Url to be queried</param>
        /// <returns>The found view or null if not found</returns>
        public IObject? GetFormByUrl(string url)
        {
            if (url.StartsWith(WorkspaceNames.UriExtentInternalForm))
            {
                return GetUserFormExtent().element(url);
            }

            return GetInternalFormExtent().element(url);
        }

        /// <summary>
        /// Gets all forms and returns them as an enumeration
        /// </summary>
        /// <returns>Enumeration of forms</returns>
        public IReflectiveCollection GetAllForms()
        {
            return
                new TemporaryReflectiveCollection(
                    GetAllFormExtents()
                        .SelectMany(x => x.elements()
                            .GetAllDescendants(new[]
                                {_UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement})
                            .WhenMetaClassIsOneOf(
                                _DatenMeister.TheOne.Forms.__Form,
                                _DatenMeister.TheOne.Forms.__DetailForm,
                                _DatenMeister.TheOne.Forms.__ListForm)),
                    true);
        }


        /// <summary>
        /// Adds a new form association between the form and the metaclass
        /// </summary>
        /// <param name="form">Form to be used to create the form association</param>
        /// <param name="metaClass">The metaclass being used for form association</param>
        /// <param name="formType">Type to be added</param>
        /// <returns></returns>
        public IElement AddFormAssociationForMetaclass(
            IElement form,
            IElement metaClass,
            _DatenMeister._Forms.___FormType formType)
        {
            var factory = new MofFactory(form);

            var formAssociation = factory.create(_DatenMeister.TheOne.Forms.__FormAssociation);
            var name = NamedElementMethods.GetName(form);

            formAssociation.set(_DatenMeister._Forms._FormAssociation.formType, formType);
            formAssociation.set(_DatenMeister._Forms._FormAssociation.form, form);
            formAssociation.set(_DatenMeister._Forms._FormAssociation.metaClass, metaClass);
            formAssociation.set(_DatenMeister._Forms._FormAssociation.name, $"Association for {name}");

            return formAssociation;
        }

        /// <summary>
        /// Gets an enumeration of all form extents. The form extents have
        /// to be in the Management Workspace and be of type "DatenMeister.Forms"
        /// </summary>
        /// <returns>Enumeration of form extents</returns>
        public IEnumerable<IExtent> GetAllFormExtents()
        {
            var result = _workspaceLogic.GetManagementWorkspace().extent
                .Where(x => x.GetConfiguration().ContainsExtentType(FormExtentType))
                .OfType<IUriExtent>()
                .ToList();

            // Even if internal extent or user form extent does not have the the flag
            var internalFormExtent = GetInternalFormExtent();
            if (result.All(x => x.contextURI() != internalFormExtent.contextURI()))
            {
                result.Add(internalFormExtent);
            }

            var userFormExtent = GetInternalFormExtent();
            if (result.All(x => x.contextURI() != userFormExtent.contextURI()))
            {
                result.Add(userFormExtent);
            }

            return result;
        }

        /// <summary>
        /// Gets all view associations and returns them as an enumeration
        /// </summary>
        /// <returns>Enumeration of associations</returns>
        public IReflectiveCollection GetAllFormAssociations()
        {
            return new TemporaryReflectiveCollection(
                GetAllFormExtents()
                    .SelectMany(x =>
                        x.elements()
                            .GetAllDescendants(new[]
                                {_UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement})
                            .WhenMetaClassIsOneOf(_DatenMeister.TheOne.Forms.__FormAssociation)),
                true);
        }

        /// <summary>
        /// Removes the view association from the database
        /// </summary>
        /// <param name="selectedExtentType">Extent type which is currently selected</param>
        /// <param name="viewExtent">The view extent which shall be looked through to remove the view association</param>
        public bool RemoveFormAssociationForExtentType(string selectedExtentType, IExtent? viewExtent = null)
        {
            var result = false;
            viewExtent ??= GetUserFormExtent();

            foreach (var foundElement in viewExtent
                         .elements()
                         .GetAllDescendantsIncludingThemselves()
                         .WhenMetaClassIs(_DatenMeister.TheOne.Forms.__FormAssociation)
                         .WhenPropertyHasValue(_DatenMeister._Forms._FormAssociation.extentType, selectedExtentType)
                         .OfType<IElement>())
            {
                RemoveElement(viewExtent, foundElement);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Removes the view association from the database
        /// </summary>
        /// <param name="metaClass">The metaclass which shall be used for the detailled form</param>
        /// <param name="viewExtent">The view extent which shall be looked through to remove the view association</param>
        public bool RemoveFormAssociationForDetailMetaClass(IElement metaClass, IExtent? viewExtent = null)
        {
            var result = false;
            viewExtent ??= GetUserFormExtent();

            foreach (var foundElement in viewExtent
                         .elements()
                         .GetAllDescendantsIncludingThemselves()
                         .WhenMetaClassIs(_DatenMeister.TheOne.Forms.__FormAssociation)
                         .WhenPropertyHasValue(_DatenMeister._Forms._FormAssociation.metaClass, metaClass)
                         .WhenPropertyHasValue(_DatenMeister._Forms._FormAssociation.formType,
                             _DatenMeister._Forms.___FormType.Detail)
                         .OfType<IElement>())
            {
                RemoveElement(viewExtent, foundElement);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Removes the element from the given extent.
        /// This is more a helper method 
        /// </summary>
        /// <param name="viewExtent">The extent in which the element is located</param>
        /// <param name="foundElement">The found element</param>
        private static void RemoveElement(IExtent viewExtent, IElement foundElement)
        {
            var container = foundElement.container();
            if (container != null)
            {
                container.getOrDefault<IReflectiveCollection>(_UML._Packages._Package.packagedElement)
                    ?.remove(foundElement);
            }
            else
            {
                viewExtent.elements().remove(foundElement);
            }
        }
    }
}