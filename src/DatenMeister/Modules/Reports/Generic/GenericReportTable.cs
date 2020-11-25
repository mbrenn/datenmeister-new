using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;

namespace DatenMeister.Modules.Reports.Generic
{
    public class GenericReportTable<T> :
        IGenericReportEvaluator<T> where T : GenericReportCreator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_DatenMeister.TheOne.Reports.__ReportTable) == true;
        }

        public void Evaluate(T genericReportCreator, IElement reportNode)
        {
            throw new System.NotImplementedException();
        }
    }
}