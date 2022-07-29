using System.Collections.Generic;
using System.Linq;
using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Html
{
    /// <summary>
    /// The report factories
    /// </summary>
    public class HtmlReportEvaluators
    {
        /// <summary>
        /// Stores the html report evaluators
        /// </summary>
        private readonly List<IGenericReportEvaluator<HtmlReportCreator>> _htmlReportEvaluators = new();
        
        /// <summary>
        /// Gets the evaluators
        /// </summary>
        public IEnumerable<IGenericReportEvaluator<HtmlReportCreator>> Evaluators => _htmlReportEvaluators.ToList();

        public void AddEvaluator(IGenericReportEvaluator<HtmlReportCreator> evaluator)
        {
            _htmlReportEvaluators.Add(evaluator);
        }
    }
}