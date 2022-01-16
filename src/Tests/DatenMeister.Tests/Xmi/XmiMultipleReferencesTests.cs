using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Provider.XMI;
using NUnit.Framework;

namespace DatenMeister.Tests.Xmi
{
    [TestFixture]
    public class XmiMultipleReferencesTests
    {
        [Test]
        public void TestReferencesForMultipleExtents()
        {
            const string xmi1 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"test\" value=\"23\" /></package>";
            const string xmi2 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"other\" value=\"23\" /></package>";

            (var extent1, var extent2) = LoadExtents(xmi1, xmi2);

            var item1 = extent1.elements().First() as IElement;
            Assert.That(item1, Is.Not.Null);

            var item2 = extent2.elements().First() as IElement;
            Assert.That(item2, Is.Not.Null);
            item2.set("sub", item1);

            VerifyXmlStructureForSingleReference(extent1, extent2);
        }

        [Test]
        public void TestListReferencesForMultipleExtents()
        {
            const string xmi1 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\">" +
                "<element xmi:id=\"item1\" value=\"23\" />" +
                "<element xmi:id=\"item2\" value=\"45\" />" +
                "<element xmi:id=\"item3\" value=\"47\" />" +
                "<element xmi:id=\"item4\" value=\"50\" />" +
                "</package>";
            const string xmi2 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"other\" value=\"23\" /></package>";

            (var extent1, var extent2) = LoadExtents(xmi1, xmi2);

            var subItem1 = extent1.element("#item1");
            var subItem2 = extent1.element("#item2");
            var subItem3 = extent1.element("#item3");
            var subItem4 = extent1.element("#item4");
            Assert.That(subItem1, Is.Not.Null);
            Assert.That(subItem2, Is.Not.Null);
            Assert.That(subItem3, Is.Not.Null);
            Assert.That(subItem4, Is.Not.Null);

            var item2 = extent2.elements().First() as IElement;
            Assert.That(item2, Is.Not.Null);
            item2.set("sub", new[] {subItem1, subItem2, subItem3, subItem4});

            VerifyXmlStructureForListReference(extent1, extent2);
        }

        [Test]
        public void TestLoadingForReferences()
        {
            const string xmi1 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"test\" value=\"23\" /></package>";
            const string xmi2 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"other\" value=\"23\"><sub href=\"dm:///xmi1/#test\" /></element></package>";

            (var extent1, var extent2) = LoadExtents(xmi1, xmi2);

            VerifyXmlStructureForSingleReference(extent1, extent2);
        }

        private static (MofUriExtent extent1, MofUriExtent extent2) LoadExtents(string xmi1, string xmi2)
        {
            var provider1 = new XmiProvider();
            var provider2 = new XmiProvider();
            var extent1 = new MofUriExtent(provider1, "dm:///xmi1/", null);
            var extent2 = new MofUriExtent(provider2, "dm:///xmi2/", null);

            var workspace = new Workspace("data");
            var loader = new SimpleLoader(workspace);
            workspace.AddExtent(extent1);
            workspace.AddExtent(extent2);
            loader.LoadFromText(new MofFactory(extent1), extent1, xmi1);
            loader.LoadFromText(new MofFactory(extent2), extent2, xmi2);

            return (extent1, extent2);
        }

        /// <summary>
        /// Verifies the Xml structure, that extent2 contains a reference to extent2
        /// </summary>
        /// <param name="extent1"></param>
        /// <param name="extent2"></param>
        private static void VerifyXmlStructureForSingleReference(MofUriExtent extent1, MofUriExtent extent2)
        {
            var item1 = extent1.elements().First() as IElement;
            Assert.That(item1, Is.Not.Null);
            var item2 = extent2.elements().First() as IElement;
            Assert.That(item2, Is.Not.Null);

            var retrievedItem1 = item2.get("sub") as IElement;
            Assert.That(retrievedItem1, Is.Not.Null);
            var foundUri = retrievedItem1.GetUri();
            Assert.That(foundUri, Is.EqualTo(item1.GetUri()));

            // Verifies that XmlNode structure is correct
            var xmlNode = ((item2 as MofElement)?.ProviderObject as XmiProviderObject)?.XmlNode;
            Assert.That(xmlNode, Is.Not.Null);
            var subElementNode = xmlNode.Elements("sub").FirstOrDefault();
            if (subElementNode != null)
            {
                Assert.That(subElementNode.Attribute("href"), Is.Not.Null);
            }
            else
            {
                Assert.That(xmlNode.Attribute("sub-ref"), Is.Not.Null);
            }

            item1.set("value", 54);
            Assert.That((item2.get("sub") as IElement)?.get("value")?.ToString(), Is.EqualTo("54"));
        }

        /// <summary>
        /// Verifies the Xml structure, that extent2 contains a reference to extent2
        /// </summary>
        /// <param name="extent1"></param>
        /// <param name="extent2"></param>
        private static void VerifyXmlStructureForListReference(MofUriExtent extent1, MofUriExtent extent2)
        {
            var item2 = extent2.elements().First() as IElement;
            Assert.That(item2, Is.Not.Null);

            var subItem1 = extent1.element("#item1");
            var subItem2 = extent1.element("#item2");
            var subItem3 = extent1.element("#item3");
            var subItem4 = extent1.element("#item4");
            Assert.That(subItem1, Is.Not.Null);
            Assert.That(subItem2, Is.Not.Null);
            Assert.That(subItem3, Is.Not.Null);
            Assert.That(subItem4, Is.Not.Null);

            var retrievedItem1 = item2.get("sub") as IReflectiveCollection;
            Assert.That(retrievedItem1, Is.Not.Null);

            var size = 0;
            foreach (var subItem in retrievedItem1.OfType<IElement>())
            {
                size++;
                Assert.That((subItem as IHasId)?.Id, Is.EqualTo("item" + size));
                Assert.That((subItem as IHasExtent)?.Extent, Is.EqualTo(extent1));
            }

            Assert.That(size, Is.EqualTo(4));
        }
    }
}