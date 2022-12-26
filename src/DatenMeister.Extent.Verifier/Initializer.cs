using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Verifier.Verifiers;

namespace DatenMeister.Extent.Verifier;

public static class Initializer
{
    public static void InitWithDefaultVerifiers(IWorkspaceLogic workspaceLogic, Verifier verifier)
    {
        verifier.AddVerifier(() => new DuplicateIdVerifier(workspaceLogic));
    }
}