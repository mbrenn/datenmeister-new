namespace DatenMeister.Runtime.Workspaces
{
    public static class WorkspaceNames
    {
        public const string NameManagement = "Management";
        public const string NameData = "Data";
        public const string NameTypes = "Types";
        public const string NameUml = "UML";
        public const string NameMof = "MOF";
        public const string NameViews = "Views";
        public const string UriUmlExtent = "datenmeister:///_internal/xmi/uml";
        public const string UriMofExtent = "datenmeister:///_internal/xmi/mof";
        public const string UriPrimitiveTypesExtent = "datenmeister:///_internal/xmi/primitivetypes";


        public const string UriInternalTypesExtent = "datenmeister:///_internal/types/internal";
        public const string UriUserTypesExtent = "datenmeister:///_internal/types/user";


        /// <summary>
        /// Defines the uri of the view to the view extents
        /// </summary>
        public const string UriInternalFormExtent = "datenmeister:///management/forms/internal";

        /// <summary>
        /// Defines the uri of the user views
        /// </summary>
        public const string UriUserFormExtent = "datenmeister:///management/forms/user";


        /// <summary>
        /// Gets the uri of the extent which contains the workspaces
        /// </summary>
        public const string ExtentManagementExtentUri = "datenmeister:///_internal/workspaces/";


        public const string StandardUmlNamespace = "http://www.omg.org/spec/UML/20131001";
        public const string StandardUmlNamespaceAlternative = "http://www.omg.org/spec/UML/20131001/UML.xmi";


        public const string StandardPrimitiveTypeNamespace = "http://www.omg.org/spec/PrimitiveTypes/20131001";
        public const string StandardPrimitiveTypeNamespaceAlternative = "http://www.omg.org/spec/UML/20131001/PrimitiveTypes.xmi";
    }
}
 