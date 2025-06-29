using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Forms.FormFactory;

public class TableFormFactoryParameter : FormFactoryParameterBase
{
    public IReflectiveCollection? Collection { get; set; }
}

public interface ITableFormFactory
{
    void CreateTableForm(
        TableFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResult result);
}