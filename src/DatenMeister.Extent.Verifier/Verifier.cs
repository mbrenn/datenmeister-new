using System.Collections.Concurrent;
using System.Diagnostics;
using BurnSystems.Logging;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.Extent.Verifier;

/// <summary>
/// Verifies that the extents are correct and there is no inconsistency in the data
/// </summary>
public class Verifier(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IWorkspaceVerifierLog
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ClassLogger Logger = new(typeof(Verifier));
    
    /// <summary>
    /// Defines the workspace logic
    /// </summary>
    private readonly IWorkspaceLogic _workspaceLogic = workspaceLogic;

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

    /// <summary>
    /// Creates all factories and verifies
    /// </summary>
    public async Task VerifyExtents()
    {
        Logger.Info("Starting Verification");
        var stopWatch = Stopwatch.StartNew();
        
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

        stopWatch.Stop();
        Logger.Info("Finalized Verification: " + stopWatch.ElapsedMilliseconds + " ms");
    }

    /// <summary>
    /// Adds an entry, this is a type-safe operation
    /// </summary>
    /// <param name="entry"></param>
    public void AddEntry(VerifyEntry entry)
    {
        Logger.Info("Verification issue: " + entry);
        
        // Creates a new item into the database
        var temporaryExtentLogic = new TemporaryExtentLogic(_workspaceLogic, scopeStorage);
        
        var newElement =
            temporaryExtentLogic.CreateTemporaryElement(_Verifier.TheOne.__VerifyEntry,
                TimeSpan.FromDays(30));
        
        newElement.set(_Verifier._VerifyEntry.workspaceId, entry.WorkspaceId);
        newElement.set(_Verifier._VerifyEntry.itemUri, entry.ItemUri);
        newElement.set(_Verifier._VerifyEntry.message, entry.Message);
        newElement.set(_Verifier._VerifyEntry.category, entry.Category);
        
        _verifyEntries.Add(entry);
    }
}