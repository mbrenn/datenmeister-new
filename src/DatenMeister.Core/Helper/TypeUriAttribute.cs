namespace DatenMeister.Core.Helper;

public enum TypeKind
{
    NotDefined,
    WrappedClass,
    ClassTree
}

/// <summary>
/// Defines the uri of the metaclass to which the source code generation of the corresponding
/// wrapper or type definition has been created.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TypeUriAttribute : Attribute
{
    /// <summary>
    /// Defines the relevant Uri
    /// </summary>
    public string Uri { get; set; } = string.Empty;

    public TypeKind TypeKind { get; set; } = TypeKind.NotDefined;
}