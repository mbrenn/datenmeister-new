using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.Forms.TableForms;

/// <summary>
/// The factory which creates an empty collection form. 
/// </summary>
public class EmptyTableFormFactory(IWorkspaceLogic workspaceLogic) : FormFactoryBase, ITableFormFactory
{
    public void CreateTableForm(TableFormFactoryParameter parameter, FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        if (!result.Forms.Any())
        {
            var scopeStorage = workspaceLogic.ScopeStorage;
            if (scopeStorage != null)
            {
                var temporaryExtentLogic = new TemporaryExtentLogic(workspaceLogic, scopeStorage);
                result.Forms.Add(temporaryExtentLogic.CreateTemporaryElement(_Forms.TheOne.__TableForm));
                result.IsManaged = true;
            }
            else
            {
                result.Forms.Add(context.Global.Factory.create(_Forms.TheOne.__TableForm));
            }

            result.IsManaged = true;
            result.AddToFormCreationProtocol(
                "[EmptyTableFormFactory] Empty object Table-Form created");
        }
    }
}