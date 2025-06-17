using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.RowForm;

public class EmptyRowFormFactory : INewRowFormFactory
{
    public void CreateRowFormForItem(IObject element, NewFormCreationContext context, FormCreationResult result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__RowForm);
        result.IsManaged = true;
    }

    public void CreateRowFormForMetaClass(IElement metaClass, NewFormCreationContext context, FormCreationResult result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__RowForm);
        result.IsManaged = true;
    }
}