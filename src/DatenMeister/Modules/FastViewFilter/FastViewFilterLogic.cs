using System;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Functions.Queries;

namespace DatenMeister.Modules.FastViewFilter
{
    public class FastViewFilterLogic
    {
        /// <summary>
        /// Defines the path to the packages of the fast view filters
        /// </summary>
        public const string PackagePathTypesFastViewFilters = "DatenMeister::FastViewFilters";

        private readonly LocalTypeSupport _localTypeSupport;
        private readonly PackageMethods _packageMethods;

        public FastViewFilterLogic(LocalTypeSupport localTypeSupport, PackageMethods packageMethods,
            IWorkspaceLogic workspaceLogic)
        {
            _localTypeSupport = localTypeSupport;
            _packageMethods = packageMethods;
        }

        /// <summary>
        /// Enumeration of the fast view filters
        /// </summary>
        public IReflectiveCollection FastViewFilters =>
            (_packageMethods.GetPackagedObjects(
                 _localTypeSupport.InternalTypes.elements(), PackagePathTypesFastViewFilters)
             ?? throw new InvalidOperationException("Fast View Filters not found"))
            .WhenMetaClassIs(_UML.TheOne.StructuredClassifiers.__Class);
    }
}