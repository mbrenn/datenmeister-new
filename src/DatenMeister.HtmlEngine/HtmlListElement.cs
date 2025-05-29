using System.Text;

namespace DatenMeister.HtmlEngine;

/// <summary>
/// Describes an html list element describing a list.
/// An ordered and an non-ordered list is supported by this implementation
/// </summary>
public class HtmlListElement: HtmlElement, IHtmlElementWithMultipleItems
{
    public bool IsOrderedList { get; set; }

    public List<HtmlElement> Items { get; } = new(); 

    private string HtmlTag =>
        IsOrderedList ? "ol" : "ul";

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append($"<{HtmlTag}>");
            
        foreach (var item in Items)
        {
            builder.Append($"<li {AttributeString}>{item}</li>");
        }

        builder.AppendLine($"</{HtmlTag}>");

        return builder.ToString();
    }
}