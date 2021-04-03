using System.IO;
using System.Reflection;

namespace DatenMeister.Core.XmiFiles
{
    public class XmiResources
    {
        public static Stream GetMofStream()
        {
            return typeof(XmiResources).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.MOF.xmi");        
        }

        public static Stream GetUmlStream()
        {
            return typeof(XmiResources).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.UML.xmi");        
        }

        public static Stream GetPrimitiveTypeStream()
        {
            return typeof(XmiResources).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.PrimitiveTypes.xmi");        
        }

        public static Stream GetDatenMeisterFormsStream()
        {
            return typeof(XmiResources).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.DatenMeister.Forms.xmi");        
        }

        public static Stream GetDatenMeisterTypesStream()
        {
            return typeof(XmiResources).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.DatemMeister.Types.xmi");        
        }
    }
}