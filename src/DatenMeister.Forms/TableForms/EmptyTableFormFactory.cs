using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.TableForms;

/// <summary>
/// The factory which creates an empty collection form. 
/// </summary>
public class EmptyTableFormFactory : INewTableFormFactory
{
    public void CreateTableForm(TableFormFactoryParameter parameter, NewFormCreationContext context,
        FormCreationResult result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__TableForm);
        result.IsManaged = true;
        
        result.AddToFormCreationProtocol(
            "[EmptyTableFormFactory] Empty object Table-Form created");
    }
}