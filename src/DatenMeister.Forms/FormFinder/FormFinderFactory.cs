using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Forms.FormFinder;

public class FormFinderFactory(IWorkspaceLogic workspaceLogic) : ICollectionFormFactory, IObjectFormFactory,
    IRowFormFactory, ITableFormFactory
{
    private readonly FormMethods _formMethods = new(workspaceLogic);

    private FormFinder CreateFormFinder()
    {
        return new FormFinder(_formMethods);
    }

    public void CreateCollectionForm(
        CollectionFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm result)
    {
        var findQuery =
            new FindFormQuery
            {
                viewModeId = context.ViewModeId,
                FormType = _Forms.___FormType.Collection
            };

        Find(result, findQuery);
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
                oneForm.Form = foundForm;
            }

            if (result is FormCreationResultMultipleForms multipleForms)
            {
                multipleForms.Forms = [foundForm];
            }
            
            result.IsMainContentCreated = true;
            result.IsManaged = true;
        }
    }

    public void CreateObjectForm(
        ObjectFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm result)
    {
        var findQuery =
            new FindFormQuery
            {
                viewModeId = context.ViewModeId,
                metaClass = parameter.MetaClass,
                FormType = _Forms.___FormType.Object
            };

        Find(result, findQuery);
    }

    public void CreateRowForm(
        RowFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        var findQuery =
            new FindFormQuery
            {
                viewModeId = context.ViewModeId,
                metaClass = parameter.MetaClass,
                FormType = _Forms.___FormType.Row
            };

        Find(result, findQuery);
    }

    public void CreateTableForm(
        TableFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        var findQuery =
            new FindFormQuery
            {
                viewModeId = context.ViewModeId,
                metaClass = parameter.MetaClass,
                FormType = _Forms.___FormType.Table
            };

        Find(result, findQuery);
    }
}