using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Reports.Generic;

public abstract class GenericReportHeadline<T> :
    IGenericReportEvaluator<T> where T : GenericReportCreator
{
    public bool IsRelevant(IElement element)
    {
        var metaClass = element.getMetaClass();
        return metaClass?.equals(_Reports.TheOne.Elements.__ReportHeadline) == true;
    }

    /// <summary>
    /// Performs the evaluation
    /// </summary>
    /// <param name="reportLogic">The report logic</param>
    /// <param name="reportCreator">Report reportCreator</param>
    /// <param name="reportNode">The report node</param>
    public void Evaluate(ReportLogic reportLogic, T reportCreator, IElement reportNode)
    {
        var headline = reportNode.getOrDefault<string>(_Reports._Elements._ReportHeadline.title);
        WriteHeadline(reportCreator, headline);
    }

    /// <summary>
    /// Writes the headline
    /// </summary>
    /// <param name="reportCreator">Creates the report</param>
    /// <param name="headline">Headline to be used</param>
    public abstract void WriteHeadline(T reportCreator, string headline);
}