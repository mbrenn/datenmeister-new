using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;

namespace DatenMeister.Modules.Reports
{
    /// <summary>
    /// Defines the evaluator for the report
    /// </summary>
    public interface IHtmlReportEvaluator
    {
        /// <summary>
        /// Gets the information whether the report factory is relevant for the given element 
        /// </summary>
        public bool IsRelevant(IElement element);

        /// <summary>
        /// Performs the evaluation
        /// </summary>
        /// <param name="htmlReportCreator">Report creator</param>
        /// <param name="reportNode">The report node</param>
        public void Evaluate(HtmlReportCreator htmlReportCreator, IElement reportNode);
    }
}