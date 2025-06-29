using System.Web;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFinder;

namespace DatenMeister.Forms.Helper;

/// <summary>
///     Contains some helper methods for forms
/// </summary>
public class FormMethods(IWorkspaceLogic workspaceLogic)
{
    /// <summary>
    ///     Logger being used
    /// </summary>
    private static readonly ClassLogger Logger = new(typeof(FormMethods));

    /// <summary>
    /// Stores the type of the extent containing the views
    /// </summary>
    public const string FormExtentType = "DatenMeister.Forms";

    /// <summary>
    ///     Stores the workspacelogic
    /// </summary>
    private readonly IWorkspaceLogic _workspaceLogic = workspaceLogic;

    /// <summary>
    /// Gets the internal view extent being empty at each start-up
    /// </summary>
    /// <returns></returns>
    public IUriExtent GetInternalFormExtent()
    {
        if (_workspaceLogic.FindExtent(
                WorkspaceNames.WorkspaceManagement,
                WorkspaceNames.UriExtentInternalForm) is not IUriExtent foundExtent)
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
        if (_workspaceLogic.FindExtent(
                WorkspaceNames.WorkspaceManagement,
                WorkspaceNames.UriExtentInternalForm) is not IUriExtent foundExtent)
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
        if (_workspaceLogic.FindExtent(
                WorkspaceNames.WorkspaceManagement,
                WorkspaceNames.UriExtentUserForm) is not IUriExtent foundExtent)
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
        if (_workspaceLogic.FindExtent(
                WorkspaceNames.WorkspaceManagement,
                WorkspaceNames.UriExtentUserForm) is not IUriExtent foundExtent)
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
                                { _UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement })
                        .WhenMetaClassIsOneOf(
                            _Forms.TheOne.__Form,
                            _Forms.TheOne.__ObjectForm,
                            _Forms.TheOne.__CollectionForm,
                            _Forms.TheOne.__RowForm,
                            _Forms.TheOne.__TableForm)),
                true);
    }


    /// <summary>
    /// Adds a new form association between the form and the metaclass
    /// </summary>
    /// <param name="form">Form to be used to create the form association</param>
    /// <param name="metaClass">The metaclass being used for form association</param>
    /// <param name="formType">Type to be added</param>
    /// <returns></returns>
    public static IElement AddFormAssociationForMetaclass(
        IElement form,
        IElement metaClass,
        _Forms.___FormType formType)
    {
        var factory = new MofFactory(form);

        var formAssociation = factory.create(_Forms.TheOne.__FormAssociation);
        var name = NamedElementMethods.GetName(form);

        formAssociation.set(_Forms._FormAssociation.formType, formType);
        formAssociation.set(_Forms._FormAssociation.form, form);
        formAssociation.set(_Forms._FormAssociation.metaClass, metaClass);
        formAssociation.set(_Forms._FormAssociation.name, $"Association for {name}");

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
                            { _UML._CommonStructure._Namespace.member, _UML._Packages._Package.packagedElement })
                        .WhenMetaClassIsOneOf(_Forms.TheOne.__FormAssociation)),
            true);
    }

    /// <summary>
    /// Removes the view association from the database
    /// </summary>
    /// <param name="selectedExtentType">Extent type which is currently selected</param>
    /// <param name="viewExtent">The view extent which shall be looked through to remove the view association</param>
    /// <returns>true, if the association is removed</returns>
    public bool RemoveFormAssociationForExtentType(string selectedExtentType, IExtent? viewExtent = null)
    {
        var result = false;
        viewExtent ??= GetUserFormExtent();

        foreach (var foundElement in viewExtent
                     .elements()
                     .GetAllDescendantsIncludingThemselves()
                     .WhenMetaClassIs(_Forms.TheOne.__FormAssociation)
                     .WhenPropertyHasValue(_Forms._FormAssociation.extentType, selectedExtentType)
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
    /// <param name="metaClass">The metaclass which shall be used for the detailed form</param>
    /// <param name="viewExtent">The view extent which shall be looked through to remove the view association</param>
    public bool RemoveFormAssociationForObjectMetaClass(IElement metaClass, IExtent? viewExtent = null)
    {
        var result = false;
        viewExtent ??= GetUserFormExtent();

        foreach (var foundElement in viewExtent
                     .elements()
                     .GetAllDescendantsIncludingThemselves()
                     .WhenMetaClassIs(_Forms.TheOne.__FormAssociation)
                     .WhenPropertyHasValue(_Forms._FormAssociation.metaClass, metaClass)
                     .WhenPropertyHasValue(_Forms._FormAssociation.formType,
                         _Forms.___FormType.Row)
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
        var formAndFields = _Forms.TheOne;
        return form
            .get<IReflectiveCollection>(_Forms._RowForm.field)
            .OfType<IElement>()
            .Any(x => x.getMetaClass()?.equals(formAndFields.__MetaClassElementFieldData) ?? false);
    }

    /// <summary>
    ///     Checks if the given element already has a metaclass within the form
    /// </summary>
    /// <param name="fields">Enumeration fo fields</param>
    /// <returns>true, if the form already contains a metaclass form</returns>
    public static bool HasMetaClassFieldInForm(IEnumerable<object> fields)
    {
        return fields
            .OfType<IElement>()
            .Any(x => x.getMetaClass()?.equals(_Forms.TheOne.__MetaClassElementFieldData) ?? false);
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
        if (_Forms._RowForm.field != _Forms._TableForm.field)
            throw new InvalidOperationException(
                "Something ugly happened here: _FormAndFields._DetailForm.tab != _FormAndFields._ListForm.tab." +
                "Please check static type definition");

        var fields = form.get<IReflectiveCollection>(_Forms._RowForm.field);
        return fields
            .WhenPropertyHasValue(_Forms._FieldData.name, fieldName)
            .OfType<IElement>()
            .FirstOrDefault(x => metaClass == null || x.metaclass?.equals(metaClass) == true);
    }

    /// <summary>
    ///     Gets the enumeration of the detail forms which are in embedded into the tabs
    /// </summary>
    /// <param name="form">Form to be checked</param>
    /// <returns>Enumeration of the detail forms</returns>
    public static IEnumerable<IElement> GetRowForms(IElement form)
    {
        if (form.getMetaClass()?.@equals(_Forms.TheOne.__RowForm) == true)
        {
            yield return form;
        }

        foreach (var tab in form.get<IReflectiveCollection>(_Forms._CollectionForm.tab))
            if (tab is IElement asElement
                && asElement.getMetaClass()?.@equals(_Forms.TheOne.__RowForm) == true)
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
        if (form.getMetaClass()?.@equals(_Forms.TheOne.__TableForm) == true)
        {
            yield return form;
        }

        foreach (var tab in form.get<IReflectiveCollection>(_Forms._CollectionForm.tab))
            if (tab is IElement asElement
                && asElement.getMetaClass()?.@equals(_Forms.TheOne.__TableForm) == true)
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
        if (form.getMetaClass()?.@equals(_Forms.TheOne.__ObjectForm) == true
            || form.getMetaClass()?.@equals(_Forms.TheOne.__CollectionForm) == true)
        {
            throw new InvalidOperationException(
                "The form is of instance Object or Collection Form. It must be Table or RowForm");
        }

        foreach (var field
                 in form.get<IReflectiveCollection>(_Forms._RowForm.field))
        {
            if (field is IElement asFieldElement
                && asFieldElement.getOrDefault<string>(_Forms._FieldData.name) == propertyName)
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
        if (_Forms._CollectionForm.tab != _Forms._ObjectForm.tab)
            throw new InvalidOperationException(
                "Something ugly happened here: _FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab");

        var tabs = form.get<IReflectiveCollection>(_Forms._CollectionForm.tab);

        foreach (var tab in tabs.OfType<IElement>())
            if (ClassifierMethods.IsSpecializedClassifierOf(tab.getMetaClass(),
                    _Forms.TheOne.__TableForm))
            {
                var property = tab.getOrDefault<string>(_Forms._TableForm.property);
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
        return managementWorkspace.GetAllDescendentsOfType(_Forms.TheOne.__ViewMode)
            .OfType<IObject>();
    }

    /// <summary>
    ///     Gets the default view mode for a certain object by querying the view mode instances as
    ///     given in the management workspace
    /// </summary>
    /// <param name="extent">Extent whose view mode is requested</param>
    /// <returns>Found element or null if not found</returns>
    public IElement? GetDefaultViewMode(IExtent? extent)
    {
        var managementWorkspace = _workspaceLogic.GetManagementWorkspace();

        var extentTypes = extent?.GetConfiguration()?.ExtentTypes;
        if (extentTypes != null)
        {
            foreach (var extentType in extentTypes)
            {
                var result = managementWorkspace
                    .GetAllDescendentsOfType(_Forms.TheOne.__ViewMode)
                    .WhenPropertyHasValue(_Forms._ViewMode.defaultExtentType, extentType)
                    .OfType<IElement>()
                    .FirstOrDefault();
                if (result != null) return result;
            }
        }

        return managementWorkspace
            .GetAllDescendentsOfType(_Forms.TheOne.__ViewMode)
            .WhenPropertyHasValue(_Forms._ViewMode.id, ViewModes.Default)
            .OfType<IElement>()
            .FirstOrDefault();
    }


    /// <summary>
    ///     Gets the extent form containing the subforms
    /// </summary>
    /// <param name="subForms">The forms to be added to the extent forms</param>
    /// <returns>The created extent</returns>
    public static IElement GetObjectFormForSubforms(params IElement[] subForms)
    {
        return CreateObjectFormFromTabs(null, subForms);
    }


    /// <summary>
    ///     Creates an extent form containing the subforms
    /// </summary>
    /// <param name="factory">The factory being used</param>
    /// <param name="tabsAsForms">The Forms which are converted to an extent form</param>
    /// <returns>The created extent</returns>
    public static IElement CreateObjectFormFromTabs(IFactory? factory, params IElement[] tabsAsForms)
    {
        factory ??= new MofFactory(tabsAsForms.First());
        var result = factory.create(_Forms.TheOne.__ObjectForm);
        result.set(_Forms._RowForm.isAutoGenerated, true);
        result.set(_Forms._CollectionForm.tab, tabsAsForms);
        return result;
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
    public static IElement ConvertFormToObjectOrCollectionForm(IElement form, _Forms.___FormType formType)
    {
        var metaClass = form.metaclass;
        if (formType == _Forms.___FormType.Collection
            && (metaClass?.equals(_Forms.TheOne.__RowForm) == true ||
                metaClass?.equals(_Forms.TheOne.__TableForm) == true))
        {
            var converted = CollectionFormHelper.CreateCollectionFormFromTabs(form);
            converted.set(_Forms._Form.originalUri, form.GetUri());
            converted.set(
                _Forms._Form.originalWorkspace,
                form.GetExtentOf()?.GetWorkspace()?.id ?? string.Empty);

            FormCreationResult.AddToFormCreationProtocol(
                converted,
                "Friendly conversion from row/table form to collection form:"
                + NamedElementMethods.GetName(form));
            return converted;
        }

        if (formType == _Forms.___FormType.Object
            && (metaClass?.equals(_Forms.TheOne.__RowForm) == true ||
                metaClass?.equals(_Forms.TheOne.__TableForm) == true))
        {
            var converted = FormMethods.GetObjectFormForSubforms(form);
            converted.set(_Forms._Form.originalUri, form.GetUri());
            converted.set(
                _Forms._Form.originalWorkspace,
                form.GetExtentOf()?.GetWorkspace()?.id ?? string.Empty);

            FormCreationResult.AddToFormCreationProtocol(
                converted,
                "Friendly conversion from row/table form to object form:"
                + NamedElementMethods.GetName(form));

            return converted;
        }

        form.set(_Forms._Form.originalUri, form.GetUri());
        form.set(_Forms._Form.originalWorkspace, form.GetExtentOf()?.GetWorkspace()?.id ?? string.Empty);

        return form;
    }

    /// <summary>
    /// Clones the form and sets the orignal uri and workspace
    /// </summary>
    /// <param name="form">The form to be cloned</param>
    /// <returns>The cloned form including the original uri and workspalce</returns>
    public static IElement CloneForm(IElement form)
    {
        // Performs the cloning
        form = ObjectCopier.Copy(
            new MofFactory(form), form, new CopyOption());

        // Sets the original ori and workspace, so the client can reference to the original uri
        var originalUrl =
            form.isSet(_Forms._Form.originalUri)
                ? form.get<string>(_Forms._Form.originalUri)
                : form.GetUri();
        if (originalUrl != null)
        {
            form.set(_Forms._Form.originalUri, originalUrl);
        }

        var originalWorkspace =
            form.isSet(_Forms._Form.originalWorkspace)
                ? form.get<string>(_Forms._Form.originalWorkspace)
                : form.GetExtentOf()?.GetWorkspace()?.id;
        if (originalWorkspace != null)
        {
            form.set(_Forms._Form.originalWorkspace, originalWorkspace);
        }

        return form;
    }

    public static string GetUrlOfTableForm(IExtent extent, IElement tableForm)
    {
        // Set the data url of the table form
        var dataUrl = (extent as IUriExtent)?.contextURI() ?? string.Empty;

        // If form also contains a metaclass, then the metaclass needs to be added
        var tableFormMetaClass =
            tableForm.getOrDefault<IElement>(_Forms._TableForm.metaClass);
        var metaClassUri = tableFormMetaClass != null
            ? tableFormMetaClass.GetUri()
            : null;

        if (!string.IsNullOrEmpty(metaClassUri))
        {
            dataUrl += "?metaclass=" + HttpUtility.UrlEncode(metaClassUri);
        }

        return dataUrl;
    }

    /// <summary>
    ///     Checks whether a detail form is already within the element form.
    ///     If yes, then it is directly returned, otherwise a new detail form is created and added to the form
    /// </summary>
    /// <param name="collectionOrObjectForm">extentForm to be evaluated</param>
    public static IElement GetOrCreateRowFormIntoForm(IElement collectionOrObjectForm)
    {
        var tabs = collectionOrObjectForm.getOrDefault<IReflectiveCollection>(_Forms._CollectionForm.tab);

        foreach (var tab in tabs.OfType<IElement>())
        {
            if (ClassifierMethods.IsSpecializedClassifierOf(
                    tab.getMetaClass(),
                    _Forms.TheOne.__RowForm))
            {
                return tab;
            }
        }

        // Create new one
        var newTab = new MofFactory(collectionOrObjectForm).create(_Forms.TheOne.__RowForm);
        tabs.add(newTab);
        return newTab;
    }
}