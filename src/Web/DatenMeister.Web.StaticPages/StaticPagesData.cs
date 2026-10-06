using DatenMeister.Web.StaticPages.HtmlElements;

namespace DatenMeister.Web.StaticPages;

/// <summary>
/// The persistant data
/// </summary>
public class StaticPagesData
{
    /// <summary>
    /// Stores the list of HTML elements that shall be rendered
    /// </summary>
    public List<IHtmlElement> HtmlElements { get; set; } = new();
}