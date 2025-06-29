using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.ObjectForm;

public class EmptyObjectFormFactory : IObjectFormFactory
{
    public void CreateObjectForm(ObjectFormFactoryParameter parameter, FormCreationContext context, FormCreationResult result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__TableForm);

        result.IsManaged = true;
        
        result.AddToFormCreationProtocol(
            "[EmptyTableFormFactory] Empty object Table-Form created");
    }
}