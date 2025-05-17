using System.Reflection;

namespace DatenMeister.Core.XmiFiles
{
    public class XmiResources
    {
        public static Stream GetMofStream()
        {
            var stream = typeof(XmiResources).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.MOF.xmi");
            if (stream == null)
                throw new InvalidOperationException("Stream is not found");
            return stream;
        }

        public static Stream GetUmlStream()
        {
            var stream = typeof(XmiResources).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.UML.xmi");
            if (stream == null)
                throw new InvalidOperationException("Stream is not found");
            return stream;
        }

        public static Stream GetPrimitiveTypeStream()
        {
            var stream = typeof(XmiResources).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.PrimitiveTypes.xmi");
            if (stream == null)
                throw new InvalidOperationException("Stream is not found");
            return stream;
        }

        public static Stream GetDatenMeisterFormsStream()
        {
            var stream = typeof(XmiResources).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.Forms.DatenMeister.xmi");
            if (stream == null)
                throw new InvalidOperationException("Stream is not found");
            return stream;
        }

        public static Stream GetDatenMeisterTypesStream()
        {
            var stream = typeof(XmiResources).GetTypeInfo()
                .Assembly.GetManifestResourceStream("DatenMeister.Core.XmiFiles.Types.DatenMeister.xmi");
            if (stream == null)
                throw new InvalidOperationException("Stream is not found");
            return stream;
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
            if (stream == null)
            {
                throw new InvalidOperationException("No stream was returned.");
            }

            using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Stream is empty"));
            return reader.ReadToEnd();
        }
    }
}