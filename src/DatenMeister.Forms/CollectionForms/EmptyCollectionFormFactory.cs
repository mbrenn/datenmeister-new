using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.CollectionForms;

/// <summary>
/// The factory which creates an empty collection form. 
/// </summary>
public class EmptyCollectionFormFactory : INewCollectionFormFactory
{
    public void CreateCollectionForm(CollectionFormFactoryParameter parameter, NewFormCreationContext context,
        FormCreationResult result)
    {
        result.AddToFormCreationProtocol(
            "[EmptyCollectionFormFactory] Empty object Collection-Form created");
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__CollectionForm);
        result.IsManaged = true;
    }
}