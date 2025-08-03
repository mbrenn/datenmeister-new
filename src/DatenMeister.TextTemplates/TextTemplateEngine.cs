using DatenMeister.Core.EMOF.Interface.Reflection;
using Scriban;
using Scriban.Runtime;

namespace DatenMeister.TextTemplates;

/// <summary>
/// Defines the text template engine which is capable to parse the texts
/// </summary>
public static class TextTemplateEngine
{
    /// <summary>
    /// Parses the given text in the content of the given element
    /// </summary>
    /// <param name="text">Text to be parsed</param>
    /// <param name="additionalObjects">Additional objects that can be added to the parser
    /// DatenMeister Objects are converted to the appropriate object type</param>
    /// <returns>The parsed element</returns>
    public static string Parse(string text, Dictionary<string, object>? additionalObjects = null)
    {
        var template = Template.Parse(text);

        var templateContext = new TemplateContext();
        var scriptObject = new ScriptObject();

        // Adds the elements to the parser
        if (additionalObjects != null)
        {
            foreach (var pair in additionalObjects)
            {
                var value = pair.Value;
                if (value is IObject asObject)
                {
                    value = new TemplateDmObject(asObject);
                }

                scriptObject[pair.Key] = value;
            }
        }

        templateContext.MemberRenamer = member => member.Name;
        templateContext.PushGlobal(scriptObject);

        return template.Render(templateContext);
    }
}