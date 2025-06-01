namespace DatenMeister.HtmlEngine;

public class HtmlDivElement(HtmlElement divElement) : HtmlElement
{
    /// <summary>
    /// Gets or sets the paragraph to be stored
    /// </summary>
    private HtmlElement DivElement { get; } = divElement;

    public override string ToString()
    {
        return $"<div{AttributeString}>{DivElement}</div>";
    }
}