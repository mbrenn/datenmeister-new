using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Modules.Reports.Adoc
{
    /// <summary>
    /// The report factories
    /// </summary>
    public class AdocReportEvaluators
    {
        /// <summary>
        /// Stores the html report evaluators
        /// </summary>
        private readonly List<IAdocReportEvaluator> _adocReportEvaluators = new List<IAdocReportEvaluator>();
        
        /// <summary>
        /// Gets the evaluators
        /// </summary>
        public IEnumerable<IAdocReportEvaluator> Evaluators => _adocReportEvaluators.ToList();

        public void AddEvaluator(IAdocReportEvaluator evaluator)
        {
            _adocReportEvaluators.Add(evaluator);
        }

        /// <summary>
        /// Adds an additional evaluators
        /// </summary>
        /// <param name="evaluator"></param>
        public void Add(IAdocReportEvaluator evaluator)
        {
            _adocReportEvaluators.Add(evaluator);
        }
    }
}