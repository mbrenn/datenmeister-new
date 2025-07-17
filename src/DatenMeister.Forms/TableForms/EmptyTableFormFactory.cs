using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.TableForms;

/// <summary>
/// The factory which creates an empty collection form. 
/// </summary>
public class EmptyTableFormFactory : ITableFormFactory
{
    public void CreateTableForm(TableFormFactoryParameter parameter, FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        if (!result.Forms.Any())
        {
            result.Forms.Add(context.Global.Factory.create(_Forms.TheOne.__TableForm));
        }

        result.IsManaged = true;
        
        result.AddToFormCreationProtocol(
            "[EmptyTableFormFactory] Empty object Table-Form created");
    }
}