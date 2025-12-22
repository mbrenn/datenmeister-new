using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Forms.FormFactory;

public record FieldFactoryParameter : FormFactoryParameterBase
{
    public IElement? Property { get; set; }
    
    public string PropertyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the property value being used to create the field value
    /// </summary>
    public object? PropertyValue { get; set; } = null;
    
    /// <summary>
    /// Gets or sets the value whether this value will be shown in a table.
    /// If it is shown in a table, a subtable will not be created
    /// This avoids a endless recursion leading to stackoverflow  
    /// </summary>
    public bool IsInTable { get; set; }
}

public interface IFieldFactory : IFormFactoryBase
{
    /// <summary>
    /// Creates a field element for a certain ProprtyType
    /// </summary>
    /// <param name="parameter">Parameters to create the field</param>
    /// <param name="context">Context being used for the creation</param>
    /// <param name="result">Here, we store the result</param>
    /// <returns></returns>
    void CreateField(
        FieldFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm result);
}