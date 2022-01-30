using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Provider
{
    [TestFixture]
    public class ManagementProviderTests
    {
        [Test]
        public void TestContainering()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();
            var workspace = scope.WorkspaceLogic.GetManagementWorkspace();
            Assert.That(workspace, Is.Not.Null);

            var extent = workspace.FindExtent(WorkspaceNames.UriExtentWorkspaces);
            Assert.That(extent, Is.Not.Null);

            var workspaceValue = extent!.elements().WhenPropertyHasValue(
                _DatenMeister._Management._Workspace.id,
                WorkspaceNames.WorkspaceManagement).FirstOrDefault() as IElement;
            Assert.That(workspaceValue, Is.Not.Null);
            Assert.That(workspaceValue!.get(_DatenMeister._Management._Workspace.id),
                Is.EqualTo(WorkspaceNames.WorkspaceManagement));

            var extents =
                workspaceValue.getOrDefault<IReflectiveCollection>(_DatenMeister._Management._Workspace.extents);
            Assert.That(extents, Is.Not.Null);
            var extentAsList = extents.ToList();
            Assert.That(extentAsList.Count, Is.GreaterThan(0));

            var firstExtent = extentAsList.First() as IElement;
            Assert.That(firstExtent, Is.Not.Null);
            var containeredElement = firstExtent!.container();
            Assert.That(containeredElement, Is.Not.Null);

            Assert.That(containeredElement, Is.EqualTo(workspaceValue));
        }
    }
}