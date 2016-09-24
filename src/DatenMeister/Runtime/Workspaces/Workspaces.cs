namespace DatenMeister.Runtime.Workspaces
{
    public class Workspaces
    {
        public Workspace Mof { get; set; }

        public Workspace Uml { get; set; }

        public Workspace Types { get; set; }

        public Workspace Data { get; set; }
        public const string NameManagement = "Management";
        public const string NameData = "Data";
        public const string NameTypes = "Types";
        public const string NameUml = "UML";
        public const string NameMof = "MOF";
        public static string UriUml = "datenmeister:///_internal/xmi/uml";
        public static string UriMof = "datenmeister:///_internal/xmi/mof";
        public static string UriPrimitiveTypes = "datenmeister:///_internal/xmi/prototypes";
        public static string UriInternalTypes = "datenmeister:///_internal/types/internal";
        public static string UriUserTypes = "datenmeister:///_internal/types/user";
    }
}