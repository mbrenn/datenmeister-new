using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Interfaces.MOF.Identifiers;

/// <summary>
///     Defines the interface for the Extent as defined in MOF Specification 2.5, Chapter 10.2
/// </summary>
public interface IExtent : IObject
{
    bool useContainment();

    IReflectiveSequence elements();
}