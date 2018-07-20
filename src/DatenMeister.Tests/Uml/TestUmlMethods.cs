using System.Linq;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.Core;
using DatenMeister.Core.Filler;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestUmlMethods
    {
        [Test]
        public void TestGeneralizedProperties()
        {
            var kernel = new ContainerBuilder();
            var builder = kernel.UseDatenMeister(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {
                var dataLayerLogic = scope.Resolve<WorkspaceLogic>();

                // Gets the logic
                var uml = dataLayerLogic.GetUmlWorkspace().Get<_UML>();
                var feature = uml.Classification.__Feature;
                var properties = ClassifierMethods.GetPropertyNamesOfClassifier(feature).ToList();

                Assert.That(properties.Contains(_UML._Classification._Feature.isStatic), Is.True,
                    "isStatic");
                Assert.That(properties.Contains(_UML._Classification._RedefinableElement.isLeaf), Is.True,
                    "isLeaf (Parent)");
                Assert.That(properties.Contains(_UML._CommonStructure._NamedElement.name), Is.True,
                    "name (Parent of Parent)");
            }
        }

        [Test]
        public void TestFullName()
        {
            var kernel = new ContainerBuilder();
            var builder = kernel.UseDatenMeister(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {
                var workspaceCollection = scope.Resolve<IWorkspaceLogic>();
                var dataLayerLogic = scope.Resolve<WorkspaceLogic>();

                // Gets the logic
                var uml = dataLayerLogic.GetUmlWorkspace().Get<_UML>();
                var feature = uml.Classification.__Feature;
                var fullName = NamedElementMethods.GetFullName(feature);

                Assert.That(fullName, Is.Not.Null);
                Assert.That(fullName, Is.EqualTo("UML::Classification::Feature"));
                
                var umlExtent = workspaceCollection.GetWorkspace(WorkspaceNames.NameUml).FindExtent(WorkspaceNames.UriUmlExtent);
                // now the other way
                var foundElement = NamedElementMethods.GetByFullName(umlExtent.elements(), fullName);
                Assert.That(foundElement, Is.EqualTo(feature));
            }
        }
    }
}