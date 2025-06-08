using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms.FormFactory;

/// <summary>
/// Defines the base class for all the FormFactory Methods.
/// It also contains generic function which are usually required during the Form Creation
/// </summary>
/// <param name="workspaceLogic">Logic of the workspace</param>
/// <param name="scopeStorage">The storage of extensions</param>
public class FormFactoryBase(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    protected readonly FormMethods FormMethods = new(workspaceLogic, scopeStorage);
    protected readonly FormsState FormsState = scopeStorage.Get<FormsState>();
    protected readonly IWorkspaceLogic WorkspaceLogic = workspaceLogic;
    protected readonly IScopeStorage ScopeStorage = scopeStorage;

    /// <summary>
    /// Creates a new instance of the form reportCreator
    /// </summary>
    /// <returns>The created instance of the form reportCreator</returns>
    protected FormFinder.FormFinder CreateFormFinder()
    {
        return new FormFinder.FormFinder(FormMethods);
    }

    internal IFactory GetMofFactory(FormFactoryConfiguration configuration) 
        => new TableFormCreator(WorkspaceLogic, ScopeStorage).GetMofFactory(configuration);

    /// <summary>
    /// Cleans up the object form. 
    /// </summary>
    /// <param name="collectionForm">Collection Form to be cleaned up</param>
    /// <param name="cleanUpTabs">Flag, whether the tabs shall also be cleaned up.
    /// Per default, the creator of the tabs should call the corresponding CleanupRow and
    /// CleanupTableForm</param>
    public static void CleanupCollectionForm(IElement collectionForm, bool cleanUpTabs = false)
    {
        CleanupObjectForm(collectionForm, cleanUpTabs);
    }

    /// <summary>
    /// Cleans up the object form. 
    /// </summary>
    /// <param name="objectForm"></param>
    /// <param name="cleanUpTabs">Flag, whether the tabs shall also be cleaned up.
    /// Per default, the creator of the tabs should call the corresponding CleanupRow and
    /// CleanupTableForm</param>
    public static void CleanupObjectForm(IElement objectForm, bool cleanUpTabs = false)
    {
        if (!cleanUpTabs)
        {
            return;
        }
        
        var rowForms = FormMethods.GetRowForms(objectForm);
        foreach (var detailForm in rowForms)
        {
            CleanupRowForm(detailForm);
        }

        var tableForms = FormMethods.GetTableForms(objectForm);
        foreach (var tableForm in tableForms)
        {
            FormMethods.CleanupTableForm(tableForm);
        }
    }

    /// <summary>
    /// Cleans up the ist form by executing several default methods like, expanding the
    /// drop down values.
    /// </summary>
    /// <param name="rowForm">Detail form to be evaluated</param>
    public static void CleanupRowForm(IElement rowForm)
    {
        FormMethods.ExpandDropDownValuesOfValueReference(rowForm);
    }
    

    /// <summary>
    ///     Adds all found extension forms.
    ///     These extensions are required, so the user can also configure special tabs via the data
    /// </summary>
    /// <param name="form">
    ///     Gives the extent form that will be extended.
    ///     Must be of type ExtentForm.
    /// </param>
    /// <param name="query">Defines the query to be evaluated</param>
    /// <param name="callAlsoForSubforms">True, if the extension call shall also be
    /// performed for all the subforms</param>
    protected void AddExtensionFormsToObjectOrCollectionForm(
        IElement form,
        FindFormQuery query,
        bool callAlsoForSubforms = false)
    {
        if (query.FormType != _Forms.___FormType.Collection &&
            query.FormType != _Forms.___FormType.Object &&
            query.FormType != _Forms.___FormType.CollectionExtension &&
            query.FormType != _Forms.___FormType.ObjectExtension)
        {
            throw new InvalidOperationException("This method can only handle collection or object forms");
        }
            
        var viewFinder = CreateFormFinder();
        var foundForms = viewFinder.FindFormsFor(query);

        FormMethods.AddToFormCreationProtocol(form ,
            $"[FormFactory.AddExtensionFormsToObjectOrCollectionForm] Queried extension forms for {query.FormType}: {form.GetUri()}");

        var tabs = form.get<IReflectiveSequence>(_Forms._CollectionForm.tab);

        // Adds the extension tab
        foreach (var extensionForm in foundForms)
        {
            foreach (var extensionTab in extensionForm.get<IReflectiveSequence>(_Forms._CollectionForm.tab).OfType<IElement>())
            {
                tabs.add(ObjectCopier.CopyForTemporary(extensionTab));
            }
        }                

        if (callAlsoForSubforms)
        {
            foreach (var tableForm in FormMethods.GetTableForms(form))
            {
                AddExtensionFieldsToRowOrTableForm(tableForm,
                    query with { FormType = _Forms.___FormType.TableExtension });

                FormMethods.AddToFormCreationProtocol(form,
                    $"[FormFactory.AddExtensionFormsToObjectOrCollectionForm] Queried extension forms for TableExtension: {tableForm.GetUri()}");

            }

            foreach (var rowForm in FormMethods.GetRowForms(form))
            {
                AddExtensionFieldsToRowOrTableForm(rowForm,
                    query with { FormType = _Forms.___FormType.RowExtension });

                FormMethods.AddToFormCreationProtocol(form,
                    $"[FormFactory.AddExtensionFormsToObjectOrCollectionForm] Queried extension forms for RowExtension: {rowForm.GetUri()}");
            }
        }
        else
        {
            FormMethods.AddToFormCreationProtocol(form,
                $"[FormFactory.AddExtensionFormsToObjectOrCollectionForm] No call for form extensions for tabs.");
        }
    }

    /// <summary>
    ///     Adds all found extension forms.
    ///     These extensions are required, so the user can also configure special tabs via the data
    /// </summary>
    /// <param name="form">
    ///     Gives the extent form that will be extended.
    ///     Must be of type ExtentForm.
    /// </param>
    /// <param name="query">Defines the query to be evaluated</param>
    protected void AddExtensionFieldsToRowOrTableForm(
        IObject form,
        FindFormQuery query)
    {
        if (query.FormType != _Forms.___FormType.Row &&
            query.FormType != _Forms.___FormType.Table &&
            query.FormType != _Forms.___FormType.RowExtension &&
            query.FormType != _Forms.___FormType.TableExtension)
        {
            throw new InvalidOperationException("This method can only handle row or table forms");
        }

        var viewFinder = CreateFormFinder();
        var foundForms = viewFinder.FindFormsFor(query);

        var fields = form.get<IReflectiveSequence>(_Forms._RowForm.field);

        foreach (var foundForm in foundForms.OfType<IElement>())
        {
            var extensionFields = foundForm.get<IReflectiveCollection>(_Forms._RowForm.field);
            foreach (var field in extensionFields.OfType<IElement>())
            {
                fields.add(ObjectCopier.CopyForTemporary(field));
            }
        }
    }

    /// <summary>
    ///     Goes through the tabs the extent form and checks whether the listform required an autogeneration.
    ///     Each tab within the list form can require an autogeneration by setting the field 'autoGenerateFields'.
    /// </summary>
    /// <param name="element">The element to be used</param>
    /// <param name="objectOrCollectionForm">The form that shall be evaluated. It must
    /// be either an object or a collection form</param>
    protected void EvaluateTableFormsForAutogenerationByItem(IObject element, IElement objectOrCollectionForm)
    {
        var listForms = FormMethods.GetTableForms(objectOrCollectionForm);
        foreach (var tab in listForms)
        {
            var tabMetaClass = tab.getMetaClass();
            if (tabMetaClass == null ||
                !tabMetaClass.equals(_Forms.TheOne.__TableForm))
            {
                // Not a table tab
                continue;
            }

            var autoGenerate = tab.getOrDefault<bool>(_Forms._TableForm.autoGenerateFields);
            if (autoGenerate)
            {
                var formCreator = new TableFormCreator(WorkspaceLogic, ScopeStorage);
                var propertyName = tab.getOrDefault<string>(_Forms._TableForm.property);
                if (propertyName == null || string.IsNullOrEmpty(propertyName))
                {
                    FormMethods.AddToFormCreationProtocol(objectOrCollectionForm,
                        $"[FormFactory.EvaluateTableFormsForAutogenerationByItem] Auto Creation of fields by Element: {NamedElementMethods.GetName(tab)}");

                    formCreator.AddToTableFormByElements(
                        tab,
                        new PropertiesAsReflectiveCollection(element),
                        new FormFactoryConfiguration());
                }
                else
                {
                    FormMethods.AddToFormCreationProtocol(objectOrCollectionForm,
                        $"[FormFactory.EvaluateTableFormsForAutogenerationByItem] Auto Creation of fields by Element: {NamedElementMethods.GetName(tab)}");

                    var reflectiveSequence = element.getOrDefault<IReflectiveCollection>(propertyName);
                    if (reflectiveSequence != null)
                        formCreator.AddToTableFormByElements(
                            tab,
                            reflectiveSequence,
                            new FormFactoryConfiguration());
                }
            }
        }
    }

    /// <summary>
    ///     Calls all the plugins for the extent form
    /// </summary>
    /// <param name="configuration">Used configuration to call the plugins</param>
    /// <param name="formCreationContext">Form Creation context to be used</param>
    /// <param name="foundForm">The form being found</param>
    /// <param name="overrideParentMetaClassForTabs">If the tabs shall be queried with a specific
    /// parent metaclass and not by the metaclass of the formCreationContext, then this information
    /// can be stored here. This is used for table form from the extent in which the parent property
    /// is the extent and not the item to which is filtered</param>
    /// <returns>Element being called</returns>
    public void CallPluginsForCollectionOrObjectForm(
        FormFactoryConfiguration configuration, 
        FormCreationContext formCreationContext, 
        ref IElement foundForm,
        IElement? overrideParentMetaClassForTabs = null)
    {
        if (configuration?.AllowFormModifications != true)
        {
            // Nothing to do
            return;
        }

        FormsState.CallFormsModificationPlugins(
            configuration,
            formCreationContext,
            ref foundForm);
            
        var rowForms = FormMethods.GetRowForms(foundForm);
        foreach (var rowForm in rowForms)
        {
            var rowFormInstance = rowForm; // Get iterative

            FormMethods.ExpandDropDownValuesOfValueReference(rowFormInstance);

            FormsState.CallFormsModificationPlugins(
                configuration,
                formCreationContext with { FormType = _Forms.___FormType.Row },
                ref rowFormInstance);
        }

        var tableForms = FormMethods.GetTableForms(foundForm);
        foreach (var tableForm in tableForms)
        {
            var tableFormInstance = tableForm; // Get iterative

            FormMethods.ExpandDropDownValuesOfValueReference(tableFormInstance);

            FormsState.CallFormsModificationPlugins(
                configuration,
                formCreationContext with
                {
                    FormType = _Forms.___FormType.Table,
                    ParentPropertyName = tableFormInstance.getOrDefault<string>(_Forms._TableForm.property),
                    ParentMetaClass = formCreationContext.MetaClass,
                    MetaClass = tableFormInstance.getOrDefault<IElement>(_Forms._TableForm.metaClass),
                    IsReadOnly = configuration.IsReadOnly
                },
                ref tableFormInstance);
        }
    }
}