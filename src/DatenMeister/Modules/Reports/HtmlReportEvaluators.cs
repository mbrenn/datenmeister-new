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
        public List<IHtmlReportEvaluator> Evaluators => _htmlReportEvaluators.ToList();

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