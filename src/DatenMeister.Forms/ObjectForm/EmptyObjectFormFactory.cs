using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.ObjectForm;

public class EmptyObjectFormFactory : INewObjectFormFactory
{
    private static void CreateEmptyForm(NewFormCreationContext context, FormCreationResult result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__TableForm);

        result.IsManaged = true;
        
        result.AddToFormCreationProtocol(
            "[EmptyTableFormFactory] Empty object Table-Form created");
    }

    public void CreateObjectFormForItem(IObject element, NewFormCreationContext context, FormCreationResult result)
    {
        CreateEmptyForm(context, result);
    }

    public void CreateObjectFormForMetaClass(IElement? metaClass, NewFormCreationContext context, FormCreationResult result)
    {
        CreateEmptyForm(context, result);
    }
}