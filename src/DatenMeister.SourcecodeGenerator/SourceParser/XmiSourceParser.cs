using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.SourcecodeGenerator.SourceParser;

/// <summary>
/// Parses the raw elements directly imported from the xmi file
/// </summary>
public class XmiSourceParser : ISourceParser
{
    private static bool IsOfType(IObject element, IElement metaClass, string packageName)
    {
        // Checks, if we are having an IElement
        if (element is not IElement asElement
            || asElement.metaclass == null)
        {
            return false;
        }
        
        if(asElement.metaclass.equals(metaClass))
        {
            return true;
        }

        var uri = asElement.metaclass.GetUri();

        if (uri == WorkspaceNames.UriExtentUml + "#" + packageName)
        {
            return true;
        }
        
        if (uri == WorkspaceNames.UriExtentMof + "#" + packageName)
        {
            return true;
        }

        if (uri == WorkspaceNames.StandardUmlNamespace + "#" + packageName)
        {
            return true;
        }
            
        var attributeXmi = "{" + Namespaces.Xmi + "}type";

        return element.isSet(attributeXmi) &&
               element.getOrDefault<string>(attributeXmi) == "uml:" + packageName;
    }
    
    public bool IsPackage(IObject element) 
        => IsOfType(element, _UML.TheOne.Packages.__Package, "Package")
        || (element as IElement)?.metaclass?.GetUri() == "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package";

    public bool IsClass(IObject element)
        => IsOfType(element, _UML.TheOne.StructuredClassifiers.__Class, "Class");

    public bool IsEnum(IObject element)
        => IsOfType(element, _UML.TheOne.SimpleClassifiers.__Enumeration, "Enumeration");

    public bool IsEnumLiteral(IObject element)
        => IsOfType(element, _UML.TheOne.SimpleClassifiers.__EnumerationLiteral, "EnumerationLiteral");

    public bool IsProperty(IObject element)
        => IsOfType(element, _UML.TheOne.Classification.__Property, "Property");

    public bool IsPrimitiveType(IObject element)
        => IsOfType(element, _UML.TheOne.SimpleClassifiers.__PrimitiveType, "PrimitiveType");
}