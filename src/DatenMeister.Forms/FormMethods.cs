using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFinder;

namespace DatenMeister.Forms
{
    /// <summary>
    ///     Contains some helper methods for forms
    /// </summary>
    public class FormMethods
    {
        /// <summary>
        ///     Logger being used
        /// </summary>
        private static readonly ClassLogger Logger = new(typeof(FormMethods));
        
        /// <summary>
        /// Stores the scope storage
        /// </summary>
        private readonly IScopeStorage _scopeStorage;
        
        /// <summary>
        /// Stores the type of the extent containing the views
        /// </summary>
        public const string FormExtentType = "DatenMeister.Forms";
        
        /// <summary>
        ///     Stores the workspacelogic
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        public FormMethods(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        /// <summary>
        ///     Performs a verification of the form and returns false, if the form is not in a valid state
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <returns>true, if the form is valid</returns>
        public static bool ValidateForm(IObject form)
        {
            Debug.Assert(_DatenMeister._Forms._RowForm.field == _DatenMeister._Forms._TableForm.field);
            Debug.Assert(_DatenMeister._Forms._CollectionForm.tab == _DatenMeister._Forms._ObjectForm.tab);
            
            var fields = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field);
            if (fields != null)
                if (!ValidateFields(fields))
                    return false;

            var tabs = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab);
            if (tabs != null)
                foreach (var tab in tabs.OfType<IObject>())
                    if (!ValidateForm(tab))
                        return false;

            return true;
        }

        /// <summary>
        ///     Checks, if there is a duplicated name of the fields
        /// </summary>
        /// <param name="fields">Fields to be enumerated</param>
        /// <returns>true, if there are no duplications</returns>
        private static bool ValidateFields(IEnumerable fields)
        {
            // Creates a random GUID to establish a separate namespace for attached fields
            var randomGuid = Guid.NewGuid();
            
            // Now go through the hash set
            var set = new HashSet<string>();
            foreach (var field in fields.OfType<IObject>())
            {
                var preName = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
                var isAttached = field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isAttached);
                var name = isAttached ? randomGuid + preName : preName;

                if (set.Contains(name) && !string.IsNullOrEmpty(name))
                {
                    Logger.Warn($"Field '{name}' is included twice. Validation of form failed");
                    return false;
                }

                set.Add(name);
            }

