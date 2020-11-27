using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.Reports.Generic
{
    public interface IGenericReportEvaluator<T> where T : GenericReportCreator
    {
        /// <summary>
        /// Gets the information whether the report factory is relevant for the given element 
        /// </summary>
        public bool IsRelevant(IElement element);

        /// <summary>
        /// Performs the evaluation
        /// </summary>
        /// <param name="reportLogic">Adds the report logic</param>
        /// <param name="reportCreator">Report reportCreator</param>
        /// <param name="reportNode">The report node</param>
        public void Evaluate(ReportLogic reportLogic, T reportCreator, IElement reportNode);
    }
}