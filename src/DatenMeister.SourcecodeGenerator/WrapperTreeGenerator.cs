using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
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
        FactoryVersion = new Version(1, 2, 0, 0);
    }

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

        Result.AppendLine($"{stack.Indentation}public class {name}Wrapper(IElement element)");
        Result.AppendLine($"{stack.Indentation}{{");
        Result.AppendLine($"{stack.Indentation}    public IElement GetWrappedElement() => element;");
        Result.AppendLine();
        
        base.WalkClass(classInstance, stack);

        Result.AppendLine($"{stack.Indentation}}}");

        Result.AppendLine();
    }

    protected override void WalkProperty(IObject propertyObject, CallStack stack)
    {
        base.WalkProperty(propertyObject, stack);

        if (propertyObject is not IElement asElement) return;

        var nameAsObject = propertyObject.get("name");
        var name = nameAsObject?.ToString() ?? string.Empty;
        
        var typeOfProperty = PropertyMethods.GetPropertyType(asElement);
        var typeByCsName = "object?";
        var isTyped = true;
        
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
        else if (typeOfProperty?.Equals(Core.Models._PrimitiveTypes.TheOne.__DateTime) == true)
        {
            typeByCsName = "DateTime";
        }
        else
        {
            isTyped = false;
        }
        
        Result.AppendLine($"{stack.Indentation}public {typeByCsName} {name}");
        Result.AppendLine($"{stack.Indentation}{{");
        Result.AppendLine($"{stack.Indentation}    get =>");

        if (isTyped)
        {
            Result.AppendLine($"{stack.Indentation}        element.getOrDefault<{typeByCsName}>(\"{name}\");");
        }
        else
        {
            Result.AppendLine($"{stack.Indentation}        element.get(\"{name}\");");
        }

        
        Result.AppendLine($"{stack.Indentation}    set => ");
        Result.AppendLine($"{stack.Indentation}        element.set(\"{name}\", value);");
        Result.AppendLine($"{stack.Indentation}}}");

        Result.AppendLine();
    }
}