using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.ObjectForm;

public class EmptyObjectFormFactory : FormFactoryBase, IObjectFormFactory
{
    public void CreateObjectForm(ObjectFormFactoryParameter parameter, FormCreationContext context, FormCreationResultOneForm result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__ObjectForm);

        result.IsManaged = true;
        
        result.AddToFormCreationProtocol(
            "[EmptyTableFormFactory] Empty object Table-Form created");
    }
}