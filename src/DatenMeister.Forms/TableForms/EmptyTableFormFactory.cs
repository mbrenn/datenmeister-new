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
    public void CreateCollectionFormForCollection(
        IReflectiveCollection collection,
        NewFormCreationContext context,
        FormCreationResult result)
    {
    }

    public void CreateTableFormForCollection(IReflectiveCollection collection, NewFormCreationContext context,
        FormCreationResult result)
    {
        CreateEmptyForm(context, result);
    }

    public void CreateTableFormForMetaclass(IElement metaClass, NewFormCreationContext context, FormCreationResult result)
    {
        CreateEmptyForm(context, result);
    }

    private static void CreateEmptyForm(NewFormCreationContext context, FormCreationResult result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__TableForm);
        result.IsManaged = true;
        
        result.AddToFormCreationProtocol(
            "[EmptyTableFormFactory] Empty object Table-Form created");
    }
}