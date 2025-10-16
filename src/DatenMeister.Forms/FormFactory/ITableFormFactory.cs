using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Forms.FormFactory;

public record TableFormFactoryParameter : FormFactoryParameterBase
{
    /// <summary>
    /// Gets or sets the name of the property being used to create the Table form
    /// </summary>
    public string? PropertyName { get; set; }
    
    public IReflectiveCollection? Collection { get; set; }
    
    public IElement? ParentMetaClass { get; set; }
    
    public string? ParentPropertyName { get; set; }
}

public interface ITableFormFactory : IFormFactoryBase
{
    void CreateTableForm(
        TableFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms result);
}