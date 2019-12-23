#nullable enable

using System;
using System.Linq;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Forms.FormFinder
{
    /// <summary>
    /// Defines the access to the view logic and abstracts the access to the view extent
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FormLogic : IDatenMeisterPlugin
    {
        /// <summary>
        /// Stores the type of the extent containing the views
        /// </summary>
        public const string ViewExtentType = "DatenMeister.Views";

        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentCreator _extentCreator;
        private readonly IntegrationSettings _integrationSettings;
        
        /// <summary>
        /// Stores the cached form and fields
        /// </summary>
        private _FormAndFields? _cachedFormAndField;

        /// <summary>
        /// Gets the workspace logic of the view logic
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic => _workspaceLogic;

        /// <summary>
        /// Initializes a new instance of the FormLogic class
        /// </summary>
        /// <param name="workspaceLogic">The workspace being used</param>
        /// <param name="extentCreator">The support class to create extents</param>
        /// <param name="integrationSettings">The settings that had been used for integration</param>
        public FormLogic(IWorkspaceLogic workspaceLogic, ExtentCreator extentCreator, IntegrationSettings integrationSettings)
        {
            _workspaceLogic = workspaceLogic;
            _extentCreator = extentCreator;
            _integrationSettings = integrationSettings;
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
                        new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriInternalFormExtent);
                    dotNetUriExtent.SetExtentType(ViewExtentType);
                    _workspaceLogic.AddExtent(mgmtWorkspace, dotNetUriExtent);
                    break;

                case PluginLoadingPosition.AfterLoadingOfExtents:
                    var extent = _extentCreator.GetOrCreateXmiExtentInInternalDatabase(
                        WorkspaceNames.NameManagement,
                        WorkspaceNames.UriUserFormExtent,
                        "DatenMeister.Views_User",
                        ViewExtentType,
                        _integrationSettings.InitializeDefaultExtents ? ExtentCreationFlags.CreateOnly : ExtentCreationFlags.LoadOrCreate
                    );

                    var formAndFields = _workspaceLogic.GetTypesWorkspace().Get<_FormAndFields>();

                    extent.AddDefaultTypePackages(new[] {formAndFields.__Form});
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
        /// Adds the form
        /// </summary>
        /// <param name="type">Location Type to which the element shall be added</param>
        /// <param name="form">Default view to be used</param>
        /// <param name="id">Id of the element that shall be created</param>
        public void Add(FormLocationType type, Form form, string? id = null)
        {
            var viewExtent = GetInternalFormExtent();
            var factory = new MofFactory(viewExtent);
            GetFormExtent(type).elements().add(factory.createFrom(form, id));
        }

        /// <summary>
        /// Adds a default view for a certain meta class
        /// </summary>
        /// <param name="type">Location Type to which the element shall be added</param>
        /// <param name="defaultForm">Default view to be used</param>
        /// <param name="id">Id of the element that shall be created</param>
        public void Add(FormLocationType type, FormAssociation defaultForm, string? id = null)
        {
            var viewExtent = GetInternalFormExtent();
            var factory = new MofFactory(viewExtent);
            GetFormExtent(type).elements().add(factory.createFrom(defaultForm, id));
        }

        /// <summary>
        /// Gets the internal view extent being empty at each start-up
        /// </summary>
        /// <returns></returns>
        public IUriExtent GetInternalFormExtent()
        {
            if (!(_workspaceLogic.FindExtent(WorkspaceNames.UriInternalFormExtent) is IUriExtent foundExtent))
            {
                throw new InvalidOperationException("The view extent is not found in the management");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the extent of the user being stored on permanent storage
        /// </summary>
        /// <returns></returns>
        public IUriExtent GetUserFormExtent()
        {
            if (!(_workspaceLogic.FindExtent(WorkspaceNames.UriUserFormExtent) is IUriExtent foundExtent))
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
        public IUriExtent GetFormExtent(FormLocationType locationType)
        {
            switch (locationType)
            {
                case FormLocationType.Internal:
                    return GetInternalFormExtent();
                case FormLocationType.User:
                    return GetUserFormExtent();
                default:
                    throw new ArgumentOutOfRangeException(nameof(locationType), locationType, null);
            }
        }

        /// <summary>
        /// Gets the view as given by the url of the view
        /// </summary>
        /// <param name="url">The Url to be queried</param>
        /// <returns>The found view or null if not found</returns>
        public IObject GetFormByUrl(string url)
        {
            if (url.StartsWith(WorkspaceNames.UriInternalFormExtent))
            {
                return GetUserFormExtent().element(url);
            }
            else
            {
                return GetInternalFormExtent().element(url);
            }
        }

        /// <summary>
        /// Gets all forms and returns them as an enumeration
        /// </summary>
        /// <returns>Enumeration of forms</returns>
        public IReflectiveCollection GetAllForms()
        {
            var internalViewExtent = GetInternalFormExtent();
            var userViewExtent = GetUserFormExtent();
            var formAndFields = GetFormAndFieldInstance(internalViewExtent);

            return internalViewExtent.elements()
                .Union(userViewExtent.elements())
                .GetAllDescendants(new[] {_UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement})
                .WhenMetaClassIsOneOf(formAndFields.__Form, formAndFields.__DetailForm, formAndFields.__ListForm);
        }

        /// <summary>
        /// Gets all view associations and returns them as an enumeration
        /// </summary>
        /// <returns>Enumeration of associations</returns>
        public IReflectiveCollection GetAllFormAssociations()
        {
            var internalViewExtent = GetInternalFormExtent();
            var userViewExtent = GetUserFormExtent();
            var formAndFields = GetFormAndFieldInstance(internalViewExtent);

            return internalViewExtent.elements()
                .Union(userViewExtent.elements())
                .GetAllDescendants(new[] {_UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement})
                .WhenMetaClassIsOneOf(formAndFields.__ViewAssociation);
        }
        
        /// <summary>
        /// Removes the view association from the database
        /// </summary>
        /// <param name="selectedExtentType">Extent type which is currently selected</param>
        /// <param name="viewExtent">The view extent which shall be looked through to remove the view association</param>
        public bool RemoveViewAssociationForExtentType(string selectedExtentType, IExtent? viewExtent = null)
        {
            var result = false;
            viewExtent ??= GetUserFormExtent();
            
            var formAndFields = GetFormAndFieldInstance(viewExtent);
            foreach (var foundElement in viewExtent
                .elements()
                .GetAllDescendantsIncludingThemselves()
                .WhenMetaClassIs(formAndFields.__ViewAssociation)
                .WhenPropertyHasValue(_FormAndFields._ViewAssociation.extentType, selectedExtentType)
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
            
            var formAndFields = GetFormAndFieldInstance(viewExtent);
            foreach (var foundElement in viewExtent
                .elements()
                .GetAllDescendantsIncludingThemselves()
                .WhenMetaClassIs(formAndFields.__ViewAssociation)
                .WhenPropertyHasValue(_FormAndFields._ViewAssociation.metaClass, metaClass)
                .WhenPropertyHasValue(_FormAndFields._ViewAssociation.viewType, FormType.Detail)
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

        /// <summary>
        /// Gets the form and field instance which contains the references to
        /// the metaclasses
        /// </summary>
        /// <param name="viewExtent">Extent of the view</param>
        /// <returns></returns>
        public _FormAndFields GetFormAndFieldInstance(IExtent? viewExtent = null)
        {
            if (_cachedFormAndField != null)
            {
                return _cachedFormAndField;
            }

            viewExtent ??= GetUserFormExtent();
            _cachedFormAndField =
                _workspaceLogic.GetWorkspaceOfExtent(viewExtent).GetFromMetaWorkspace<_FormAndFields>();
            return _cachedFormAndField;
        }

        public IElement GetDetailForm(IObject? element, IExtent extent, FormDefinitionMode formDefinitionMode)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                // Tries to find the form
                var viewFinder = new FormFinder(this);
                var foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        metaClass = (element as IElement)?.getMetaClass(),
                        FormType = FormType.Detail,
                        extentType = extent == null ? string.Empty : extent.GetExtentType()
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    return foundForm;
                }
            }

            // Ok, we have not found the form. So create one
            var formCreator = CreateFormCreator();
            return formCreator.CreateDetailForm(element);
        }

        public IElement? GetExtentForm(IUriExtent extent, FormDefinitionMode formDefinitionMode)
        {
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder(this);
                var foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = extent.GetExtentType(),
                        FormType = FormType.TreeItemExtent
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    return foundForm;
                }
            }

            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                return formCreator.CreateExtentForm(
                    extent,
                    CreationMode.All | CreationMode.ForListForms);
            }

            return null;
        }

        /// <summary>
        /// Gets the extent form containing the subforms
        /// </summary>
        /// <param name="subForms">The forms to be added to the extent forms</param>
        /// <returns>The created extent</returns>
        public IElement GetExtentFormForSubforms(params IElement[] subForms)
        {
            var formCreator = CreateFormCreator();
            return formCreator.CreateExtentForm(subForms);
        }

        /// <summary>
        /// Gets one of the list forms for the extent. If the extent form is available, but
        /// the form creator thinks about creating a list form for the extent, it will query this
        /// method
        /// </summary>
        /// <param name="extent">Extent for which the list is created</param>
        /// <param name="metaClass">Metaclass of the items that are listed now</param>
        /// <param name="formDefinitionMode">The view definition mode</param>
        /// <returns>The found or created list form</returns>
        public IElement? GetListFormForExtent(
            IExtent extent,
            IElement metaClass,
            FormDefinitionMode formDefinitionMode)
        {
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder(this);
                var foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = extent.GetExtentType(),
                        FormType = FormType.TreeItemExtent,
                        metaClass = metaClass
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    return foundForm;
                }
            }

            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                return formCreator.CreateListFormForMetaClass(metaClass, CreationMode.All);
            }

            return null;
        }

        /// <summary>
        /// Gets the list form for an elements property to be shown in sub item view or other views
        /// </summary>
        /// <param name="element">Element whose property is enumerated</param>
        /// <param name="property">Name of the property to be enumeration</param>
        /// <param name="formDefinitionMode">The view definition mode</param>
        /// <returns>The list form for the list</returns>
        public IElement? GetListFormForElementsProperty(
            IObject element,
            string property,
            FormDefinitionMode formDefinitionMode = FormDefinitionMode.Default)
        {
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder(this);
                var foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = (element as IHasExtent)?.Extent?.GetExtentType() ?? string.Empty,
                        FormType = FormType.ObjectList
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    return foundForm;
                }
            }

            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                var formCreator = CreateFormCreator();
                var createdForm = formCreator.CreateListFormForElements(
                    element.get<IReflectiveCollection>(property),
                    CreationMode.All | CreationMode.OnlyCommonProperties);

                return createdForm;
            }

            return null;
        }

        public IElement? GetExtentForm(IReflectiveCollection collection, FormDefinitionMode formDefinitionMode)
        {
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                // Try to find the view, but very improbable
            }

            var formCreator = CreateFormCreator();
            return formCreator.CreateExtentForm(collection, CreationMode.All);
        }

        /// <summary>
        /// Gets the list view next to the item explorer control
        /// </summary>
        /// <param name="element">Element for which the list view will be created</param>
        /// <param name="formDefinitionMode">Defines the method how to retrieve the form</param>
        /// <returns>Found extent form</returns>
        public IElement? GetItemTreeFormForObject(IObject element, FormDefinitionMode formDefinitionMode)
        {
            var extent = (element as IHasExtent)?.Extent;
            if (extent == null)
            {
                throw new InvalidOperationException("Item Tree for extent-less object can't be created");
            }
            
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder(this);
                var foundForm = viewFinder.FindFormsFor(new FindFormQuery
                {
                    extentType = extent.GetExtentType(),
                    metaClass = (element as IElement)?.getMetaClass(),
                    FormType = FormType.TreeItemDetail
                }).FirstOrDefault();

                if (foundForm != null)
                {
                    return foundForm;
                }
            }

            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                var formCreator = CreateFormCreator();
                var createdForm = formCreator.CreateExtentFormForObject(element, extent, CreationMode.All);

                return createdForm;
            }

            // No Form
            return null;
        }

        /// <summary>
        /// Gets the list form for a property in the object
        /// </summary>
        /// <param name="element">Element to be queried</param>
        /// <param name="extent">Extent in which the element is located</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="metaClass">The metaclass for which the form is created</param>
        /// <param name="formDefinitionMode">The view definition mode</param>
        /// <returns></returns>
        public IElement GetListFormForExtentForPropertyInObject(
            IObject element,
            IExtent extent,
            string propertyName,
            IElement metaClass, 
            FormDefinitionMode formDefinitionMode)
        {
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder(this);
                var foundForm = viewFinder.FindFormsFor(new FindFormQuery
                {
                    extentType = extent.GetExtentType(),
                    metaClass = metaClass,
                    FormType = FormType.TreeItemDetail,
                    parentProperty = propertyName,
                    parentMetaClass = (element as IElement)?.getMetaClass()
                }).FirstOrDefault();

                if (foundForm != null)
                    return foundForm;
            }

            var formCreator = CreateFormCreator();
            var createdForm =
                formCreator.CreateListFormForPropertyInObject(metaClass, propertyName, CreationMode.All);

            return createdForm;
        }
        
        /// <summary>
        /// Creates a new instance of the form creator
        /// </summary>
        /// <returns>The created instance of the form creator</returns>
        public FormCreator.FormCreator CreateFormCreator()
            => new FormCreator.FormCreator(this, new DefaultClassifierHints(_workspaceLogic));
    }
}