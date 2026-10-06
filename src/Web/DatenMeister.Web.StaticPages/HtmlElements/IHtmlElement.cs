using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.HtmlEngine;

namespace DatenMeister.Web.StaticPages.HtmlElements;

/// <summary>
/// Defines the interface for each html element
/// </summary>
public interface IHtmlElement
{
    /// <summary>
    /// Returns true, if the element's metaclass matches to the implementation of that class
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    bool IsResponsible(IElement element);

    /// <summary>
    /// Implements the generation of html code via htmlReport according the properties
    /// </summary>
    /// <param name="htmlReport">The html report to be used</param>
    /// <param name="element">Element whose html shall be implemented</param>
    void Generate(HtmlReport htmlReport, IElement element);
}
