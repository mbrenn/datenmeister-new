using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Types;

namespace DatenMeister.FastViewFilter;

public class FastViewFilterLogic(LocalTypeSupport localTypeSupport)
{
    /// <summary>
    /// Defines the path to the packages of the fast view filters
    /// </summary>
    public const string PackagePathTypesFastViewFilters = "DatenMeister::FastViewFilters";

    /// <summary>
    /// Enumeration of the fast view filters
    /// </summary>
    public IReflectiveCollection FastViewFilters =>
        (PackageMethods.GetPackagedObjects(
             localTypeSupport.InternalTypes.elements(), PackagePathTypesFastViewFilters)
         ?? throw new InvalidOperationException("Fast View Filters not found"))
        .WhenMetaClassIs(_UML.TheOne.StructuredClassifiers.__Class);
}