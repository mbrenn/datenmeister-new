using Autofac;
using DatenMeister.Core;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestNamedElementMethods
    {
        [Test]
        public void TestFullName()
        {
            using var builder = DatenMeisterTests.GetDatenMeisterScope();
            using var scope = builder.BeginLifetimeScope();
            var workspaceCollection = scope.Resolve<IWorkspaceLogic>();
            var dataLayerLogic = scope.Resolve<WorkspaceLogic>();

            // Gets the logic
            var uml = dataLayerLogic.GetUmlWorkspace().Get<_UML>();
            var feature = uml.Classification.__Feature;
            var fullName = NamedElementMethods.GetFullName(feature);

            Assert.That(fullName, Is.Not.Null);
            Assert.That(fullName, Is.EqualTo("UML::Classification::Feature"));

            var umlExtent = workspaceCollection.GetWorkspace(WorkspaceNames.WorkspaceUml).FindExtent(WorkspaceNames.UriExtentUml);
            // now the other way
            var foundElement = NamedElementMethods.GetByFullName(umlExtent.elements(), fullName);
            Assert.That(foundElement, Is.EqualTo(feature));
        }
        
    }
}