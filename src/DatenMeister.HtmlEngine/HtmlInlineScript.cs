namespace DatenMeister.HtmlEngine;

/// <summary>
/// Represents an HTML inline script element.
/// This class is used to encapsulate a JavaScript block,
/// rendering it as an inline <script> element.
/// </summary>
public class HtmlInlineScript : HtmlElement
{
    /// <summary>
    /// Represents an HTML inline script element.
    /// </summary>
    /// <remarks>
    /// This class is used to facilitate the creation of an inline JavaScript block,
    /// encapsulated and rendered as a valid <script> element within an HTML document.
    /// </remarks>
    public HtmlInlineScript(string script)
    {
        Script = script;
    }

    /// <summary>
    /// Gets the JavaScript code encapsulated by this script element.
    /// This property holds the raw JavaScript content that will be rendered
    /// within the inline <script> tag.
    /// </summary>
    public string Script { get; }

    /// <summary>
    /// Converts the current instance of the HtmlInlineScript class into its string representation.
    /// </summary>
    /// <returns>
    /// A string containing the HTML representation of the inline script element, including
    /// the encapsulated JavaScript content.
    /// </returns>
    public override string ToString()
    {
        return $"<script>{Script}</script>";
    }
}