using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Adoc
{
    /// <summary>
    /// The report factories
    /// </summary>
    public class AdocReportEvaluators
    {
        /// <summary>
        /// Stores the html report evaluators
        /// </summary>
        private readonly List<IGenericReportEvaluator<AdocReportCreator>> _adocReportEvaluators = new();
        
        /// <summary>
        /// Gets the evaluators
        /// </summary>
        public IEnumerable<IGenericReportEvaluator<AdocReportCreator>> Evaluators => _adocReportEvaluators.ToList();

        public void AddEvaluator(IGenericReportEvaluator<AdocReportCreator> evaluator)
        {
            _adocReportEvaluators.Add(evaluator);
        }
    }
}