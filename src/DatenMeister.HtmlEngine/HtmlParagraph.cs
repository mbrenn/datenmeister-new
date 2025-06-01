namespace DatenMeister.HtmlEngine;

public class HtmlParagraph(HtmlElement paragraph) : HtmlElement
{
    /// <summary>
    /// Gets or sets the paragraph to be stored
    /// </summary>
    private HtmlElement Paragraph { get; } = paragraph;

    public override string ToString()
    {
        return $"<p{AttributeString}>{Paragraph}</p>";
    }
}