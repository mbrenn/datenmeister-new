using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Reports;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportTable : IAdocReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_Reports.TheOne.__ReportTable) == true;
        }

        public void Evaluate(AdocReportCreator adocReportCreator, IElement reportNode, TextWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}