using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Forms.FormFactory;

public class FieldFactoryParameter
{
    public IElement? PropertyType { get; set; }
    
    public string PropertyName { get; set; } = string.Empty;
}
public interface INewFieldFactory
{
    /// <summary>
    /// Creates a field element for a certain ProprtyType
    /// </summary>
    /// <param name="umlPropertyType">Type of the property of the element</param>
    /// <param name="context">Context being used for the creation</param>
    /// <param name="result">Here, we store the result</param>
    /// <returns></returns>
    void CreateFieldForProperty(
        IObject? umlPropertyType,
        NewFormCreationContext context,
        FormCreationResult result);
}