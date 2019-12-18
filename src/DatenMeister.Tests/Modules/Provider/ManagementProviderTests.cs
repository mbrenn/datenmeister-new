using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.ManagementProvider;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
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

            var extent = workspace.FindExtent(WorkspaceNames.ExtentManagementExtentUri);
            Assert.That(extent, Is.Not.Null);

            var workspaceValue = extent.elements().WhenPropertyHasValue(
                _ManagementProvider._Workspace.id,
                WorkspaceNames.NameManagement).FirstOrDefault() as IElement;
            Assert.That(workspaceValue, Is.Not.Null);
            Assert.That(workspaceValue.get(_ManagementProvider._Workspace.id), Is.EqualTo(WorkspaceNames.NameManagement));

            var extents =
                workspaceValue.getOrDefault<IReflectiveCollection>(_ManagementProvider._Workspace.extents);
            Assert.That(extents, Is.Not.Null);
            var extentAsList = extents.ToList();
            Assert.That(extentAsList.Count, Is.GreaterThan(0));

            var firstExtent = extentAsList.First() as IElement;
            Assert.That(firstExtent, Is.Not.Null);
            var containeredElement = firstExtent.container();
            Assert.That(containeredElement, Is.Not.Null);

            Assert.That(containeredElement, Is.EqualTo(workspaceValue));
        }
    }
}