namespace DatenMeister.Core.TypeIndexAssembly.Model;

/// <summary>
/// Stores the properties of the class being evaluated 
/// </summary>
public class ClassModel
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}