using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.Xmi;

namespace DatenMeister.SourcecodeGenerator.SourceParser;

/// <summary>
/// Parses the raw elements directly imported from the xmi file
/// </summary>
public class XmiSourceParser : ISourceParser
{
    public bool IsPackage(IObject element)
    {
        // Checks, if we are having an IElement
        if (element is IElement asElement && asElement.metaclass?.equals(
                _UML.TheOne.Packages.__Package) == true)
        {
            return true;
        }
            
        var attributeXmi = "{" + Namespaces.Xmi + "}type";

        return element.isSet(attributeXmi) &&
               element.getOrDefault<string>(attributeXmi) == "uml:Package";
    }

    public bool IsClass(IObject element)
    {
        // Checks, if we are having an IElement
        if (element is IElement asElement && asElement.metaclass?.equals(
                _UML.TheOne.StructuredClassifiers.__Class) == true)
        {
            return true;
        }
            
        var attributeXmi = "{" + Namespaces.Xmi + "}type";

        return element.isSet(attributeXmi) &&
               element.getOrDefault<string>(attributeXmi) == "uml:Class";
    }

    public bool IsEnum(IObject element)
    {
        // Checks, if we are having an IElement
        if (element is IElement asElement && asElement.metaclass?.equals(
                _UML.TheOne.SimpleClassifiers.__Enumeration) == true)
        {
            return true;
        }
            
        var attributeXmi = "{" + Namespaces.Xmi + "}type";

        return element.isSet(attributeXmi) &&
               element.getOrDefault<string>(attributeXmi) == "uml:Enumeration";
    }

    public bool IsEnumLiteral(IObject element)
    {
        // Checks, if we are having an IElement
        if (element is IElement asElement && asElement.metaclass?.equals(
                _UML.TheOne.SimpleClassifiers.__EnumerationLiteral) == true)
        {
            return true;
        }
            
        var attributeXmi = "{" + Namespaces.Xmi + "}type";

        return element.isSet(attributeXmi) &&
               element.getOrDefault<string>(attributeXmi) == "uml:EnumerationLiteral";
    }

    public bool IsProperty(IObject element)
    {
        // Checks, if we are having an IElement
        if (element is IElement asElement && asElement.metaclass?.equals(
                _UML.TheOne.Classification.__Property) == true)
        {
            return true;
        }
            
        var attributeXmi = "{" + Namespaces.Xmi + "}type";

        return element.isSet(attributeXmi) &&
               element.getOrDefault<string>(attributeXmi) == "uml:Property";
    }

    public bool IsPrimitiveType(IObject element)
    {
        // Checks, if we are having an IElement
        if (element is IElement asElement && asElement.metaclass?.equals(
                _UML.TheOne.SimpleClassifiers.__PrimitiveType) == true)
        {
            return true;
        }
            
        var attributeXmi = "{" + Namespaces.Xmi + "}type";

        return element.isSet(attributeXmi) &&
               element.getOrDefault<string>(attributeXmi) == "uml:PrimitiveType";
    }
}