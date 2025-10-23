using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Forms.FormFinder;

/// <summary>
/// Implements the definition of forms by looking into the database and finding the pre-defined
/// forms via the FormFinder class.
/// </summary>
/// <param name="workspaceLogic">The workspacelogic being used to find the right forms</param>
public class FormFinderFactory(IWorkspaceLogic workspaceLogic) : 
    FormFactoryBase, ICollectionFormFactory, IObjectFormFactory, IRowFormFactory, ITableFormFactory
{
    private readonly FormMethods _formMethods = new(workspaceLogic);

    private FormFinder CreateFormFinder()
    {
        return new FormFinder(_formMethods);
    }

    private void Find(FormCreationResult result, FindFormQuery findQuery)
    {
        // Checks, if the form is already created, if yes, we do not need to recreate it
        if (result.IsMainContentCreated)
            return;

        var formFinder = CreateFormFinder();
        var foundForm = formFinder.FindFormsFor(findQuery).FirstOrDefault();

        if (foundForm != null)
        {
            if (result is FormCreationResultOneForm oneForm)
            {
                oneForm.Form = FormMethods.CloneForm(foundForm);
            }

            if (result is FormCreationResultMultipleForms multipleForms)
            {
                multipleForms.Forms.Add(FormMethods.CloneForm(foundForm));
            }

            result.IsMainContentCreated = true;
            result.IsManaged = true;
        }
    }

    /// <summary>
    /// Return all forms which fit to the given query
    /// </summary>
    /// <param name="findQuery">Query to be evaluated</param>
    /// <returns>Enumeration of all found forms</returns>
    private IEnumerable<IElement> FindAll(FindFormQuery findQuery)
    {
        var formFinder = CreateFormFinder();
        return formFinder.FindFormsFor(findQuery).Select(FormMethods.CloneForm);
    }

    public void CreateCollectionForm(
        CollectionFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm result)
    {
        if (!context.IsInExtensionCreationMode())
        {
            var findQuery =
                new FindFormQuery
                {
                    ViewModeId = context.ViewModeId,
                    FormType = _Forms.___FormType.Collection,
                    WorkspaceId = parameter.Extent?.GetWorkspace()?.id ?? string.Empty,
                    ExtentTypes = parameter.ExtentTypes ?? [],
                    ExtentUri = parameter.Extent?.contextURI() ?? string.Empty
                };

            Find(result, findQuery);
        }
        else
        {
            // We are in the extension mode, so we retrieve the found form and add it
            // to the found tabs to the original Form
            if (result.Form == null)
            {
                // We have no result to which an extension is required to be added
                return;
            }
            var findQuery =
                new FindFormQuery
                {
                    ViewModeId = context.ViewModeId,
                    FormType = _Forms.___FormType.CollectionExtension,
                    WorkspaceId = parameter.Extent?.GetWorkspace()?.id ?? string.Empty,
                    ExtentTypes = parameter.ExtentTypes ?? [],
                    ExtentUri = parameter.Extent?.contextURI() ?? string.Empty
                };
            
            var foundForms = FindAll(findQuery);
            foreach (var extensionForm in foundForms)
            {
                foreach (var tab in
                         extensionForm.getOrDefault<IReflectiveCollection>(_Forms._CollectionForm.tab)
                             .OfType<IElement>())
                {
                    result.Form.AddCollectionItem(_Forms._ObjectForm.tab, tab);
                }
            }
        }
    }

    public void CreateObjectForm(
        ObjectFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm result)
    {
        if (!context.IsInExtensionCreationMode())
        {
            var findQuery =
                new FindFormQuery
                {
                    ViewModeId = context.ViewModeId,
                    MetaClass = parameter.MetaClass,
                    FormType = _Forms.___FormType.Object,
                    WorkspaceId = parameter.Extent?.GetWorkspace()?.id ?? string.Empty,
                    ExtentTypes = parameter.ExtentTypes ?? [],
                    ExtentUri = parameter.Extent?.contextURI() ?? string.Empty
                };

            Find(result, findQuery);
        }
        else
        {
            // We are in the extension mode, so we retrieve the found form and add it
            // to the found tabs to the original Form
            if (result.Form == null)
            {
                // We have no result to which an extension is required to be added
                return;
            }
            var findQuery =
                new FindFormQuery
                {
                    ViewModeId = context.ViewModeId,
                    FormType = _Forms.___FormType.CollectionExtension,
                    WorkspaceId = parameter.Extent?.GetWorkspace()?.id ?? string.Empty,
                    ExtentTypes = parameter.ExtentTypes ?? [],
                    ExtentUri = parameter.Extent?.contextURI() ?? string.Empty
                };
            
            var foundForms = FindAll(findQuery);
            foreach (var extensionForm in foundForms)
            {
                foreach (var tab in
                         extensionForm.getOrDefault<IReflectiveCollection>(_Forms._ObjectForm.tab)
                             .OfType<IElement>())
                {
                    result.Form.AddCollectionItem(_Forms._ObjectForm.tab, tab);
                }
            }
        }
    }

    public void CreateRowForm(
        RowFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        if (!context.IsInExtensionCreationMode())
        {
            var findQuery =
                new FindFormQuery
                {
                    ViewModeId = context.ViewModeId,
                    MetaClass = parameter.MetaClass,
                    FormType = _Forms.___FormType.Row,
                    WorkspaceId = parameter.Extent?.GetWorkspace()?.id ?? string.Empty,
                    ExtentTypes = parameter.ExtentTypes ?? [],
                    ExtentUri = parameter.Extent?.contextURI() ?? string.Empty
                };

            Find(result, findQuery);
        }
        else
        {
            var form = result.Forms.FirstOrDefault();
            // We are in the extension mode, so we retrieve the found form and add it
            // to the found tabs to the original Form
            if (form == null)
            {
                // We have no result to which an extension is required to be added
                return;
            }
            
            var findQuery =
                new FindFormQuery
                {
                    ViewModeId = context.ViewModeId,
                    FormType = _Forms.___FormType.RowExtension,
                    WorkspaceId = parameter.Extent?.GetWorkspace()?.id ?? string.Empty,
                    ExtentTypes = parameter.ExtentTypes ?? [],
                    ExtentUri = parameter.Extent?.contextURI() ?? string.Empty
                };
            
            var foundForms = FindAll(findQuery);
            foreach (var extensionForm in foundForms)
            {
                foreach (var field in
                         extensionForm.getOrDefault<IReflectiveCollection>(_Forms._RowForm.field)
                             .OfType<IElement>())
                {
                    form.AddCollectionItem(_Forms._ObjectForm.tab, field);
                }
            }
        }
    }

    public void CreateTableForm(
        TableFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        if (!context.IsInExtensionCreationMode())
        {
            var findQuery =
                new FindFormQuery
                {
                    ViewModeId = context.ViewModeId,
                    MetaClass = parameter.MetaClass,
                    FormType = _Forms.___FormType.Table,
                    WorkspaceId = parameter.Extent?.GetWorkspace()?.id ?? string.Empty,
                    ExtentTypes = parameter.ExtentTypes ?? [],
                    ExtentUri = parameter.Extent?.contextURI() ?? string.Empty
                };

            Find(result, findQuery);
        }
        else
        {
            var form = result.Forms.FirstOrDefault();
            // We are in the extension mode, so we retrieve the found form and add it
            // to the found tabs to the original Form
            if (form == null)
            {
                // We have no result to which an extension is required to be added
                return;
            }
            
            var findQuery =
                new FindFormQuery
                {
                    ViewModeId = context.ViewModeId,
                    FormType = _Forms.___FormType.TableExtension,
                    WorkspaceId = parameter.Extent?.GetWorkspace()?.id ?? string.Empty,
                    ExtentTypes = parameter.ExtentTypes ?? [],
                    ExtentUri = parameter.Extent?.contextURI() ?? string.Empty
                };
            
            var foundForms = FindAll(findQuery);
            foreach (var extensionForm in foundForms)
            {
                foreach (var field in
                         extensionForm.getOrDefault<IReflectiveCollection>(_Forms._TableForm.field)
                             .OfType<IElement>())
                {
                    form.AddCollectionItem(_Forms._ObjectForm.tab, field);
                }
            }
        }
    }
}