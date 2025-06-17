using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.CollectionForms;

/// <summary>
/// The factory which creates an empty collection form. 
/// </summary>
public class EmptyCollectionFormFactory : INewCollectionFormFactory
{
    public void CreateCollectionFormForCollection(
        IReflectiveCollection collection,
        NewFormCreationContext context,
        FormCreationResult result)
    {
        CreateEmptyForm(context, result);
    }

    private static void CreateEmptyForm(NewFormCreationContext context, FormCreationResult result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__CollectionForm);
        result.IsManaged = true;
        
        result.AddToFormCreationProtocol(
            "[EmptyCollectionFormFactory] Empty object Collection-Form created");
    }

    public void CreateCollectionFormForMetaClass(IElement metaClass, NewFormCreationContext context, FormCreationResult result)
    {
        CreateEmptyForm(context, result);
    }
}