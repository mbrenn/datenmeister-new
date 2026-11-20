using DatenMeister.Core.Interfaces.MOF.Common;

namespace DatenMeister.Actions.ActionHandler.Checker;

/// <summary>
/// Carries the extent URLs required by a checker to perform a comparison.
/// The master extent contains the authoritative (golden master) data,
/// the derivative extent contains the data to be verified, and the report
/// extent stores the findings and issues produced by the checker.
/// </summary>
public class CheckerParameter
{
    /// <summary>
    /// The URL of the golden master extent containing the authoritative data.
    /// </summary>
    public IReflectiveCollection MasterExtent { get; set; } = null!;

    /// <summary>
    /// The URL of the derivative extent to be compared against the master.
    /// </summary>
    public IReflectiveCollection DerivativeExtent { get; set; } = null!;

    /// <summary>
    /// The URL of the extent where report entries (findings and issues) are stored.
    /// </summary>
    public IReflectiveCollection ReportExtent { get; set; } = null!;
}