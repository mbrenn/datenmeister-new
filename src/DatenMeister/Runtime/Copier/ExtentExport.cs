using System.IO;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.XMI.EMOF;

namespace DatenMeister.Runtime.Copier
{
    /// <summary>
    /// Performs an export of an extent to an xmi structure which then ca be stored to disc
    /// </summary>
    public class ExtentExport
    {
        /// <summary>
        /// Exports the given extent to a file and stores it permanently on the disc
        /// </summary>
        /// <param name="extent">Extent to be exported</param>
        /// <param name="filePath">Path of the file to be used</param>
        public static void ExportToFile(IExtent extent, string filePath)
        {
            var xmiExtent = ExportToXmiExtent(extent);

            var xmiProvider = (XmiProvider) xmiExtent.Provider;
            xmiProvider.Document.Save(filePath, SaveOptions.OmitDuplicateNamespaces);
        }

        /// <summary>
        /// Exports the given extent to an xml document
        /// </summary>
        /// <param name="extent">Extent to be exported</param>
        /// <returns>The document representing the export as a decoupled element</returns>
        public static XDocument ExportToXml(IExtent extent)
        {
            return ((XmiProvider) ExportToXmiExtent(extent).Provider).Document;
        }

        /// <summary>
        /// Exports the given extent to a text string containing the Xml. 
        /// </summary>
        /// <param name="extent">Extent to be exported</param>
        /// <returns>The string representing</returns>
        public static string ExportToString(IExtent extent)
        {
            using (var textWriter = new StringWriter())
            {
                ExportToXml(extent).Save(textWriter);

                return textWriter.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Exports the given extent to a new, decoupled Xmi Extent
        /// </summary>
        /// <param name="extent">Extent to be Exported</param>
        /// <returns>Exported Extent</returns>
        private static MofExtent ExportToXmiExtent(IExtent extent)
        {
            var xmiExtent = new MofExtent(
                new XmiProvider());

            // Copies the meta data
            var metaObject = ((MofExtent) extent).GetMetaObject();
            var targetMetaObject = xmiExtent.GetMetaObject();

            var copier = new ObjectCopier(new MofFactory(xmiExtent));
            copier.CopyProperties(metaObject, targetMetaObject);

            // Copies the data itself
            var extentCopier = new ExtentCopier(new MofFactory(xmiExtent));
            extentCopier.Copy(extent, xmiExtent);
            return xmiExtent;
        }
    }
}