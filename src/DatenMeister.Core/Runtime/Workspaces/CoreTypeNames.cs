namespace DatenMeister.Core.Runtime.Workspaces;

/// <summary>
/// Stores a set of IDs representing the internal type names
/// </summary>
public static class CoreTypeNames
{
    /// <summary>
    /// Stores the DateTime type
    /// </summary>
    public static readonly string DateTimeType = WorkspaceNames.UriExtentInternalTypes + "#PrimitiveTypes.DateTime";

    public static readonly string StringType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#String";

    public static readonly string IntegerType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#Integer";

    public static readonly string BooleanType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#Boolean";

    public static readonly string RealType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#Real";
}