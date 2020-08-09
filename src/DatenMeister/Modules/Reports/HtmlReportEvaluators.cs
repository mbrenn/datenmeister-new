using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Modules.Reports
{
    /// <summary>
    /// The report factories
    /// </summary>
    public class HtmlReportEvaluators
    {
        /// <summary>
        /// Stores the html report evaluators
        /// </summary>
        private readonly List<IHtmlReportEvaluator> _htmlReportEvaluators = new List<IHtmlReportEvaluator>();
        
        /// <summary>
        /// Gets the evaluators
        /// </summary>
        public IEnumerable<IHtmlReportEvaluator> Evaluators => _htmlReportEvaluators.ToList();

        public void AddEvaluator(IHtmlReportEvaluator evaluator)
        {
            _htmlReportEvaluators.Add(evaluator);
        }

        /// <summary>
        /// Adds an additional evaluators
        /// </summary>
        /// <param name="evaluator"></param>
        public void Add(IHtmlReportEvaluator evaluator)
        {
            _htmlReportEvaluators.Add(evaluator);
        }
    }
}