using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Reports.Generic
{
    public abstract class GenericReportCreator
     {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(GenericReportCreator));

        public abstract void StartReport(GenericReportLogic logic, IObject reportDefinition);

        public abstract void EndReport(GenericReportLogic logic, IObject definition);

        public abstract void EvaluateElements(GenericReportLogic reportLogic, IReflectiveCollection reportElements);
     }
}