using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.Forms.ObjectForm;

public class EmptyObjectFormFactory(IWorkspaceLogic workspaceLogic) : FormFactoryBase, IObjectFormFactory
{
    public void CreateObjectForm(ObjectFormFactoryParameter parameter, FormCreationContext context, FormCreationResultOneForm result)
    {
        if (result.Form == null)
        {
            var scopeStorage = workspaceLogic.ScopeStorage;
            if (scopeStorage != null)
            {
                var temporaryExtentLogic = new TemporaryExtentLogic(workspaceLogic, scopeStorage);
                result.Form = temporaryExtentLogic.CreateTemporaryElement(_Forms.TheOne.__ObjectForm);
                
            }
            else
            {
                result.Form = context.Global.Factory.create(_Forms.TheOne.__ObjectForm);
            }
            
            result.IsManaged = true;
            result.AddToFormCreationProtocol(
                "[EmptyTableFormFactory] Empty object Table-Form created");
        }
    }
}