﻿using DatenMeister.Core.EMOF.Interface.Reflection;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace DatenMeister.Modules.TextTemplates
{
    /// <summary>
    /// Defines the text template engine which is capable to parse the texts
    /// </summary>
    public static class TextTemplateEngine
    {
        /// <summary>
        /// Parses the given text in the content of the given element
        /// </summary>
        /// <param name="element">Element to be used</param>
        /// <param name="text">Text to be parsed</param>
        /// <returns>The parsed element</returns>
        public static string Parse(IObject? element, string text)
        {
            var template = Template.Parse(text);

            var templateContext = new TemplateContext();
            var scriptObject = new ScriptObject();
            scriptObject["i"] = new TemplateDmObject(element);

            templateContext.MemberRenamer = member => member.Name;
            templateContext.PushGlobal(scriptObject);

            return template.Render(templateContext);
        }
    }

    public class NullDmObject : ScriptObject
    {
        public override string ToString(TemplateContext context, SourceSpan span)
        {
            return string.Empty;
        }

        public override bool TryGetValue(TemplateContext context, SourceSpan span, string member, out object value)
        {
            value = new NullDmObject();
            return true;
        }
    }

    /// <summary>
    /// Defines the abstraction for the template engine.
    /// It provides the properties of the datenmeister objects to
    /// the Template Engine
    /// </summary>
    public class TemplateDmObject : ScriptObject
    {
        private readonly IObject? _value;

        public TemplateDmObject(IObject? value)
        {
            _value = value;
        }

        public override bool TryGetValue(TemplateContext context, SourceSpan span, string member, out object? value)
        {
            if (_value == null)
            {
                value = new NullDmObject();
                return true;
            }

            if (_value.isSet(member))
            {
                value = _value.get(member);
                switch (value)
                {
                    case IObject o:
                        value = new TemplateDmObject(o);
                        break;
                    case null:
                        value = new NullDmObject();
                        break;
                }

                return true;
            }

            value = new NullDmObject();
            return false;
        }
    }
}