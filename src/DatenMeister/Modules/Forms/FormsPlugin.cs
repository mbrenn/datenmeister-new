#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Models.EMOF;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.Forms.FormModifications;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Extents.Configuration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Forms
{
    /// <summary>
    /// Defines the access to the view logic and abstracts the access to the view extent
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FormsPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(FormsPlugin));

        /// <summary>
        /// Stores the type of the extent containing the views
        /// </summary>
        public const string FormExtentType = "DatenMeister.Forms";

        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentCreator _extentCreator;
        private readonly IntegrationSettings _integrationSettings;
        private readonly ExtentSettings _extentSettings;

        /// <summary>
        /// Defines the state for the form plugin
        /// </summary>
        private FormsPluginState _formPluginState;

        /// <summary>
        /// Gets the workspace logic of the view logic
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic => _workspaceLogic;

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
            _integrationSettings = scopeStorage.Get<IntegrationSettings>();
            _extentSettings = scopeStorage.Get<ExtentSettings>();
            _formPluginState = scopeStorage.Get<FormsPluginState>();
        }

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
                        new MofUriExtent(new InMemoryProvider(), WorkspaceNames.UriExtentInternalForm);
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
                        .AddDefaultTypePackages(new[]
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
            if (!(_workspaceLogic.FindExtent(WorkspaceNames.UriExtentInternalForm) is IUriExtent foundExtent))
            {
                throw new InvalidOperationException("The form extent is not found in the management");
            }

            return foundExtent;
        }

        /// <summary>
        /// Gets the extent of the user being stored on permanent storage
        /// </summary>
        /// <returns></returns>
        public IUriExtent GetUserFormExtent()
        {
            if (!(_workspaceLogic.FindExtent(WorkspaceNames.UriExtentUserForm) is IUriExtent foundExtent))
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
        public IElement AddFormAssociationForMetaclass(IElement form, IElement metaClass, _DatenMeister._Forms.___FormType formType)
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
                .WhenPropertyHasValue(_DatenMeister._Forms._FormAssociation.formType, _DatenMeister._Forms.___FormType.Detail)
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

        public IElement? GetDetailForm(IObject? element, IExtent? extent, FormDefinitionMode formDefinitionMode)
        {
            IElement? foundForm = null;
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                // Tries to find the form
                var viewFinder = new FormFinder.FormFinder(this);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        metaClass = (element as IElement)?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.Detail,
                        extentType = extent == null ? string.Empty : extent.GetConfiguration().ExtentType
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetDetailForm: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                // Ok, we have not found the form. So create one
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateDetailForm(element);
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext()
                    {
                        DefinitionMode = formDefinitionMode,
                        MetaClass = (element as IElement)?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.Detail,
                        ExtentType = extent?.GetConfiguration().ExtentType ?? string.Empty,
                        DetailElement = element
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        public IElement? GetExtentForm(IUriExtent extent, FormDefinitionMode formDefinitionMode, string viewModeId = "")
        {
            IElement? foundForm = null;
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder.FormFinder(this);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = extent.GetConfiguration().ExtentType,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtent,
                        viewModeId = viewModeId
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetExtentForm: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateExtentForm(
                    extent,
                    CreationMode.All | CreationMode.ForListForms);
            }
            
            // Adds the extension forms to the found extent
            if (foundForm != null)
            {
                AddExtensionFormsToExtentForm(
                    foundForm,
                    new FindFormQuery
                    {
                        extentType = extent.GetConfiguration().ExtentType,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtentExtension,
                        viewModeId = viewModeId
                    });
            }
            
            // 
            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                EvaluateListFormsForAutogenerationByReflectiveCollection(extent.elements(), foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        DefinitionMode = formDefinitionMode,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtent,
                        ExtentType = extent.GetConfiguration().ExtentType
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        /// <summary>
        /// Goes through the tabs the extent form and checks whether the listform required an autogeneration
        /// </summary>
        /// <param name="reflectiveCollection">The reflective collection to be used</param>
        /// <param name="foundForm">The element that has been found</param>
        private void EvaluateListFormsForAutogenerationByReflectiveCollection(IReflectiveCollection reflectiveCollection, IElement foundForm)
        {
            // Go through the list forms and check if we need to auto-populate
            foreach (var tab in
                foundForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab).OfType<IElement>())
            {
                var tabMetaClass = tab.getMetaClass();
                if (tabMetaClass == null ||
                    !tabMetaClass.@equals(_DatenMeister.TheOne.Forms.__ListForm))
                {
                    // Not a list tab
                    continue;
                }

                var autoGenerate = tab.getOrDefault<bool>(_DatenMeister._Forms._ListForm.autoGenerateFields);
                if (autoGenerate)
                {
                    var formCreator = CreateFormCreator();
                    formCreator.AddToListFormByElements(tab, reflectiveCollection, CreationMode.All);
                }
            }
        }

        /// <summary>
        /// Goes through the tabs the extent form and checks whether the listform required an autogeneration
        /// </summary>
        /// <param name="element">The element to be used</param>
        /// <param name="foundForm">The element that has been found</param>
        private void EvaluateListFormsForAutogenerationByElement(IElement element, IElement foundForm)
        {
            // Go through the list forms and check if we need to auto-populate
            foreach (var tab in
                foundForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab).OfType<IElement>())
            {
                var tabMetaClass = tab.getMetaClass();
                if (tabMetaClass == null ||
                    !tabMetaClass.@equals(_DatenMeister.TheOne.Forms.__ListForm))
                {
                    // Not a list tab
                    continue;
                }

                var autoGenerate = tab.getOrDefault<bool>(_DatenMeister._Forms._ListForm.autoGenerateFields);
                if (autoGenerate)
                {
                    var formCreator = CreateFormCreator();
                    var propertyName = tab.getOrDefault<string>(_DatenMeister._Forms._ListForm.property);
                    if (propertyName == null || string.IsNullOrEmpty(propertyName))
                    {
                        formCreator.AddToListFormByElements(
                            tab,
                            new PropertiesAsReflectiveCollection(element), 
                            CreationMode.All);
                    }
                    else
                    {
                        var reflectiveSequence = element.getOrDefault<IReflectiveCollection>(propertyName);
                        if (reflectiveSequence != null)
                        {
                            formCreator.AddToListFormByElements(
                                tab,
                                reflectiveSequence,
                                CreationMode.All);
                        }
                    }
                }
            }
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
        /// the form reportCreator thinks about creating a list form for the extent, it will query this
        /// method
        /// </summary>
        /// <param name="extent">Extent for which the list is created</param>
        /// <param name="metaClass">Metaclass of the items that are listed now</param>
        /// <param name="formDefinitionMode">The view definition mode</param>
        /// <returns>The found or created list form</returns>
        public IElement? GetListFormForExtentsItem(
            IExtent extent,
            IElement metaClass,
            FormDefinitionMode formDefinitionMode)
        {
            IElement? foundForm = null;
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder.FormFinder(this);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = extent.GetConfiguration().ExtentType,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtent,
                        metaClass = metaClass
                    }).FirstOrDefault();

                
                if (foundForm != null)
                {
                    Logger.Info("GetListFormForExtent: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                // Ok, now perform the creation...
                var formCreator = CreateFormCreator();
                foundForm =  formCreator.CreateListFormForMetaClass(metaClass, CreationMode.All);
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        DefinitionMode = formDefinitionMode,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemExtent,
                        MetaClass = metaClass
                    },
                    ref foundForm);
            }

            return foundForm;
        }
        
        /// <summary>
        /// Gets the list form for an elements property to be shown in sub item view or other views
        /// </summary>
        /// <param name="parentElement">Element whose property is enumerated</param>
        /// <param name="parentProperty">Name of the property to be enumeration</param>
        /// <param name="propertyType">Type of the property</param>
        /// <param name="formDefinitionMode">The view definition mode</param>
        /// <returns>The list form for the list</returns>
        public IElement? GetListFormForElementsProperty(
            IObject? parentElement,
            string parentProperty,
            IElement? propertyType,
            FormDefinitionMode formDefinitionMode = FormDefinitionMode.Default)
        {
            IElement? foundForm = null;

            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder.FormFinder(this);
                foundForm = viewFinder.FindFormsFor(
                    new FindFormQuery
                    {
                        extentType = (parentElement as IHasExtent)?.Extent?.GetConfiguration().ExtentType ?? string.Empty,
                        parentMetaClass = (parentElement as IElement)?.metaclass,
                        metaClass = propertyType,
                        FormType = _DatenMeister._Forms.___FormType.ObjectList,
                        parentProperty = parentProperty
                    }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetListFormForElementsProperty: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateListFormForElements(
                    parentElement.get<IReflectiveCollection>(parentProperty),
                    CreationMode.All | CreationMode.OnlyCommonProperties);
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        DefinitionMode = formDefinitionMode,
                        FormType = _DatenMeister._Forms.___FormType.ObjectList,
                        MetaClass = (parentElement as IElement)?.metaclass,
                        ParentPropertyName = parentProperty,
                        DetailElement = parentElement
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        public IElement? GetExtentForm(IReflectiveCollection collection, FormDefinitionMode formDefinitionMode)
        {
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                // Try to find the view, but very improbable
            }

            var formCreator = CreateFormCreator();
            return formCreator.CreateExtentForm(collection, CreationMode.All, new ExtentFormConfiguration());
        }

        /// <summary>
        /// Gets the list view next to the item explorer control
        /// </summary>
        /// <param name="element">Element for which the list view will be created</param>
        /// <param name="formDefinitionMode">Defines the method how to retrieve the form</param>
        /// <param name="viewModeId">Defines the id of the view mode in which the user currently is</param>
        /// <returns>Found extent form</returns>
        public IElement? GetItemTreeFormForObject(IObject element, FormDefinitionMode formDefinitionMode, string viewModeId)
        {
            IElement? foundForm = null;

            var extent = (element as IHasExtent)?.Extent;
            if (extent == null)
            {
                throw new InvalidOperationException("Item Tree for extent-less object can't be created");
            }

            string? packageViewMode = null;
            // Checks if the current item is a package and if the viewmode
            if (DefaultClassifierHints.IsPackageLike(element))
            {
                packageViewMode = element.getOrDefault<string>(_DatenMeister._CommonTypes._Default._Package.defaultViewMode);
            }
            
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder.FormFinder(this);
                foundForm = viewFinder.FindFormsFor(new FindFormQuery
                {
                    extentType = extent.GetConfiguration().ExtentType,
                    metaClass = (element as IElement)?.getMetaClass(),
                    FormType = _DatenMeister._Forms.___FormType.TreeItemDetail,
                    viewModeId = packageViewMode ?? viewModeId
                }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetItemTreeFormForObject: Found form: " + NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                var formCreator = CreateFormCreator();
                foundForm = formCreator.CreateExtentFormForObject(
                    element, 
                    extent, 
                    CreationMode.All | CreationMode.OnlyCommonProperties);
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                // Adds the extension forms to the found extent
                AddExtensionFormsToExtentForm(
                    foundForm,
                    new FindFormQuery
                    {
                        extentType = extent.GetConfiguration().ExtentType,
                        metaClass = (element as IElement)?.getMetaClass(),
                        FormType = _DatenMeister._Forms.___FormType.TreeItemDetailExtension,
                        viewModeId = viewModeId
                    });

                if (element is IElement asElement)
                {
                    EvaluateListFormsForAutogenerationByElement(asElement, foundForm);
                }

                CallFormsModificationPlugins(new FormCreationContext
                    {
                        DefinitionMode = formDefinitionMode,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemDetail,
                        MetaClass = (element as IElement)?.metaclass,
                        ViewMode = viewModeId,
                        DetailElement = element
                    },
                    ref foundForm);
            }

            // No Form
            return foundForm;
        }

        private static IElement CloneForm(IElement foundForm)
        {
            var originalUrl = foundForm.GetUri();
            foundForm = ObjectCopier.Copy(InMemoryObject.TemporaryFactory, foundForm, new CopyOption());
            if (originalUrl != null)
            {
                foundForm.set(_DatenMeister._Forms._Form.originalUri, originalUrl);
            }

            return foundForm;
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
        public IElement? GetListFormForExtentForPropertyInObject(
            IObject element,
            IExtent extent,
            string propertyName,
            IElement metaClass,
            FormDefinitionMode formDefinitionMode)
        {
            IElement? foundForm = null;

            var parentMetaClass = (element as IElement)?.getMetaClass();
            if (formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormFinder))
            {
                var viewFinder = new FormFinder.FormFinder(this);
                foundForm = viewFinder.FindFormsFor(new FindFormQuery
                {
                    extentType = extent.GetConfiguration().ExtentType,
                    metaClass = metaClass,
                    FormType = _DatenMeister._Forms.___FormType.TreeItemDetail,
                    parentProperty = propertyName,
                    parentMetaClass = parentMetaClass
                }).FirstOrDefault();

                if (foundForm != null)
                {
                    Logger.Info("GetListFormForExtentForPropertyInObject: Found form: " +
                                NamedElementMethods.GetFullName(foundForm));
                }
            }

            if (foundForm == null && formDefinitionMode.HasFlag(FormDefinitionMode.ViaFormCreator))
            {
                var formCreator = CreateFormCreator();
                foundForm =
                    formCreator.CreateListFormForPropertyInObject(metaClass, propertyName, CreationMode.All);
            }

            if (foundForm != null)
            {
                foundForm = CloneForm(foundForm);
                
                CallFormsModificationPlugins(new FormCreationContext
                    {
                        DefinitionMode = formDefinitionMode,
                        FormType = _DatenMeister._Forms.___FormType.TreeItemDetail,
                        MetaClass = metaClass,
                        ParentPropertyName = propertyName,
                        ParentMetaClass = parentMetaClass,
                        DetailElement = element
                    },
                    ref foundForm);
            }

            return foundForm;
        }

        /// <summary>
        /// Calls all the form modification plugins, if allowed. 
        /// </summary>
        /// <param name="formCreationContext">The creation context used by the plugins</param>
        /// <param name="form">The form that is evaluated</param>
        public void CallFormsModificationPlugins(FormCreationContext formCreationContext, ref IElement form)
        {
            if (form == null || formCreationContext.DefinitionMode.HasFlagFast(FormDefinitionMode.NoFormModifications))
            {
                return; // Nothing to do
            }

            foreach (var plugin in _formPluginState.FormModificationPlugins)
            {
                plugin.ModifyForm(formCreationContext, form);
            }
        }

        /// <summary>
        /// Adds all found extension forms to the  
        /// </summary>
        /// <param name="form">Gives the extent form that will be extended.
        /// Must be of type ExtentForm.</param>
        /// <param name="query">Defines the query to be evaluated</param>
        private void AddExtensionFormsToExtentForm(
            IElement form,
            FindFormQuery query)
        {
            var viewFinder = new FormFinder.FormFinder(this);
            var foundForms = viewFinder.FindFormsFor(query);

            var tabs = form.get<IReflectiveSequence>(_DatenMeister._Forms._ExtentForm.tab);
            foreach (var listForm in foundForms)
            {
                tabs.add(listForm);
            }
        }
        
        /// <summary>
        /// Creates a new instance of the form reportCreator
        /// </summary>
        /// <returns>The created instance of the form reportCreator</returns>
        public FormCreator.FormCreator CreateFormCreator()
            =>  FormCreator.FormCreator.Create(WorkspaceLogic, this, _extentSettings);
    }
}