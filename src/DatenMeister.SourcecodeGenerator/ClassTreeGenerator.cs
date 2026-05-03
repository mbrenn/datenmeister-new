using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourcecodeGenerator;

/// <summary>
///     Creates a class file which contains objects for all
///     items within the given extent.
///     It parses classes and packages.
/// </summary>
public class ClassTreeGenerator : WalkPackageClass
{
    /// <summary>
    /// Stores the typeUriMappingLogic which updates the references during the build
    /// </summary>
    public TypeUriMappingLogic? UriMappingLogic { get; init; }

    /// <summary>
    ///     Initializes a new instance of the ClassTreeGenerator
    /// </summary>
    public ClassTreeGenerator(ISourceParser? parser = null) : base(parser)
    {
        FactoryVersion = new Version(1, 3, 0, 0);
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
            "DatenMeister.Core.Interfaces",
            "DatenMeister.Core.Interfaces.MOF.Reflection"
        ]);
            
        WriteResharperComments();

        base.Walk(extent);
    }

    /// <summary>
    ///     Parses the packages and creates the C# Code for all the
    ///     packages by recursive calls to itself for packages and
    ///     ParseClasses for classes.
    /// </summary>
    /// <param name="element">Element being parsed</param>
    /// <param name="stack">Callstack being used</param>
    protected override void WalkPackage(IObject element, CallStack stack)
    {
        var name = GetNameOfElement(element);

        Result.AppendLine($"{stack.Indentation}public class _{name}");
        Result.AppendLine($"{stack.Indentation}{{");

        var innerStack = new CallStack(stack)
        {
            Fullname = string.IsNullOrEmpty(stack.Fullname) ? name : $"{stack.Fullname}.{name}"
        };

        base.WalkPackage(element, stack);

        if (stack.Level == 0)
        {
            Result.AppendLine($"{innerStack.Indentation}public static readonly _{name} TheOne = new ();");
            Result.AppendLine();
        }

        Result.AppendLine($"{stack.Indentation}}}");

        Result.AppendLine();

        if (stack.Level > 0)
        {
            Result.AppendLine($"{stack.Indentation}public _{name} {name} = new ();");
            Result.AppendLine();
        }
    }

    /// <summary>
    /// Tries to get the class name as defined in the class model.
    /// This is a bit ugly re-engineering of the way how this method is working, but it will be working 
    /// </summary>
    /// <param name="classInstance">Instance of the class.</param>
    /// <returns></returns>
    public string GetFullClassName(IObject classInstance)
    {
        return  Namespace + "._" + NamedElementMethods.GetFullName(classInstance, "._");
    }
    
    /// <summary>
    /// Tries to get the class name as defined in the class model.
    /// This is a bit ugly re-engineering of the way how this method is working, but it will be working 
    /// </summary>
    /// <param name="classInstance">Instance of the class.</param>
    /// <returns></returns>
    public string GetWrapperFullClassName(IObject classInstance)
    {
        return Namespace + "." + NamedElementMethods.GetFullName(classInstance, ".") + "_Wrapper";
    }

    /// <summary>
    ///     Parses the packages
    /// </summary>
    /// <param name="classInstance">The class that shall be retrieved</param>
    /// <param name="stack">Stack being used to walk through</param>
    protected override void WalkClass(IObject classInstance, CallStack stack)
    {
        if (classInstance is not IElement asElement) return;

        var name = GetNameOfElement(classInstance);
        UpdateEntry(classInstance);
        
        Result.AppendLine($"{stack.Indentation}[TypeUri(Uri = \"{classInstance.GetUri()}\",");
        Result.AppendLine($"{stack.Indentation}    TypeKind = TypeKind.ClassTree)]");
        Result.AppendLine($"{stack.Indentation}public class _{name}");
        Result.AppendLine($"{stack.Indentation}{{");

        base.WalkClass(classInstance, stack);

        Result.AppendLine($"{stack.Indentation}}}");

        Result.AppendLine();
        Result.AppendLine($"{stack.Indentation}public _{name} @{name} = new ();");
        Result.AppendLine($"{stack.Indentation}public MofObjectShadow @__{name} = new (\"{asElement.GetUri()}\");");
        Result.AppendLine();
    }

    protected override void WalkProperty(IObject propertyObject, CallStack stack)
    {
        base.WalkProperty(propertyObject, stack);

        if (propertyObject is not IElement asElement) return;

        var nameAsObject = propertyObject.get("name");
        var name = nameAsObject?.ToString() ?? string.Empty;
        Result.AppendLine($"{stack.Indentation}public static readonly string @{name} = \"{name}\";");

        var id = (asElement as IHasId)?.Id ?? string.Empty;
        if (DotNetHelper.IsGuid(id))
        {
            Result.AppendLine(
                $"{stack.Indentation}public IElement? @_{name} = null;");
        }
        else
        {
            Result.AppendLine(
                $"{stack.Indentation}public IElement @_{name} = new MofObjectShadow(\"{asElement.GetUri() ?? string.Empty}\");");
        }

        Result.AppendLine();
    }

    /// <summary>
    ///     Parses the packages
    /// </summary>
    /// <param name="enumInstance">The class that shall be retrieved</param>
    /// <param name="stack">Stack being used</param>
    /// <param name="callee">The element to be called when tn enumeration is required to be called.
    /// Maybe null, then WalkEnumLiteral will be called</param>
    protected override void WalkEnum(IObject enumInstance, CallStack stack, Action<IObject, CallStack>? callee = null)
    {
        var asElement = (IElement) enumInstance;
        var name = GetNameOfElement(enumInstance);
        UpdateEntry(enumInstance);

        Result.AppendLine($"{stack.Indentation}public class _{name}");
        Result.AppendLine($"{stack.Indentation}{{");

        base.WalkEnum(enumInstance, stack, callee);

        Result.AppendLine();
        Result.AppendLine($"{stack.Indentation}}}");
        Result.AppendLine();

        Result.AppendLine($"{stack.Indentation}public _{name} @{name} = new _{name}();");
        Result.AppendLine($"{stack.Indentation}public IElement @__{name} = new MofObjectShadow(\"{asElement.GetUri()}\");");
        Result.AppendLine();
        Result.AppendLine();
            
        Result.AppendLine($"{stack.Indentation}public enum ___{name}");
        Result.AppendLine($"{stack.Indentation}{{");

        var first = true;

        base.WalkEnum(enumInstance, stack, (literal, innerStack) =>
        {
            if (!first)
            {
                Result.AppendLine(",");
            }
                
            var nameAsObject = literal.get("name");
            var literalName = nameAsObject == null ? string.Empty : nameAsObject.ToString();

            Result.Append($"{innerStack.Indentation}@{literalName}");
            first = false;
        });

        Result.AppendLine();
        Result.AppendLine($"{stack.Indentation}}}");
        Result.AppendLine();
    }

    private void UpdateEntry(IObject enumInstance)
    {
        var fullName = GetFullClassName(enumInstance);
        var wrapperName = GetWrapperFullClassName(enumInstance);
        UriMappingLogic?.UpdateEntry(enumInstance.GetUri(), fullName, TypeKind.ClassTree);
        UriMappingLogic?.UpdateEntry(enumInstance.GetUri(), wrapperName, TypeKind.WrappedClass);
    }

    protected override void WalkEnumLiteral(IObject enumLiteralObject, CallStack stack)
    {
        var asElement = enumLiteralObject as IElement;
        base.WalkEnumLiteral(enumLiteralObject, stack);

        var nameAsObject = enumLiteralObject.get("name");
        var name = nameAsObject == null ? string.Empty : nameAsObject.ToString();
            
        Result.AppendLine($"{stack.Indentation}public static string @{name} = \"{name}\";");

        var id = (asElement as IHasId)?.Id ?? string.Empty;
        if (DotNetHelper.IsGuid(id))
        {
            Result.AppendLine(
                $"{stack.Indentation}public IElement? @__{name} = null;");
        }        
        else
        {
            Result.AppendLine(
                $"{stack.Indentation}public IElement @__{name} = new MofObjectShadow(\"{asElement?.GetUri() ?? string.Empty}\");");
        }
    }
}