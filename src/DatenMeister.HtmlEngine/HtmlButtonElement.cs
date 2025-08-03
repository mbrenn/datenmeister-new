namespace DatenMeister.HtmlEngine;

/// <summary>
/// Defines a html button
/// </summary>
public class HtmlButtonElement(HtmlElement content) : HtmlElement
{
    /// <summary>
    /// Gets or sets the paragraph to be stored
    /// </summary>
    private HtmlElement Content { get; } = content;

    public override string ToString()
    {
        return $"<button {AttributeString} type=\"button\">{Content}</button>";
    }
}