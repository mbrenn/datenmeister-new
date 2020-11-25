using System.Collections.Generic;
using System.Linq;
using DatenMeister.Modules.Reports.Generic;

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
        private readonly List<IGenericReportEvaluator<AdocGenericReportCreator>> _adocReportEvaluators = 
            new List<IGenericReportEvaluator<AdocGenericReportCreator>>();
        
        /// <summary>
        /// Gets the evaluators
        /// </summary>
        public IEnumerable<IGenericReportEvaluator<AdocGenericReportCreator>> Evaluators => _adocReportEvaluators.ToList();

        public void AddEvaluator(IGenericReportEvaluator<AdocGenericReportCreator> evaluator)
        {
            _adocReportEvaluators.Add(evaluator);
        }
    }
}