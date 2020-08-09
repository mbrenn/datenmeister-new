using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.DataViews;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class ResolverTests
    {
        [Test]
        public void TestExtentUrlNavigator()
        {
            var extent = GetTestExtent();
            var uriExtentNavigator = new ExtentUrlNavigator(extent);
            var firstChild = uriExtentNavigator.element(TestUri + "#child1") as IElement;
            var firstChildCached = uriExtentNavigator.element(TestUri + "#child1") as IElement;
            
            Assert.That(firstChild, Is.Not.Null);
            Assert.That(firstChildCached, Is.Not.Null);
            Assert.That(firstChild.getOrDefault<string>("name"), Is.EqualTo("child1"));
            Assert.That(firstChildCached.getOrDefault<string>("name"), Is.EqualTo("child1"));
        }
        
        [Test]
        public void TestById()
        {
            var extent = GetTestExtent();
            var firstChild = extent.GetUriResolver().Resolve(TestUri + "#child1", ResolveType.Default)
                as IElement;
            var firstChildCached = extent.GetUriResolver().Resolve(TestUri + "#child1", ResolveType.Default)
                as IElement;
            var noChild = extent.GetUriResolver().Resolve(TestUri + "#none", ResolveType.Default)
                as IElement;
            var child2Child1 = extent.GetUriResolver().Resolve(TestUri + "#child2child1", ResolveType.Default)
                as IElement;
            var item1 = extent.GetUriResolver().Resolve(TestUri + "#item1", ResolveType.Default)
                as IElement;
            var item1_2 = extent.GetUriResolver().Resolve("#item1", ResolveType.Default)
                as IElement;

            Assert.That(firstChild, Is.Not.Null);
            Assert.That(firstChildCached, Is.Not.Null);
            Assert.That(item1, Is.Not.Null);
            Assert.That(item1_2, Is.Not.Null);
            Assert.That(noChild, Is.Null);
            Assert.That(child2Child1, Is.Not.Null);
            Assert.That(firstChild.getOrDefault<string>("name"), Is.EqualTo("child1"));
            Assert.That(firstChildCached.getOrDefault<string>("name"), Is.EqualTo("child1"));
            Assert.That(item1.getOrDefault<string>("name"), Is.EqualTo("item1"));
            Assert.That(item1_2.getOrDefault<string>("name"), Is.EqualTo("item1"));
            Assert.That(child2Child1.getOrDefault<string>("name"), Is.EqualTo("child2child1"));
        }
        
        [Test]
        public void TestByFullname()
        {
            var extent = GetTestExtent();
            var firstChild = extent.GetUriResolver().Resolve(TestUri + "?fn=item2::child1", ResolveType.Default)
                as IElement;
            var noChild = extent.GetUriResolver().Resolve(TestUri + "?fn=none", ResolveType.Default)
                as IElement;
            var child2Child1 = extent.GetUriResolver().Resolve(TestUri + "?fn=item2::child2::child2child1", ResolveType.Default)
                as IElement;
            var item1 = extent.GetUriResolver().Resolve(TestUri + "?fn=item1", ResolveType.Default)
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
            var firstChild = extent.GetUriResolver().Resolve(TestUri + "?fn=item2&prop=packagedElement", ResolveType.Default)
                as IReflectiveSequence;

            Assert.That(firstChild, Is.Not.Null);
            var asList = firstChild.ToList<object>();

            Assert.That(asList.Count, Is.EqualTo(3));
            Assert.That(
                asList.OfType<IElement>().Any(x => x.getOrDefault<string>("name") == "child2"),
                Is.True);
            Assert.That(
                asList.OfType<IElement>().Any(x => x.getOrDefault<string>("name") == "child1"),
                Is.True);
        }
        
        [Test]
        public void TestByDataView()
        {
            var extent = GetTestExtent();
            var firstChild = extent.GetUriResolver().Resolve(
                    TestUri + "?fn=item2&prop=packagedElement&dataview=#child1Filter",
                    ResolveType.Default)
                as IReflectiveSequence;

            Assert.That(firstChild, Is.Not.Null);
            var asList = firstChild.ToList<object>();

            Assert.That(asList.Count, Is.EqualTo(1));
            Assert.That(
                asList.OfType<IElement>().All(x => x.getOrDefault<string>("name") != "child2"),
                Is.True);
            Assert.That(
                asList.OfType<IElement>().Any(x => x.getOrDefault<string>("name") == "child1"),
                Is.True);
        }

        [Test]
        public void TestGetExtentPerResolver()
        {
            var extent = GetTestExtent();
            var found = extent.GetUriResolver().Resolve(TestUri, ResolveType.Default);
            Assert.That(found, Is.Not.Null);
            Assert.That(found is IUriExtent, Is.True);
            Assert.That(found.Equals(extent));
        }

        /// <summary>
        /// Defines the test uri
        /// </summary>
        private const string TestUri = "dm:///Test";
        
        /// <summary>
        /// Gets the test extent
        /// </summary>
        /// <returns>The extent being used for test extent</returns>
        private static MofUriExtent GetTestExtent()
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
    
    <item p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package"" name=""Filter Open"">
        <packagedElement p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterPropertyNode""
            name=""Open Issues""
            input-ref=""#filterNode1"" property=""name"" value=""child1"" comparisonMode=""Equal""
                p1:id=""child1Filter"" />
        <packagedElement
            p1:type=""dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode""
            name=""input"" p1:id=""filterNode1""/>
    </item>
</item>";

            var scopeStorage = new ScopeStorage();
            scopeStorage.Add(DataViewPlugin.GetDefaultViewNodeFactories());
            var provider = new XmiProvider(XDocument.Parse(document));
            return new MofUriExtent(provider, TestUri, scopeStorage);
        }
    }
}