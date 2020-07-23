using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Evaluators
{
    public class HtmlReportHeadline : IHtmlReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_Reports.TheOne.__ReportHeadline) == true;
        }

        public void Evaluate(HtmlReportCreator htmlReportCreator, IElement reportNode)
        {
            var title = reportNode.getOrDefault<string>(_Reports._ReportHeadline.title);
            htmlReportCreator.HtmlReporter.Add(new HtmlHeadline(title, 1));
        }
    }
}