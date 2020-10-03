using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Html
{
    public class HtmlEvalReportHeadline : IHtmlReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_DatenMeister.TheOne.Reports.__ReportHeadline) == true;
        }

        public void Evaluate(HtmlReportCreator htmlReportCreator, IElement reportNode)
        {
            var title = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportHeadline.title);
            htmlReportCreator.HtmlReporter.Add(new HtmlHeadline(title, 1));
        }
    }
}