            return true;
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
                            .GetAllDescendantsIncludingThemselves(
                                new[]
                                {_UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement})
                            .WhenMetaClassIsOneOf(
                                _DatenMeister.TheOne.Forms.__Form,
                                _DatenMeister.TheOne.Forms.__ObjectForm,
                                _DatenMeister.TheOne.Forms.__CollectionForm,
                                _DatenMeister.TheOne.Forms.__RowForm,
                                _DatenMeister.TheOne.Forms.__TableForm)),
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
        /// to be in the Management Workspace and be of type "DatenMeister.Forms".
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
                            .GetAllDescendantsIncludingThemselves(new[]
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
        public bool RemoveFormAssociationForObjectMetaClass(IElement metaClass, IExtent? viewExtent = null)
        {
            var result = false;
            viewExtent ??= GetUserFormExtent();

            foreach (var foundElement in viewExtent
                         .elements()
                         .GetAllDescendantsIncludingThemselves()
                         .WhenMetaClassIs(_DatenMeister.TheOne.Forms.__FormAssociation)
                         .WhenPropertyHasValue(_DatenMeister._Forms._FormAssociation.metaClass, metaClass)
                         .WhenPropertyHasValue(_DatenMeister._Forms._FormAssociation.formType,
                             _DatenMeister._Forms.___FormType.Row)
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
        ///     Checks if the given element already has a metaclass within the form
        /// </summary>
        /// <param name="form">Form to be checked</param>
        /// <returns>true, if the form already contains a metaclass form</returns>
        public static bool HasMetaClassFieldInForm(IObject form)
        {
            var formAndFields = _DatenMeister.TheOne.Forms;
            return form
                .get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field)
                .OfType<IElement>()
                .Any(x => x.getMetaClass()?.equals(formAndFields.__MetaClassElementFieldData) ?? false);
        }

        /// <summary>
        ///     Checks if the given element already has a metaclass within the form
        /// </summary>
        /// <param name="fields">Enumeration fo fields</param>
        /// <returns>true, if the form already contains a metaclass form</returns>
        public bool HasMetaClassFieldInForm(IEnumerable<object> fields)
        {
            var typesWorkspace = _workspaceLogic.GetTypesWorkspace();

            return fields
                .OfType<IElement>()
                .Any(x => x.getMetaClass()?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) ?? false);
        }

        /// <summary>
        ///     Looks for the given field in the form and returns it, if it is available
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="metaClass">If not null, the field will only be returned, if the metaclass is fitting</param>
        /// <returns>The found element or null, if not found</returns>
        public static IElement? GetField(IElement form, string fieldName, IElement? metaClass = null)
        {
            if (_DatenMeister._Forms._RowForm.field != _DatenMeister._Forms._TableForm.field)
                throw new InvalidOperationException(
                    "Something ugly happened here: _FormAndFields._DetailForm.tab != _FormAndFields._ListForm.tab." +
                    "Please check static type definition");

            var fields = form.get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field);
            return fields
                .WhenPropertyHasValue(_DatenMeister._Forms._FieldData.name, fieldName)
                .OfType<IElement>()
                .FirstOrDefault(
                    x => metaClass == null || x.metaclass?.equals(metaClass) == true);
        }

        /// <summary>
        ///     Gets the enumeration of the detail forms which are in embedded into the tabs
        /// </summary>
        /// <param name="form">Form to be checked</param>
        /// <returns>Enumeration of the detail forms</returns>
        public static IEnumerable<IElement> GetRowForms(IElement form)
        {
            if (form.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__RowForm) == true)
            {
                yield return form;
            }
            
            foreach (var tab in form.get<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab))
                if (tab is IElement asElement
                    && asElement.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__RowForm) == true)
                    yield return asElement;
        }

        /// <summary>
        /// Gets the enumeration of the table forms which are in embedded into the tabs
        /// If the given form is itself a tableform, it will also be returned
        /// </summary>
        /// <param name="form">Form to be checked</param>
        /// <returns>Enumeration of the detail forms</returns>
        public static IEnumerable<IElement> GetTableForms(IElement form)
        {
            if (form.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__TableForm) == true)
            {
                yield return form;
            }

            foreach (var tab in form.get<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab))
                if (tab is IElement asElement
                    && asElement.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__TableForm) == true)
                    yield return asElement;
        }

        /// <summary>
        /// Gets the fields for a certain propertyname
        /// </summary>
        /// <param name="form">The form, whose fields will be traversed. It must be a rowform or a tableform.
        /// If an object or collection Form is submitted, an exception will be thrown</param>
        /// <param name="propertyName">Name of the property to be evaluated</param>
        /// <returns>The found element or null, if not found</returns>
        public static IElement? GetFieldForProperty(IElement form, string propertyName)
        {
            if (form.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__ObjectForm) == true
                || form.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__CollectionForm) == true)
            {
                throw new InvalidOperationException(
                    "The form is of instance Object or Collection Form. It must be Table or RowForm");
            }

            foreach (var field 
                     in form.get<IReflectiveCollection>(_DatenMeister._Forms._RowForm.field))
            {
                if (field is IElement asFieldElement
                    && asFieldElement.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) == propertyName)
                {
                    return asFieldElement;
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets the list tab listing the items of the properties
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="propertyName">Name of the property to which the propertyname shall belong</param>
        /// <returns>The found element</returns>
        public static IElement? GetTableFormForPropertyName(IElement form, string propertyName)
        {
            if (_DatenMeister._Forms._CollectionForm.tab != _DatenMeister._Forms._ObjectForm.tab)
                throw new InvalidOperationException(
                    "Something ugly happened here: _FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab");

            var tabs = form.get<IReflectiveCollection>(_DatenMeister._Forms._CollectionForm.tab);

            foreach (var tab in tabs.OfType<IElement>())
                if (ClassifierMethods.IsSpecializedClassifierOf(tab.getMetaClass(),
                        _DatenMeister.TheOne.Forms.__TableForm))
                {
                    var property = tab.getOrDefault<string>(_DatenMeister._Forms._TableForm.property);
                    if (property == propertyName) return tab;
                }

            return null;
        }

        /// <summary>
        /// Gets all available view modes which are stored within the management workspace
        /// </summary>
        /// <returns>Enumeration of view modes. These elements are of type 'ViewMode'</returns>
        public IEnumerable<IObject> GetViewModes()
        {
            var managementWorkspace = _workspaceLogic.GetManagementWorkspace();
            return managementWorkspace.GetAllDescendentsOfType(_DatenMeister.TheOne.Forms.__ViewMode)
                .OfType<IObject>();
        }

        /// <summary>
        ///     Gets the default view mode for a certain object by querying the view mode instances as
        ///     given in the in the management workspace
        /// </summary>
        /// <param name="extent">Extent whose view mode is requested</param>
        /// <returns>Found element or null if not found</returns>
        public IElement? GetDefaultViewMode(IExtent? extent)
        {
            var managementWorkspace = _workspaceLogic.GetManagementWorkspace();

            var extentTypes = extent?.GetConfiguration()?.ExtentTypes;
            if (extentTypes != null)
                foreach (var extentType in extentTypes)
                {
                    var result = managementWorkspace
                        .GetAllDescendentsOfType(_DatenMeister.TheOne.Forms.__ViewMode)
                        .WhenPropertyHasValue(_DatenMeister._Forms._ViewMode.defaultExtentType, extentType)
                        .OfType<IElement>()
                        .FirstOrDefault();
                    if (result != null) return result;
                }

            return managementWorkspace
                .GetAllDescendentsOfType(_DatenMeister.TheOne.Forms.__ViewMode)
                .WhenPropertyHasValue(_DatenMeister._Forms._ViewMode.id, ViewModes.Default)
                .OfType<IElement>()
                .FirstOrDefault();
        }


        /// <summary>
        ///     Gets the extent form containing the subforms
        /// </summary>
        /// <param name="subForms">The forms to be added to the extent forms</param>
        /// <returns>The created extent</returns>
        public static IElement GetCollectionFormForSubforms(params IElement[] subForms)
        {
            return FormCreator.FormCreator.CreateCollectionFormFromTabs(null, subForms);
        }
        
        /// <summary>
        ///     Gets the extent form containing the subforms
        /// </summary>
        /// <param name="subForms">The forms to be added to the extent forms</param>
        /// <returns>The created extent</returns>
        public static IElement GetObjectFormForSubforms(params IElement[] subForms)
        {
            return FormCreator.FormCreator.CreateObjectFormFromTabs(null, subForms);
        }

        /// <summary>
        ///     Adds a certain text to the form creation protocol.
        ///     This protocol is used to allow more easy debugging of the form creation process.
        ///     Otherwise, the form is 'just' there and nobody knows how it was created.
        /// </summary>
        /// <param name="form">For to which the message shall be addedparam</param>
        /// <param name="message">Message it self that shall be added</param>
        public static void AddToFormCreationProtocol(IObject form, string message)
        {
            var currentMessage =
                form.getOrDefault<string>(_DatenMeister._Forms._Form.creationProtocol)
                ?? string.Empty;

            if (currentMessage != string.Empty)
                currentMessage += "\r\n" + message;
            else
                currentMessage = message;

            form.set(_DatenMeister._Forms._Form.creationProtocol, currentMessage);
        }

        /// <summary>
        /// Cleans duplicates of default new types
        /// </summary>
        /// <param name="form">For to be handled</param>
        public static void RemoveDuplicatingDefaultNewTypes(IObject form)
        {
            var defaultNewTypesForElements =
                form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._TableForm.defaultTypesForNewElements);
            if (defaultNewTypesForElements == null)
            {
                // Nothing to do, when no default types are set
                return;
            }

            var handled = new List<IObject>();

            foreach (var element in defaultNewTypesForElements.OfType<IObject>().ToList())
            {
                var metaClass = element.getOrDefault<IObject>(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass);
                if (metaClass == null) continue;

                if (handled.Any(x => x.@equals(metaClass)))
                {
                    defaultNewTypesForElements.remove(element);
                }
                else
                {
                    handled.Add(metaClass);
                }
            }
        }

        /// <summary>
        /// Adds a default type for a new element.
        /// If the element is already added, then it will be skipped.
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="defaultType">DefaultType to be added</param>
        public static void AddDefaultTypeForNewElement(IObject form, IObject defaultType)
        {
            var currentDefaultPackages =
                form.get<IReflectiveCollection>(_DatenMeister._Forms._TableForm.defaultTypesForNewElements);
            if (currentDefaultPackages.OfType<IElement>().Any(x =>
                    x.getOrDefault<IElement>(
                            _DatenMeister._Forms._DefaultTypeForNewElement.metaClass)
                        ?.@equals(defaultType) == true))
            {

                AddToFormCreationProtocol(
                    form,
                    $"[FormMethods.AddDefaultTypeForNewElement] Not added because default type is already existing: {NamedElementMethods.GetName(defaultType)}");
                // No adding, because it already exists
                return;
            }

            var defaultTypeInstance =
                new MofFactory(form).create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
            defaultTypeInstance.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, defaultType);
            defaultTypeInstance.set(_DatenMeister._Forms._DefaultTypeForNewElement.name,
                NamedElementMethods.GetName(defaultType));
            currentDefaultPackages.add(defaultTypeInstance);

            AddToFormCreationProtocol(
                form,
                $"[FormMethods.AddDefaultTypeForNewElement] Added defaulttype: {NamedElementMethods.GetName(defaultType)}");
        }

        /// <summary>
        ///     Expands the dropdown values of the the DropDownField.
        ///     The DropDownField supports a reference field which is not resolved by every Form Client.
        ///     So, the DropDownField can already be resolved on server side
        /// </summary>
        /// <param name="listOrDetailForm">The list form or the DetailForm being handled</param>
        public static void ExpandDropDownValuesOfValueReference(IElement listOrDetailForm)
        {
            var factory = new MofFactory(listOrDetailForm);
            var fields = listOrDetailForm.get<IReflectiveCollection>(_DatenMeister._Forms._TableForm.field);
            foreach (var field in fields.OfType<IElement>())
            {
                if (field.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__DropDownFieldData) != true) continue;

                var byEnumeration =
                    field.getOrDefault<IElement>(_DatenMeister._Forms._DropDownFieldData.valuesByEnumeration);
                var byValues =
                    field.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DropDownFieldData.values);
                if (byValues == null && byEnumeration != null)
                {
                    var enumeration = EnumerationMethods.GetEnumValues(byEnumeration);
                    foreach (var value in enumeration)
                    {
                        var element = factory.create(_DatenMeister.TheOne.Forms.__ValuePair);
                        element.set(_DatenMeister._Forms._ValuePair.name, value);
                        element.set(_DatenMeister._Forms._ValuePair.value, value);
                        field.AddCollectionItem(_DatenMeister._Forms._DropDownFieldData.values, element);
                    }

                    AddToFormCreationProtocol(listOrDetailForm,
                        $"[ExpandDropDownValuesOfValueReference] Expanded DropDown-Values for {NamedElementMethods.GetName(field)}");
                }
            }
        }

        /// <summary>
        /// Cleans up the ist form by executing several default methods like, expanding the
        /// drop down values are removing duplicates
        /// </summary>
        /// <param name="listForm">List form to be evaluated</param>
        public static void CleanupTableForm(IElement listForm)
        {
            AddDefaultTypeForListFormsMetaClass(listForm);
            ExpandDropDownValuesOfValueReference(listForm);            
            RemoveDuplicatingDefaultNewTypes(listForm);
        }

        private static void AddDefaultTypeForListFormsMetaClass(IObject listForm)
        {
            // Adds the default type corresponding to the list form
            var metaClass = listForm.getOrDefault<IElement>(_DatenMeister._Forms._TableForm.metaClass);
            if (metaClass != null)
            {
                AddDefaultTypeForNewElement(listForm, metaClass);
            }
        }

        /// <summary>
        /// Checks the type of the given element <code>asElement</code> and add its property-type
        /// to the default types
        /// </summary>
        /// <param name="foundForm">The listform to be modified</param>
        /// <param name="asElement">The element being evaluated</param>
        public static void AddDefaultTypesInListFormByElementsProperty(IElement foundForm, IElement asElement)
        {
            var listForms = GetTableForms(foundForm);
            foreach (var listForm in listForms)
            {
                var property = listForm.getOrDefault<string>(_DatenMeister._Forms._TableForm.property);
                var objectMetaClass = asElement.getMetaClass();

                if (property == null || objectMetaClass == null) continue;
                var propertyInstance = ClassifierMethods.GetPropertyOfClassifier(objectMetaClass, property);

                if (propertyInstance == null) continue;
                var propertyType = PropertyMethods.GetPropertyType(propertyInstance);

                if (propertyType == null) continue;
                AddDefaultTypeForNewElement(listForm, propertyType);
            }
        }

        /// <summary>
        /// Adds a view to the system
        /// </summary>
        /// <param name="type">Location Type to which the element shall be added</param>
        /// <param name="form">View to be added</param>
        public void Add(FormLocationType type, IObject form)
        {
            GetFormExtent(type).elements().add(form);
        }

        /// <summary>
        /// Takes the given form and evaluates the requested form type.
        /// If the requested FormType is a Object or Collection-Form, but the sent form is just a row or
        /// table form, then the form a new Object/Collection Form will be created and the provided form will
        /// be added as a child form to the new form.
        ///
        /// This allows some loose handling of the correct form type. 
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="formType">Type of the form which is requested</param>
        /// <returns>The converted form</returns>
        public static IElement ConvertFormToObjectOrCollectionForm(IElement form, _DatenMeister._Forms.___FormType formType)
        {
            var metaClass = form.metaclass;
            if (formType == _DatenMeister._Forms.___FormType.Collection
                && (metaClass?.equals(_DatenMeister.TheOne.Forms.__RowForm) == true ||
                    metaClass?.equals(_DatenMeister.TheOne.Forms.__TableForm) == true))
            {
                var converted = FormMethods.GetCollectionFormForSubforms(form);
                converted.set(_DatenMeister._Forms._Form.originalUri, form.GetUri());

                FormMethods.AddToFormCreationProtocol(
                    converted,
                    "Friendly conversion from row/table form to collection form:"
                    + NamedElementMethods.GetName(form));
                return converted;
            }

            if (formType == _DatenMeister._Forms.___FormType.Object
                && (metaClass?.equals(_DatenMeister.TheOne.Forms.__RowForm) == true ||
                    metaClass?.equals(_DatenMeister.TheOne.Forms.__TableForm) == true))
            {
                var converted = FormMethods.GetObjectFormForSubforms(form);
                converted.set(_DatenMeister._Forms._Form.originalUri, form.GetUri());

                FormMethods.AddToFormCreationProtocol(
                    converted,
                    "Friendly conversion from row/table form to object form:"
                    + NamedElementMethods.GetName(form));

                return converted;
            }

            form.set(_DatenMeister._Forms._Form.originalUri, form.GetUri());

            return form;
        }
    }
}