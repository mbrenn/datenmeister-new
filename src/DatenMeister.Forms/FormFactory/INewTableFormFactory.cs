using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class TableFormFactoryParameter
{
    public IReflectiveCollection? Collection { get; set; }
    
    public IElement? MetaClass { get; set; }
}

public interface INewTableFormFactory
{
    void CreateTableForm(
        TableFormFactoryParameter parameter,
        NewFormCreationContext context,
        FormCreationResult result);
}