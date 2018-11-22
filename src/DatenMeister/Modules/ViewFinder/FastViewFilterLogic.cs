using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.ViewFinder
{
    public class FastViewFilterLogic
    {
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly PackageMethods _packageMethods;

        public FastViewFilterLogic(LocalTypeSupport localTypeSupport, PackageMethods packageMethods)
        {
            _localTypeSupport = localTypeSupport;
            _packageMethods = packageMethods;
        }

        /// <summary>
        /// Enumeration of the fast view filters
        /// </summary>
        public IReflectiveCollection FastViewFilters =>
            _packageMethods.GetPackagedObjects(
                _localTypeSupport.InternalTypes.elements(),
                ViewLogic.PackagePathTypesFastViewFilters);
    }
}