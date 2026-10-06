using BurnSystems;
using BurnSystems.Logging;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.HtmlEngine;

namespace DatenMeister.Web.StaticPages.HtmlElements;

public class HtmlParagraph : IHtmlElement
{
    /// <summary>
    /// Stores the logger for this class
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(HtmlParagraph));
    
    public bool IsResponsible(IElement element)
    {
        return element.getMetaClass()?.equals(
            _Reports.TheOne.Elements.__ReportParagraph) == true;
    }

    public void Generate(HtmlReport htmlReport, IElement element)
    {
        var paragraph = element.getOrDefault<string>(_Reports._Elements._ReportParagraph.paragraph);
        var cssClass = element.getOrDefault<string>(_Reports._Elements._ReportParagraph.cssClass);

        var htmlParagraph = new DatenMeister.HtmlEngine.HtmlParagraph(paragraph);
        if (!string.IsNullOrEmpty(cssClass))
        {
            htmlParagraph.CssClass = cssClass;
        }

        Logger.Debug($"Generating paragraph: {paragraph.ShortenString(20)}");
        htmlReport.Add(htmlParagraph);
    }
}