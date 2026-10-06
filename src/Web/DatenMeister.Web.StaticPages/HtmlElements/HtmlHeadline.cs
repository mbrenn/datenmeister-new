using BurnSystems.Logging;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.HtmlEngine;

namespace DatenMeister.Web.StaticPages.HtmlElements;

public class HtmlHeadline : IHtmlElement
{
    /// <summary>
    /// Stores the logger for this class
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(HtmlHeadline));

    public bool IsResponsible(IElement element)
    {
        return element.getMetaClass()?.equals(
            _Reports.TheOne.Elements.__ReportHeadline) == true;
    }

    public void Generate(HtmlReport htmlReport, IElement element)
    {
        var headline = element.getOrDefault<string>(_Reports._Elements._ReportHeadline.title);
        Logger.Debug($"Generating headline '{headline}'");
        htmlReport.Add(new DatenMeister.HtmlEngine.HtmlHeadline(headline, 1));
    }
}