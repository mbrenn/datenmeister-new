using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Reports.Generic
{
    public abstract class GenericReportCreator
     {
        private static readonly ClassLogger Logger = new(typeof(GenericReportCreator));

        public abstract void StartReport(ReportLogic logic, IElement reportInstance, IObject reportDefinition);

        public abstract void EndReport(ReportLogic logic, IObject definition);

        /// <summary>
        /// Evaluates the reporting elements. 
        /// </summary>
        /// <param name="reportLogic">Report Logic to be used</param>
        /// <param name="reportElements">The report elements which define which elements will be consist
        /// of the elements</param>
        public abstract void EvaluateElements(ReportLogic reportLogic, IReflectiveCollection reportElements);
     }
}