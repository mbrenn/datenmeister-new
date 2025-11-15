using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Provider.ExtentManagement;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Provider;

[TestFixture]
public class ManagementProviderTests
{
    [Test]
    public async Task TestContainering()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
        var workspace = scope.WorkspaceLogic.GetManagementWorkspace();
        Assert.That(workspace, Is.Not.Null);

        var extent = workspace.FindExtent(WorkspaceNames.UriExtentWorkspaces);
        Assert.That(extent, Is.Not.Null);

        var workspaceValue = extent!.elements().WhenPropertyHasValue(
            _Management._Workspace.id,
            WorkspaceNames.WorkspaceManagement).FirstOrDefault() as IElement;
        Assert.That(workspaceValue, Is.Not.Null);
        Assert.That(workspaceValue!.get(_Management._Workspace.id),
            Is.EqualTo(WorkspaceNames.WorkspaceManagement));

        var extents =
            workspaceValue.getOrDefault<IReflectiveCollection>(_Management._Workspace.extents);
        Assert.That(extents, Is.Not.Null);
        var extentAsList = extents.ToList();
        Assert.That(extentAsList.Count, Is.GreaterThan(0));

        var firstExtent = extentAsList.First() as IElement;
        Assert.That(firstExtent, Is.Not.Null);
        var containeredElement = firstExtent!.container();
        Assert.That(containeredElement, Is.Not.Null);

        Assert.That(containeredElement, Is.EqualTo(workspaceValue));
    }

    [Test]
    public async Task TestGetWorkspaceElement()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();

        var managementExtent = ExtentManagementHelper.GetExtentForWorkspaces(scope.WorkspaceLogic);

        var workspaceElement = ExtentManagementHelper.GetWorkspaceElement(managementExtent, WorkspaceNames.WorkspaceTypes);
        Assert.That(workspaceElement, Is.Not.Null);
        Assert.That(workspaceElement.getOrDefault<string>(_Management._Workspace.id), Is.EqualTo(WorkspaceNames.WorkspaceTypes));
    }

    [Test]
    public async Task TestGetExtentElement()
    {
        await using var scope = await DatenMeisterTests.GetDatenMeisterScope();

        var managementExtent = ExtentManagementHelper.GetExtentForWorkspaces(scope.WorkspaceLogic);

        var extentElement =
            ExtentManagementHelper.GetExtentElement(managementExtent, WorkspaceNames.WorkspaceTypes, WorkspaceNames.UriExtentInternalTypes);
        Assert.That(extentElement, Is.Not.Null);
        Assert.That(extentElement.getOrDefault<string>(_Management._Extent.uri), Is.EqualTo(WorkspaceNames.UriExtentInternalTypes));
    }
}