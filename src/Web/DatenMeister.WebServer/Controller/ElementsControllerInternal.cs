using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Web.Json;
using DatenMeister.Provider.ExtentManagement;
using DatenMeister.WebServer.Library.Helper;

namespace DatenMeister.WebServer.Controller;

public class ElementsControllerInternal(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    private readonly IScopeStorage _scopeStorage = scopeStorage;


    public ItemWithNameAndId[]? GetComposites(string? workspaceId, string? itemUrl)
    {
        workspaceId = MvcUrlEncoder.DecodePath(workspaceId);
        itemUrl = MvcUrlEncoder.DecodePath(itemUrl);

        if (workspaceId == null)
        {
            var extent = ExtentManagementHelper.GetExtentForWorkspaces(workspaceLogic);
            var rootElements = extent.elements();

            return rootElements.OfType<IObject>().Select(x => ItemWithNameAndId.Create(x)!).ToArray();
        }

        if (itemUrl == null)
        {
            var workspaceExtent = ExtentManagementHelper.GetExtentForWorkspaces(workspaceLogic);
            var rootElements = workspaceExtent.elements();

            var foundExtent =
                rootElements
                    .OfType<IObject>()
                    .FirstOrDefault(x =>
                        x.getOrDefault<string>(_Management._Workspace.id) == workspaceId);

            var foundExtents =
                foundExtent?.getOrDefault<IReflectiveCollection>(_Management._Workspace.extents);

            return
                foundExtents?.OfType<IObject>()
                    .Select(t =>
                    {
                        var x = ItemWithNameAndId.Create(t)!;
                        var realExtent = workspaceLogic.FindExtent(
                            workspaceId,
                            t.get<string>(_Management._Extent.uri));
                        x.extentUri = (realExtent as IUriExtent)?.contextURI() ?? string.Empty;
                        if (t.isSet(_UML._CommonStructure._NamedElement.name))
                            x.name = realExtent.get<string>(_UML._CommonStructure._NamedElement.name);

                        if (string.IsNullOrEmpty(x.name))
                            x.name = (realExtent as IUriExtent)?.contextURI() ?? "Unknown";

                        return x;
                    })
                    .ToArray();
        }

        var workspace = workspaceLogic.GetWorkspace(workspaceId);
        if (workspace?.Resolve(itemUrl, ResolveType.IncludeWorkspace) is not IObject foundItem) return null;

        var packagedItems = DefaultClassifierHints.GetPackagedElements(foundItem);
        return packagedItems
            .OfType<IObject>()
            .Select(x => ItemWithNameAndId.Create(x)!).ToArray();
    }

    /// <summary>
    ///     Performs a search by the search string
    /// </summary>
    /// <param name="search">The string being typed in by the user</param>
    /// <returns>The answer by the logic. </returns>
    public ElementsController.FindBySearchStringResult FindBySearchString(string search)
    {
        var result = new ElementsController.FindBySearchStringResult();
        var found = workspaceLogic.Resolve(search, ResolveType.Default, false);
        if (found is IUriExtent asExtent)
        {
            result.ResultType = ElementsController.FindBySearchStringResult.ResultTypeReferenceExtent;
            result.Reference = ItemWithNameAndId.Create(asExtent)
                               ?? throw new InvalidOperationException("Create could not handle an object...");
        }
        else if (found is IObject asObject)
        {
            result.ResultType = ElementsController.FindBySearchStringResult.ResultTypeReference;
            result.Reference = ItemWithNameAndId.Create(asObject)
                               ?? throw new InvalidOperationException("Create could not handle an object...");
        }
        else
        {
            result.ResultType = ElementsController.FindBySearchStringResult.ResultTypeNone;
        }

        return result;
    }
}