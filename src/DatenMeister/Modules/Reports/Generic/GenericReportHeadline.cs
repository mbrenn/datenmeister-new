using DatenMeister.Runtime;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;

namespace DatenMeister.Modules.Reports.Generic
{
    public abstract class GenericReportHeadline<T> :
        IGenericReportEvaluator<T> where T : GenericReportCreator
    {
        /// <summary>
        /// Stores the relevant MetaClass
        /// </summary>
        private readonly IElement _relevantMetaClass;

        public GenericReportHeadline()
        {
            _relevantMetaClass = _DatenMeister.TheOne.Reports.__ReportHeadline;
        }

        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_relevantMetaClass) == true;
        }

        /// <summary>
        /// Performs the evaluation
        /// </summary>
        /// <param name="genericReportCreator">Report creator</param>
        /// <param name="reportNode">The report node</param>
        public void Evaluate(T genericReportCreator, IElement reportNode)
        {
            var headline = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportHeadline.title);
            WriteHeadline(genericReportCreator, headline);
        }

        /// <summary>
        /// Writes the headline
        /// </summary>
        /// <param name="reportCreator">Creates the report</param>
        /// <param name="headline">Headline to be used</param>
        public abstract void WriteHeadline(T reportCreator, string headline);
    }
}
