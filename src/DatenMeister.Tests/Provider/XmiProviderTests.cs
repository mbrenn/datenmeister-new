using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    [TestFixture]
    public class XmiProviderTests
    {
        [Test]
        public void TestGetAndSet()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestGetAndSetOfPrimitiveTypes(provider);
        }
        
        [Test]
        public void TestLists()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestListsWithObjects(provider);
        }

        [Test]
        public void TestMovements()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestListMovement(provider);
        }

        [Test]
        public void TestSetReferenceAndSetValue()
        {
            var provider = new XmiProvider();
            ProviderTestHelper.TestSetReferenceAndSetValue(provider);
        }

        [Test]
        public void TestStringReferenceInXmi()
        {
            var x =
                @"
<xml xmlns:p1=""http://www.omg.org/spec/XMI/20131001"">
    <node element=""test"" p1:id=""first"" />
    <node element=""first"" p1:id=""second"" />
</xml>";
            var xmiProvider = new XmiProvider(XDocument.Parse(x));
            var uriExtent = new MofUriExtent(xmiProvider);

            var first = uriExtent.elements().ElementAt(0) as IElement;
            var second = uriExtent.elements().ElementAt(1) as IElement;

            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);

            Assert.That((second as IHasId)!.Id, Is.EqualTo("second"));
            Assert.That((first as IHasId)!.Id, Is.EqualTo("first"));

            Assert.That(second.getOrDefault<string>("element"), Is.EqualTo("first"));
            var secondElement = second.getOrDefault<IElement>("element");
            Assert.That(secondElement, Is.Not.Null);
            Assert.That((secondElement as IHasId)!.Id, Is.EqualTo("first"));
        }
    }
}