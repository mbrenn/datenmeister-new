using DatenMeister.Core.EMOF.Interface.Reflection;
using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Reports.Generic
{
    public class GenericReportLoop<T> : IGenericReportEvaluator<T> where T : GenericReportCreator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_DatenMeister.TheOne.Reports.Elements.__ReportLoop) == true;
        }
        public void Evaluate(ReportLogic reportLogic, T reportCreator, IElement reportNode)
        {
            var viewNode = reportNode.getOrDefault<IElement>(_DatenMeister._Reports._Elements._ReportLoop.viewNode);
            if (viewNode == null)
            {
                throw new InvalidOperationException(
                    $"The viewNode of the listForm '{NamedElementMethods.GetName(reportNode)}' is null");
            }

            var reportElements =
                reportNode.getOrDefault<IReflectiveCollection>(_DatenMeister._Reports._Elements._ReportLoop.elements);

            var dataviewEvaluation = reportLogic.GetDataViewEvaluation();
            var elements = dataviewEvaluation.GetElementsForViewNode(viewNode);

            foreach (var element in elements.OfType<IElement>())
            {
                foreach (var reportElement in reportElements)
                {
                    // reportCreator.
                }
            }
        }
    }
}
