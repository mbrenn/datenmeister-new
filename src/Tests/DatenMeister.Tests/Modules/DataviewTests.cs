using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Types;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class DataviewTests
    {
        [Test]
        public async Task TestCreationOfDataviews()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var helper = dm.Resolve<DataViewHelper>();
            var viewWorkspace = helper.GetViewWorkspace();

            Assert.That(viewWorkspace.extent.Count(), Is.EqualTo(0));
            helper.CreateDataview("Test", "dm:///view/test");

            Assert.That(viewWorkspace.extent.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task TestPropertyFilter()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var dataExtent = await CreateDataForTest(dm);
            Assert.That(dataExtent.elements().Count(), Is.GreaterThan(1));

            var helper = dm.Resolve<DataViewHelper>();
            var dataView = helper.CreateDataview("Test", "dm:///view/test");

            var userViewExtent = helper.GetUserFormExtent();

            var factory = new MofFactory(userViewExtent);
            var extentSource = factory.create(_DatenMeister.TheOne.DataViews.__SourceExtentNode);
            extentSource.set(_DatenMeister._DataViews._SourceExtentNode.extentUri, "dm:///testdata");
            userViewExtent.elements().add(extentSource);

            var propertyFilter = factory.create(_DatenMeister.TheOne.DataViews.__FilterPropertyNode);
            userViewExtent.elements().add(propertyFilter);
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.property, "name");
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.comparisonMode,
                _DatenMeister._DataViews.___ComparisonMode.Contains);
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.value, "ai");
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.input, extentSource);

            dataView.set(_DatenMeister._DataViews._DataView.viewNode, propertyFilter);

            var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
            var extent = workspaceLogic.FindExtent("dm:///view/test");

            Assert.That(extent, Is.Not.Null);

            var elements = extent!.elements().OfType<IElement>().ToArray();
            Assert.That(elements.All(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.True);
            Assert.That(elements.Any(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.True);
            Assert.That(elements.Length, Is.GreaterThan(0));

            // Go to Non-Contain
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.comparisonMode,
                _DatenMeister._DataViews.___ComparisonMode.DoesNotContain);
            elements = extent.elements().OfType<IElement>().ToArray();
            Assert.That(elements.All(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.False);
            Assert.That(elements.Any(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.False);
            Assert.That(elements.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task TestDynamicSourceNodes()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var dataExtent = await CreateDataForTest(dm);

            var factory = InMemoryObject.TemporaryFactory;

            // Creates the dataview
            var extentSource = factory.create(_DatenMeister.TheOne.DataViews.__DynamicSourceNode);
            extentSource.set(_DatenMeister._DataViews._DynamicSourceNode.name, "input");

            var propertyFilter = factory.create(_DatenMeister.TheOne.DataViews.__FilterPropertyNode);
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.property, "name");
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.comparisonMode,
                _DatenMeister._DataViews.___ComparisonMode.Contains);
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.value, "ai");
            propertyFilter.set(_DatenMeister._DataViews._FilterPropertyNode.input, extentSource);

            // Gets the elements
            var dataViewEvaluator = new DataViewEvaluation(dm.WorkspaceLogic, dm.ScopeStorage);
            dataViewEvaluator.AddDynamicSource("input", dataExtent.elements());

            var elements = dataViewEvaluator.GetElementsForViewNode(propertyFilter)
                .OfType<IElement>().ToArray();

            // Evaluates the elements
            Assert.That(elements.All(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.True);
            Assert.That(elements.Any(x => x.getOrDefault<string>("name")?.Contains("ai") == true), Is.True);
            Assert.That(elements.Length, Is.GreaterThan(0));
        }

        private async Task<IUriExtent> CreateDataForTest(IDatenMeisterScope dm)
        {
            var localTypeSupport = dm.Resolve<LocalTypeSupport>();
            var userTypeExtent = localTypeSupport.GetUserTypeExtent();

            // Create two example types
            var userTypeFactory = new MofFactory(userTypeExtent);
            var createdClass = userTypeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
            createdClass.set(_UML._CommonStructure._NamedElement.name, "First Class");
            userTypeExtent.elements().add(createdClass);

            var secondClass = userTypeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
            secondClass.set(_UML._CommonStructure._NamedElement.name, "Second Class");
            userTypeExtent.elements().add(secondClass);

            // Ok, now add the data
            var extent = (await XmiExtensions
                .CreateAndAddXmiExtent(dm.Resolve<ExtentManager>(), "dm:///testdata", "testdata.xmi")).Extent!;
            Assert.That(extent, Is.Not.Null);
            var factory = new MofFactory(extent);
            var element1 = factory.create(createdClass);
            element1.set("name", "Bach");
            element1.set("zip", 32432);
            extent.elements().add(element1);

            element1 = factory.create(createdClass);
            element1.set("name", "Mainz");
            element1.set("zip", 55130);
            extent.elements().add(element1);

            element1 = factory.create(secondClass);
            element1.set("name", "Bischofsheim");
            element1.set("zip", 65474);
            extent.elements().add(element1);

            return extent;
        }
    }
}