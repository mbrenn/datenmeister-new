using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class ResolverTests
    {
        [Test]
        public void TestById()
        {
            var extent = GetTestExtent();
            var firstChild = extent.GetUriResolver().Resolve(testUri + "#child1", ResolveType.Default)
                as IElement;
            var noChild = extent.GetUriResolver().Resolve(testUri + "#none", ResolveType.Default)
                as IElement;
            var child2Child1 = extent.GetUriResolver().Resolve(testUri + "#child2child1", ResolveType.Default)
                as IElement;
            var item1 = extent.GetUriResolver().Resolve(testUri + "#item1", ResolveType.Default)
                as IElement;
            var item1_2 = extent.GetUriResolver().Resolve("#item1", ResolveType.Default)
                as IElement;

            Assert.That(firstChild, Is.Not.Null);
            Assert.That(item1, Is.Not.Null);
            Assert.That(item1_2, Is.Not.Null);
            Assert.That(noChild, Is.Null);
            Assert.That(child2Child1, Is.Not.Null);
            Assert.That(firstChild.getOrDefault<string>("name"), Is.EqualTo("child1"));
            Assert.That(item1.getOrDefault<string>("name"), Is.EqualTo("item1"));
            Assert.That(item1_2.getOrDefault<string>("name"), Is.EqualTo("item1"));
            Assert.That(child2Child1.getOrDefault<string>("name"), Is.EqualTo("child2child1"));
        }
        
        [Test]
        public void TestByFullname()
        {
            var extent = GetTestExtent();
            var firstChild = extent.GetUriResolver().Resolve(testUri + "?fn=item2::child1", ResolveType.Default)
                as IElement;
            var noChild = extent.GetUriResolver().Resolve(testUri + "?fn=none", ResolveType.Default)
                as IElement;
            var child2Child1 = extent.GetUriResolver().Resolve(testUri + "?fn=item2::child2::child2child1", ResolveType.Default)
                as IElement;
            var item1 = extent.GetUriResolver().Resolve(testUri + "?fn=item1", ResolveType.Default)
                as IElement;

            Assert.That(firstChild, Is.Not.Null);
            Assert.That(item1, Is.Not.Null);
            Assert.That(noChild, Is.Null);
            Assert.That(child2Child1, Is.Not.Null);
            Assert.That(firstChild.getOrDefault<string>("name"), Is.EqualTo("child1"));
            Assert.That(item1.getOrDefault<string>("name"), Is.EqualTo("item1"));
            Assert.That(child2Child1.getOrDefault<string>("name"), Is.EqualTo("child2child1"));
        }
        
        [Test]
        public void TestByProperty()
        {
            var extent = GetTestExtent();
            var firstChild = extent.GetUriResolver().Resolve(testUri + "fn=item2&prop=packagedElement", ResolveType.Default)
                as IReflectiveSequence;

            Assert.That(firstChild, Is.Not.Null);
            var asList = firstChild.ToList<object>();

            Assert.That(asList.Count, Is.EqualTo(2));
            Assert.That(
                asList.OfType<IElement>().Any(x => x.getOrDefault<string>("name") == "child2"),
                Is.True);
            Assert.That(
                asList.OfType<IElement>().Any(x => x.getOrDefault<string>("name") == "child1"),
                Is.True);
        }

        /// <summary>
        /// Defines the test uri
        /// </summary>
        private const string testUri = "dm:///Test";
        
        /// <summary>
        /// Gets the test extent
        /// </summary>
        /// <returns>The extent being used for test extent</returns>
        public IUriExtent GetTestExtent()
        {
            var document = @"
<item xmlns:p1=""http://www.omg.org/spec/XMI/20131001"">
    <item p1:id=""item1"" name=""item1"" />
    <item p1:id=""item2"" name=""item2"">
        <packagedElement p1:id=""child1"" name=""child1"" />
        <packagedElement p1:id=""child2"" name=""child2"">
            <packagedElement p1:id=""child2child1"" name=""child2child1"" />
        </packagedElement>
        <packagedElement p1:id=""child3"" name=""child3"" />
    </item>
    <item p1:id=""item3"" name=""item3"" /> 
</item>";

            var provider = new XmiProvider(XDocument.Parse(document));
            return new MofUriExtent(provider, testUri);
        }
    }
}