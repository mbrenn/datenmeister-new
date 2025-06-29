using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class FieldFactoryParameter : FormFactoryParameterBase
{
    public IElement? PropertyType { get; set; }
    
    public string PropertyName { get; set; } = string.Empty;
}

public interface INewFieldFactory
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
        NewFormCreationContext context,
        FormCreationResult result);
}