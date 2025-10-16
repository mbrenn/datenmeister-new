using DatenMeister.Core.Interfaces.MOF.Reflection;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace DatenMeister.TextTemplates;

/// <summary>
/// Defines the abstraction for the template engine.
/// It provides the properties of the datenmeister objects to
/// the Template Engine
/// </summary>
public class TemplateDmObject(IObject? value) : ScriptObject
{
    public override bool TryGetValue(TemplateContext context, SourceSpan span, string member, out object value1)
    {
        if (value == null)
        {
            value1 = new NullDmObject();
            return true;
        }

        if (member == "_container")
        {
            var asElement = value as IElement;
            var container = asElement?.container();
            value1 = container == null ? new NullDmObject() : new TemplateDmObject(container);
            return true;
        }

        if (value.isSet(member))
        {
            var tempValue = value.get(member);
            switch (tempValue)
            {
                case IObject o:
                    value1 = new TemplateDmObject(o);
                    break;
                case null:
                    value1 = new NullDmObject();
                    break;
                default:
                    value1 = tempValue;
                    break;
            }

            return true;
        }

        value1 = new NullDmObject();
        return false;
    }

    public override bool TrySetValue(TemplateContext context, SourceSpan span, string member, object value1, bool readOnly)
    {
        value?.set(member, value1);
        return true;
    }
}