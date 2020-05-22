using Autofac;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestMofUmlWorkspaceIntegration
    {
        [Test]
        public void TestUrisOfWorkspaces()
        {
            using var builder = DatenMeisterTests.GetDatenMeisterScope();
            
            using var scope = builder.BeginLifetimeScope();
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            var umlWorkspace = workspaceLogic.GetUmlWorkspace();
            var umlExtent = umlWorkspace.FindExtent(WorkspaceNames.UriUmlExtent);
            var activityElement = umlExtent.element("http://www.omg.org/spec/UML/20131001#Activity");
            Assert.That(activityElement, Is.Not.Null);

            var activityElement2 = workspaceLogic.FindItem("http://www.omg.org/spec/UML/20131001#Activity");
            Assert.That(activityElement2, Is.Not.Null);
            Assert.That(activityElement2.Equals(activityElement), Is.True);

            var umlExtent2 = umlWorkspace.FindExtent(WorkspaceNames.StandardUmlNamespace);
            Assert.That(umlExtent2, Is.Not.Null);
            Assert.That(umlExtent2.Equals(umlExtent), Is.True);
        }
    }
}