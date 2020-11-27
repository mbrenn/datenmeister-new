using System.Collections.Generic;
using System.Linq;
using DatenMeister.Modules.Reports.Adoc;
using DatenMeister.Modules.Reports.Generic;

namespace DatenMeister.Modules.Reports.Html
{
    /// <summary>
    /// The report factories
    /// </summary>
    public class HtmlReportEvaluators
    {
        /// <summary>
        /// Stores the html report evaluators
        /// </summary>
        private readonly List<IGenericReportEvaluator<HtmlReportCreator>> _htmlReportEvaluators = new List<IGenericReportEvaluator<HtmlReportCreator>>();
        
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