using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.Forms.RowForm;

public class EmptyRowFormFactory(IWorkspaceLogic workspaceLogic) : FormFactoryBase, IRowFormFactory
{
    public void CreateRowForm(RowFormFactoryParameter parameter,
        FormCreationContext context, 
        FormCreationResultMultipleForms result)
    {
        if (!result.Forms.Any())
        {
            result.Forms.Add(context.Global.FactoryForForms.create(_Forms.TheOne.__RowForm));
            result.IsManaged = true;
            result.AddToFormCreationProtocol(
                "[EmptyRowFormFactory] Empty object Row-Form created");
        }
    }
}