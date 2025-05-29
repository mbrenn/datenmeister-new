namespace DatenMeister.Core.Runtime.Workspaces;

/// <summary>
/// Stores a set of IDs representing the internal type names
/// </summary>
public static class CoreTypeNames
{
    /// <summary>
    /// Stores the DateTime type
    /// </summary>
    public static string DateTimeType = WorkspaceNames.UriExtentInternalTypes + "#PrimitiveTypes.DateTime";

    public static string StringType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#String";

    public static string IntegerType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#Integer";

    public static string BooleanType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#Boolean";

    public static string RealType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#Real";
}