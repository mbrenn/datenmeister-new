using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.FastViewFilter
{
    public class FastViewFilterLogic
    {
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly PackageMethods _packageMethods;
        private readonly _UML _uml;

        public FastViewFilterLogic(LocalTypeSupport localTypeSupport, PackageMethods packageMethods, IWorkspaceLogic workspaceLogic)
        {
            _localTypeSupport = localTypeSupport;
            _packageMethods = packageMethods;
            _uml = workspaceLogic.GetUmlData();
        }

        /// <summary>
        /// Enumeration of the fast view filters
        /// </summary>
        public IReflectiveCollection FastViewFilters =>
            _packageMethods.GetPackagedObjects(
                _localTypeSupport.InternalTypes.elements(),
                ViewLogic.PackagePathTypesFastViewFilters).WhenMetaClassIs(_uml.StructuredClassifiers.__Class);
    }
}