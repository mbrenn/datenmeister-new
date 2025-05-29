namespace DatenMeister.HtmlEngine;

/// <summary>
/// Defines a html button
/// </summary>
public class HtmlButtonElement : HtmlElement
{
    /// <summary>
    /// Gets or sets the paragraph to be stored
    /// </summary>
    private HtmlElement Content { get; }

    public HtmlButtonElement(HtmlElement content)
    {
        Content = content;
    }

    public override string ToString()
    {
        return $"<button {AttributeString} type=\"button\">{Content}</button>";
    }
}