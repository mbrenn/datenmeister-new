using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Reports.Generic;

public class GenericReportLoop<T> : IGenericReportEvaluator<T> where T : GenericReportCreator
{
    public bool IsRelevant(IElement element)
    {
        var metaClass = element.getMetaClass();
        return metaClass?.equals(_Reports.TheOne.Elements.__ReportLoop) == true;
    }
    public void Evaluate(ReportLogic reportLogic, T reportCreator, IElement reportNode)
    {
        var viewNode =
            ReportLogic.GetViewNode(
                reportNode,
                _Reports._Elements._ReportLoop.viewNode);
        if (viewNode == null)
        {
            throw new InvalidOperationException(
                $"The viewNode of the listForm '{NamedElementMethods.GetName(reportNode)}' is null");
        }

        var reportElements =
            reportNode.getOrDefault<IReflectiveCollection>(_Reports._Elements._ReportLoop.elements);

        var dataviewEvaluation = reportLogic.GetDataViewEvaluation();
        var elements = dataviewEvaluation.GetElementsForViewNode(viewNode);

        foreach (var element in elements.OfType<IElement>())
        {
            var sources = reportLogic.PushSources();
            reportLogic.AddSource("item", new TemporaryReflectiveCollection([element]));
            reportLogic.ReportCreator.EvaluateElements(reportLogic, reportElements);
            reportLogic.PopSources(sources);
        }
    }
}