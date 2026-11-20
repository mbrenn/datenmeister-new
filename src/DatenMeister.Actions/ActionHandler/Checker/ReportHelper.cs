using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;

namespace DatenMeister.Actions.ActionHandler.Checker;

/// <summary>
/// Static helper methods for creating and managing report entries during extent comparison.
/// Each checker uses these helpers to write findings to the report extent.
/// </summary>
public static class ReportHelper
{
    /// <summary>
    /// Creates a new <c>ReportEntry_Wrapper</c> backed by the given reflective collection.
    /// The wrapper is used to construct a single report entry before adding it to the report extent.
    /// </summary>
    /// <param name="reflectiveCollection">The report extent collection that provides the MOF factory for creating the entry.</param>
    /// <returns>A new <c>ReportEntry_Wrapper</c> instance ready to be populated with finding details.</returns>
    public static Model.Actions.Checker.ReportEntry_Wrapper CreateWrapper(IReflectiveCollection reflectiveCollection)
    {
        var factory = new MofFactory(reflectiveCollection);
        return Model.Actions.Checker.ReportEntry_Wrapper.Create(factory);
    }

    /// <summary>
    /// Adds a completed report entry to the report extent collection.
    /// </summary>
    /// <param name="collection">The report extent collection that stores all findings and issues.</param>
    /// <param name="entry">The populated report entry wrapper to add.</param>
    public static void Add(IReflectiveCollection collection, Model.Actions.Checker.ReportEntry_Wrapper entry)
    {
        collection.add(entry.GetWrappedElement());
    }
}