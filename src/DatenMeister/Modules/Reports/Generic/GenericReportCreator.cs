using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Reports.Generic
{
    public abstract class GenericReportCreator
     {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(GenericReportCreator));

        public abstract void StartReport(ReportLogic logic, IObject reportDefinition);

        public abstract void EndReport(ReportLogic logic, IObject definition);

        public abstract void EvaluateElements(ReportLogic reportLogic, IReflectiveCollection reportElements);
     }
}