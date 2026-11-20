using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Actions.ActionHandler.Checker;

/// <summary>
/// Defines a checker that compares elements between a golden master extent and a derivative extent.
/// Each checker verifies a specific criterion (e.g., completeness, property equality) and reports
/// findings to the report extent.
/// </summary>
public interface IChecker
{
    /// <summary>
    /// Checks whether the checker is responsible for the given checker configuration.
    /// </summary>
    /// <param name="checkerConfig">Configuration to be used</param>
    /// <returns>true, if the checker is responsible</returns>
    bool IsResponsible(IElement checkerConfig);
    
    /// <summary>
    /// Performs the comparison check between the master and derivative extents.
    /// </summary>
    /// <param name="parameters">Contains the URLs of the master, derivative, and report extents.</param>
    Task Check(CheckerParameter parameters);
}