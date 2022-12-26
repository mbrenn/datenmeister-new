namespace DatenMeister.Extent.Verifier;

public interface IWorkspaceVerifier
{
    /// <summary>
    /// Verifies the extents within the given workspacelogic.
    /// If there is a failure in the data consistency, the information can be added to the logger 
    /// </summary>
   Task VerifyExtents(IWorkspaceVerifierLog log);

}