using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public interface INewTableFormFactory
{
    void CreateTableFormForCollection(
        IReflectiveCollection collection,
        NewFormCreationContext context,
        FormCreationResult result);
    
    void CreateTableFormForMetaclass(
        IElement metaClass,
        NewFormCreationContext context,
        FormCreationResult result);
}