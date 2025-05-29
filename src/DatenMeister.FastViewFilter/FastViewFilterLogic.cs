using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Types;

namespace DatenMeister.FastViewFilter;

public class FastViewFilterLogic
{
    /// <summary>
    /// Defines the path to the packages of the fast view filters
    /// </summary>
    public const string PackagePathTypesFastViewFilters = "DatenMeister::FastViewFilters";

    private readonly LocalTypeSupport _localTypeSupport;

    public FastViewFilterLogic(LocalTypeSupport localTypeSupport,
        IWorkspaceLogic workspaceLogic)
    {
        _localTypeSupport = localTypeSupport;
    }

    /// <summary>
    /// Enumeration of the fast view filters
    /// </summary>
    public IReflectiveCollection FastViewFilters =>
        (PackageMethods.GetPackagedObjects(
             _localTypeSupport.InternalTypes.elements(), PackagePathTypesFastViewFilters)
         ?? throw new InvalidOperationException("Fast View Filters not found"))
        .WhenMetaClassIs(_UML.TheOne.StructuredClassifiers.__Class);
}