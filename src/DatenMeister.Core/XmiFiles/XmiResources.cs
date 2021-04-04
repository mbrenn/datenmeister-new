using System;
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

        /// <summary>
        /// Converts a stream to a string
        /// </summary>
        /// <param name="streamFactory">Factory method to get the stream. The created stream will be disposed after
        /// the function call</param>
        /// <returns>The string by the stream</returns>
        public static string GetStringFromStream(Func<Stream> streamFactory)
        {
            using var stream = streamFactory();
            
            using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Stream is empty"));
            return reader.ReadToEnd();
        }
    }
}