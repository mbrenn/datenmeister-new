namespace DatenMeister.Extent.Verifier;

/// <summary>
/// This interface defines the log to which the verifiers can add one entry
/// into the log. The log is shown to the user and can trigger certain actions
/// </summary>
public interface IWorkspaceVerifierLog
{
    /// <summary>
    /// Adds the log to the entry
    /// </summary>
    /// <param name="entry">Entry to be handled</param>
    void AddEntry(VerifyEntry entry);
}