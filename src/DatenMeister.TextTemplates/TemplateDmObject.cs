using DatenMeister.Core.EMOF.Interface.Reflection;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace DatenMeister.TextTemplates;

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

    public override bool TryGetValue(TemplateContext context, SourceSpan span, string member, out object value)
    {
        if (_value == null)
        {
            value = new NullDmObject();
            return true;
        }

        if (member == "_container")
        {
            var asElement = _value as IElement;
            var container = asElement?.container();
            value = container == null ? new NullDmObject() : new TemplateDmObject(container);
            return true;
        }

        if (_value.isSet(member))
        {
            var tempValue = _value.get(member);
            switch (tempValue)
            {
                case IObject o:
                    value = new TemplateDmObject(o);
                    break;
                case null:
                    value = new NullDmObject();
                    break;
                default:
                    value = tempValue;
                    break;
            }

            return true;
        }

        value = new NullDmObject();
        return false;
    }

    public override bool TrySetValue(TemplateContext context, SourceSpan span, string member, object value, bool readOnly)
    {
        _value?.set(member, value);
        return true;
    }
}