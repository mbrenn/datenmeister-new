using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Extent.Verifier.Verifiers;

public class DuplicateIdVerifier : IWorkspaceVerifier
{
    private readonly IWorkspaceLogic _workspaceLogic;

    public DuplicateIdVerifier(IWorkspaceLogic workspaceLogic)
    {
        _workspaceLogic = workspaceLogic;
    }
    public async Task VerifyExtents(IWorkspaceVerifierLog log)
    {
        foreach (Workspace workspace in _workspaceLogic.Workspaces)
        {
            if (workspace.id is
                WorkspaceNames.WorkspaceMof
                or WorkspaceNames.WorkspaceUml
                or WorkspaceNames.WorkspaceViews)
            {
                continue;
            }

            foreach (IExtent extent in workspace.extent)
            {
                await Task.Run(() =>
                {
                    var bag = new HashSet<string>();
                    foreach (var item in extent.elements().GetAllDescendantsIncludingThemselves().OfType<IObject>())
                    {
                        if (item is not IHasId asId || asId.Id == null) continue;

                        if (bag.Contains(asId.Id))
                        {
                            log.AddEntry(
                                new VerifyEntry
                                {
                                    WorkspaceId = workspace.id,
                                    ItemUri = item.GetUri() ?? "Unknown Uri",
                                    Category = "DuplicateId",
                                    Message = "Duplicate ID: " + asId.Id

                                });
                        }

                        bag.Add(asId.Id);
                    }
                });
            }
        }
    }
}