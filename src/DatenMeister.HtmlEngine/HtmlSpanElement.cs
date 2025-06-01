namespace DatenMeister.HtmlEngine;

public class HtmlSpanElement(HtmlElement content) : HtmlElement
{
    /// <summary>
    /// Gets or sets the paragraph to be stored
    /// </summary>
    private HtmlElement Content { get; } = content;

    public override string ToString()
    {
        return $"<span{AttributeString}>{Content}</span>";
    }
}