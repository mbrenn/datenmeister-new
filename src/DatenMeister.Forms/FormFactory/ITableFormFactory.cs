using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Forms.FormFactory;

public record TableFormFactoryParameter : FormFactoryParameterBase
{
    /// <summary>
    /// Gets or sets the name of the property being used to create the Table form
    /// </summary>
    public string PropertyName { get; set; }
    
    public IReflectiveCollection? Collection { get; set; }
}

public interface ITableFormFactory
{
    void CreateTableForm(
        TableFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms result);
}