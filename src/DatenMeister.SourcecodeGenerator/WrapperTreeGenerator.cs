using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourcecodeGenerator;

public class WrapperTreeGenerator : WalkPackageClass    
{
    /// <summary>
    ///     Initializes a new instance of the ClassTreeGenerator
    /// </summary>
    public WrapperTreeGenerator(ISourceParser? parser = null) : base(parser)
    {
        FactoryVersion = new Version(1, 3, 0, 0);
    }

    public required TypeUriMappingLogic TypeUriMapping { get; set; }

    /// <summary>
    ///     Creates a C# source code. Not to be used for recursive
    ///     call since the namespace is just once created
    /// </summary>
    /// <param name="extent">
    ///     Regards the given element as a package
    ///     and returns a full namespace for the package.
    /// </param>
    public override void Walk(IUriExtent extent)
    {
        WriteUsages([
            "DatenMeister.Core.EMOF.Interface.Reflection",
            "DatenMeister.Core.Helper"
        ]);
        
        WriteResharperComments();

        base.Walk(extent);
    }

    protected override void WalkPackage(IObject element, CallStack stack)
    {
        var name = GetNameOfElement(element);

        Result.AppendLine($"{stack.Indentation}public class {name}");
        Result.AppendLine($"{stack.Indentation}{{");

        base.WalkPackage(element, stack);

        Result.AppendLine($"{stack.Indentation}}}");

        Result.AppendLine();
    }

    /// <summary>
    ///     Parses the packages
    /// </summary>
    /// <param name="classInstance">The class that shall be retrieved</param>
    /// <param name="stack">Stack being used to walk through</param>
    protected override void WalkClass(IObject classInstance, CallStack stack)
    {
        if (classInstance is not IElement) return;

        var name = GetNameOfElement(classInstance);

        Result.AppendLine($"{stack.Indentation}[TypeUri(Uri = \"{classInstance.GetUri()}\",");
        Result.AppendLine($"{stack.Indentation}    TypeKind = TypeKind.WrappedClass)]");
        Result.AppendLine($"{stack.Indentation}public class {name}_Wrapper(IElement innerDmElement) : IElementWrapper");
        Result.AppendLine($"{stack.Indentation}{{");
        Result.AppendLine($"{stack.Indentation}    public IElement GetWrappedElement() => innerDmElement;");
        Result.AppendLine();
        
        base.WalkClass(classInstance, stack);

        Result.AppendLine($"{stack.Indentation}}}");

        Result.AppendLine();
    }

    private enum TypingClassification
    {
        Unknown,
        Primitive, 
        Class
    }

    protected override void WalkProperty(IObject propertyObject, CallStack stack)
    {
        base.WalkProperty(propertyObject, stack);

        if (propertyObject is not IElement asElement) return;

        var nameAsObject = propertyObject.get("name");
        var name = nameAsObject?.ToString() ?? string.Empty;

        var typeOfProperty = PropertyMethods.GetPropertyType(asElement);
        var typeByCsName = "object?";
        var isTyped = TypingClassification.Primitive;

        if (typeOfProperty?.Equals(_PrimitiveTypes.TheOne.__String) == true)
        {
            typeByCsName = "string";
        }
        else if (typeOfProperty?.Equals(_PrimitiveTypes.TheOne.__Boolean) == true)
        {
            typeByCsName = "bool";
        }
        else if (typeOfProperty?.Equals(_PrimitiveTypes.TheOne.__Real) == true)
        {
            typeByCsName = "double";
        }
        else if (typeOfProperty?.Equals(_PrimitiveTypes.TheOne.__UnlimitedNatural) == true
                 || typeOfProperty?.Equals(_PrimitiveTypes.TheOne.__Integer) == true)
        {
            typeByCsName = "int";
        }
        else if (typeOfProperty?.Equals(DatenMeister.Core.Models._CommonTypes.TheOne.__DateTime) == true)
        {
            typeByCsName = "DateTime";
        }
        else
        {
            // Check, if we know the type
            var foundTypes = TypeUriMapping.Entries.Where(x => x.TypeUri == typeOfProperty?.GetUri()
                                                               && x.TypeKind == TypeKind.WrappedClass).ToList();
            if (foundTypes.Count > 1)
            {
                throw new InvalidOperationException("We have an issue. We found more than one match! ");
            }

            // Now, whether we found a type, we will create different styles of source code
            var foundType = foundTypes.FirstOrDefault();
            if (foundType != null)
            {
                Result.AppendLine($"{stack.Indentation}// {foundType.ClassFullName}");
                isTyped = TypingClassification.Class;
            }
            else
            {
                Result.AppendLine($"{stack.Indentation}// Not found");
                isTyped = TypingClassification.Unknown;
            }


        }

        Result.AppendLine($"{stack.Indentation}public {typeByCsName} @{name}");
        Result.AppendLine($"{stack.Indentation}{{");
        

        switch (isTyped)
        {
            case TypingClassification.Unknown:
                Result.AppendLine($"{stack.Indentation}    get =>");
                Result.AppendLine($"{stack.Indentation}        innerDmElement.get(\"{name}\");");
                break;
            case TypingClassification.Primitive:
                Result.AppendLine($"{stack.Indentation}    get =>");
                Result.AppendLine(
                    $"{stack.Indentation}        innerDmElement.getOrDefault<{typeByCsName}>(\"{name}\");");
                break;
            case TypingClassification.Class:
                Result.AppendLine($"{stack.Indentation}    get");
                Result.AppendLine($"{stack.Indentation}    {{");
                Result.AppendLine($"{stack.Indentation}        var foundElement = innerDmElement.getOrDefault<IElement>(\"{name}\");");
                Result.AppendLine($"{stack.Indentation}        return foundElement == null ? null : new {typeByCsName}(foundElement);");
                Result.AppendLine($"{stack.Indentation}    }}");
                break;
            default:
                throw new ArgumentOutOfRangeException(isTyped.ToString());
        }

        switch (isTyped)
        {
            case TypingClassification.Unknown:
                Result.AppendLine($"{stack.Indentation}    set => ");
                Result.AppendLine($"{stack.Indentation}        innerDmElement.set(\"{name}\", value);");
                break;
            case TypingClassification.Primitive:
                Result.AppendLine($"{stack.Indentation}    set => ");
                Result.AppendLine($"{stack.Indentation}        innerDmElement.set(\"{name}\", value);");
                break;
            case TypingClassification.Class:
                Result.AppendLine($"{stack.Indentation}    set ");
                Result.AppendLine($"{stack.Indentation}    {{");
                Result.AppendLine($"{stack.Indentation}        if(value is IWrappedElement wrappedElement)");
                Result.AppendLine($"{stack.Indentation}        {{");
                Result.AppendLine($"{stack.Indentation}            innerDmElement.set(\"{name}\", wrappedElement.GetWrappedElement());");
                Result.AppendLine($"{stack.Indentation}        }}");
                Result.AppendLine($"{stack.Indentation}        else");
                Result.AppendLine($"{stack.Indentation}        {{");
                Result.AppendLine($"{stack.Indentation}            innerDmElement.set(\"{name}\", value);");
                Result.AppendLine($"{stack.Indentation}        }}");
                Result.AppendLine($"{stack.Indentation}    }}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Result.AppendLine($"{stack.Indentation}}}");

        Result.AppendLine();
    }
}