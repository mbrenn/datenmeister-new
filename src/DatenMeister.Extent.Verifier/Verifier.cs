using System.Collections.Concurrent;
using BurnSystems.Logging;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Extent.Verifier;

/// <summary>
/// Verifies that the extents are correct and there is no inconsistency in the data
/// </summary>
public class Verifier : IWorkspaceVerifierLog
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ClassLogger Logger = new(typeof(Verifier));
    
    /// <summary>
    /// Defines the workspace logic
    /// </summary>
    private readonly IWorkspaceLogic _workspaceLogic;

    /// <summary>
    /// Stores the verify entries
    /// </summary>
    private readonly ConcurrentBag<VerifyEntry> _verifyEntries = new();

    /// <summary>
    /// Stores the factory for the extent verifiers
    /// </summary>
    private readonly List<Func<IWorkspaceVerifier>> _extentVerifyFactories = new();

    /// <summary>
    /// Adds a verifier
    /// </summary>
    /// <param name="value"></param>
    public void AddVerifier(Func<IWorkspaceVerifier> value) => _extentVerifyFactories.Add(value);

    /// <summary>
    /// Gets a copy of all verify entries
    /// </summary>
    public List<VerifyEntry> VerifyEntries => _verifyEntries.ToList();

    public Verifier(IWorkspaceLogic workspaceLogic)
    {
        _workspaceLogic = workspaceLogic;
    }

    /// <summary>
    /// Creates all factories and verifies
    /// </summary>
    public async Task VerifyExtents()
    {
        Logger.Info("Starting Verification");
        try
        {

            foreach (var verifierFactory in _extentVerifyFactories)
            {
                await verifierFactory().VerifyExtents(this);
            }
        }
        catch (Exception exc)
        {
            Logger.Error("Failure during verification: " + exc);
        }

        Logger.Info("Finalized Verification");
    }

    /// <summary>
    /// Adds an entry, this is a type-safe operation
    /// </summary>
    /// <param name="entry"></param>
    public void AddEntry(VerifyEntry entry)
    {
        Logger.Info("Verification issue: " + entry);
        _verifyEntries.Add(entry);
    }
}