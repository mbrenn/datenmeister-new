namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Stores a set of IDs representing the internal type names
    /// </summary>
    public static class TypeNames
    {
        /// <summary>
        /// Stores the DateTime type
        /// </summary>
        public static string DateTimeType = WorkspaceNames.UriInternalTypesExtent + "#PrimitiveTypes.DateTime";

        public static string StringType = WorkspaceNames.StandardPrimitiveTypeNamespace + "#String";
    }
}