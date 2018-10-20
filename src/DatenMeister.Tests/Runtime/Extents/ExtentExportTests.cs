using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.Copier;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents
{
    public class ExtentExportTests
    {
        public static IUriExtent GetExampleExtent()
        {
            var xmlDocument = "<test><abc><def x=\"DatenMeister Yeahaa\" /></abc></test>";
            var document = XDocument.Parse(xmlDocument);

            return new MofUriExtent(
                new XmiProvider(document), "dm:///test");
        }

        [Test]
        public void TestExport()
        {
            var exportedText = ExtentExport.ExportToString(GetExampleExtent());
            Assert.That(exportedText.Contains("dm:///test"));
            Assert.That(exportedText.Contains("DatenMeister Yeahaa"));
            Assert.That(exportedText.Contains("def"));
        }
    }
}