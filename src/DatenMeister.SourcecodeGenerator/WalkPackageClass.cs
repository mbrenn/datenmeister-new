using System.Text;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Provider.Xmi.Provider.XMI.UmlBootstrap;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourcecodeGenerator;

/// <summary>
///     Creates a class tree out of an XML which can be used to fill the appropriate instance
/// </summary>
public class WalkPackageClass
{
    protected Version FactoryVersion = new(1, 0, 1, 0);

    /// <summary>
    /// Stores the source parser to find out the elements and attributes
    /// </summary>
    private readonly ISourceParser _parser;

    /// <summary>
    /// Initializates a new instance of the WalkPackageClass instance
    /// </summary>
    /// <param name="parser"></param>
    public WalkPackageClass(ISourceParser? parser)
    {
        _parser = parser ?? new XmiSourceParser();
        Result = new StringBuilder();
    }

    /// <summary>
    ///     Gets or sets the namespace
    /// </summary>
    public string? Namespace { get; set; }

    /// <summary>
    ///     Gets or sets the result being delivered back
    /// </summary>
    public StringBuilder Result { get; set; }

    public int PackagesWalked { get; set; }
    
    public int ClassesWalked { get; set; }
    
    public int PropertiesWalked { get; set; }
    
    public int EnumWalked { get; set; }
    
    public int LiteralWalked { get; set; }

    public int TotalWalked 
        => PackagesWalked + ClassesWalked+ PropertiesWalked + EnumWalked + LiteralWalked;

    /// <summary>
    ///     Creates a C# class instance for all the packages and classes within the extent
    /// </summary>
    /// <param name="extent">Extent to be used</param>
    public virtual void Walk(IUriExtent extent)
    {
        var stack = new CallStack(null);

        StartNamespace(ref stack);

        // First, get all the packages of the extent
        foreach (var element in extent.elements().OfType<IObject>().Where(x => _parser.IsPackage(x)))
        {
            Walk(element, stack);
        }

        // For all the other elements, we will need to create a helper class because a method cannot be contained
        // directly within a namespace
        var nonPackageElements = extent.elements().OfType<IObject>().Where(
            x => !_parser.IsPackage(x) && _parser.IsClass(x)).ToList();
        if (nonPackageElements.Count != 0)
        {
            var packageInstance = MofFactory.CreateByExtent(extent).create(_UML.TheOne.Packages.__Package);
            packageInstance.set(_UML._Packages._Package.name, "Root");
            packageInstance.set(_UML._Packages._Package.packagedElement, nonPackageElements);

            Walk(packageInstance, stack);
        }

        EndNamespace(ref stack);
    }

    /// <summary>
    ///     Creates a C# source code. Not to be used for recursive
    ///     call since the namespace is just once created
    /// </summary>
    /// <param name="element">
    ///     Regards the given element as a package
    ///     and returns a full namespace for the package.
    /// </param>
    /// <param name="stack">Current callstack</param>
    private void Walk(IObject element, CallStack stack)
    {
        if (_parser.IsPackage(element))
        {
            WalkPackage(element, stack);
            PackagesWalked++;
        }

        if (_parser.IsClass(element))
        {
            WalkClass(element, stack);
            ClassesWalked++;
        }

        if (_parser.IsEnum(element))
        {
            WalkEnum(element, stack);
            EnumWalked++;
        }
    }

    /// <summary>
    /// Writes the usage declaration for each element as given in the namespaces parameter
    /// </summary>
    /// <param name="namespaces">Namespaces to be used within the file</param>
    protected void WriteUsages(IEnumerable<string> namespaces)
    {
        if (namespaces == null)
        {
            throw new ArgumentNullException(nameof(namespaces));
        }

        foreach (var space in namespaces)
        {
            Result.AppendLine($"using {space};");
        }

        Result.AppendLine();
    }

    protected void WriteResharperComments()
    {
        Result.AppendLine("// ReSharper disable InconsistentNaming");
        Result.AppendLine("// ReSharper disable RedundantNameQualifier");
    }

    /// <summary>
    ///     Creates a C# source code. Not to be used for recursive
    ///     call since the namespace is just once created
    /// </summary>
    /// <param name="stack">
    ///     Regards the given element as a package
    ///     and returns a full namespace for the package.
    /// </param>
    private void StartNamespace(ref CallStack stack)
    {
        Result.AppendLine($"{stack.Indentation}// Created by {GetType().FullName} Version {FactoryVersion}");

        // Check, if we have namespaces
        if (string.IsNullOrEmpty(Namespace))
        {
            return;
        }
        
        var indentation = stack.Indentation;
        Result.AppendLine($"{indentation}namespace {Namespace};");
        Result.AppendLine();
    }

    /// <summary>
    ///     Creates a C# source code. Not to be used for recursive
    ///     call since the namespace is just once created
    /// </summary>
    /// <param name="stack">
    /// Defines the stack for tjhe given
    ///     Regards the given element as a package
    ///     and returns a full namespace for the package.
    /// </param>
    private void EndNamespace(ref CallStack stack)
    {
        // Nothing to do, we are using flatline C# 9 namespaces
    }

    /// <summary>
    ///     Parses the packages and creates the C# Code for all the
    ///     packages by recursive calls to itself for packages and
    ///     ParseClasses for classes.
    /// </summary>
    /// <param name="element">Element being parsed</param>
    /// <param name="stack">The callstack containing information about depth and full names</param>
    protected virtual void WalkPackage(IObject element, CallStack stack)
    {
        var innerStack = new CallStack(stack);
        var name = GetNameOfElement(element);
        if (stack.Level == 0)
        {
            innerStack.Fullname = string.Empty;
        }
        else
        {
            innerStack.Fullname =
                string.IsNullOrEmpty(innerStack.Fullname) ? $".{name}" : $"{innerStack.Fullname}.{name}";
        }

        // Finds the subpackages and classes
        foreach (var subElement in Helper.GetSubProperties(element))
        {
            if (_parser.IsPackage(subElement))
            {
                WalkPackage(subElement, innerStack);
                PackagesWalked++;
            }

            if (_parser.IsClass(subElement) || _parser.IsPrimitiveType(subElement))
            {
                WalkClass(subElement, innerStack);
                ClassesWalked++;
            }

            if (_parser.IsEnum(subElement))
            {
                WalkEnum(subElement, innerStack);
                EnumWalked++;
            }
        }
    }

    /// <summary>
    ///     Parses the packages
    /// </summary>
    /// <param name="classInstance">The classes that need to be parsed</param>
    /// <param name="stack">Stack to be used</param>
    protected virtual void WalkClass(IObject classInstance, CallStack stack)
    {
        var innerStack = new CallStack(stack);
        var name = GetNameOfElement(classInstance);
        innerStack.Fullname += $".{name}";

        /*
        // Needs to be updated
        foreach (var propertyObject in Helper.GetSubProperties(classInstance))
        {
            if (_parser.IsProperty(propertyObject))
            {
                WalkProperty(propertyObject, innerStack);
            }
        }*/

        foreach (var propertyObject in ClassifierMethods.GetPropertiesOfClassifier(classInstance))
        {
            if (_parser.IsProperty(propertyObject))
            {
                WalkProperty(propertyObject, innerStack);
                PropertiesWalked++;
            }
        }
    }

    protected virtual void WalkProperty(IObject classInstance, CallStack stack)
    {
    }


    /// <summary>
    /// Walks the enumeration
    /// </summary>
    /// <param name="enumInstance">Enumeration to be walked</param>
    /// <param name="stack">Stack being used</param>
    /// <param name="callee">The element to be called when tn enumeration is required to be called.
    /// May be null, then WalkEnumLiteral will be called</param>
    protected virtual void WalkEnum(IObject enumInstance, CallStack stack, Action<IObject, CallStack>? callee = null)
    {
        var innerStack = new CallStack(stack);
        var name = GetNameOfElement(enumInstance);
        innerStack.Fullname += $".{name}";

        // Needs to be updated
        foreach (var enumLiteral in enumInstance.GetAsEnumerable(_UML._SimpleClassifiers._Enumeration.ownedLiteral)
                     .OfType<IElement>())
        {
            if (_parser.IsEnumLiteral(enumLiteral))
            {
                if (callee != null)
                {
                    callee(enumLiteral, innerStack);
                }
                else
                {
                    WalkEnumLiteral(enumLiteral, innerStack);
                    LiteralWalked++;
                }
            }
        }
    }

    /// <summary>
    /// Walks the enumeration literal
    /// </summary>
    /// <param name="enumLiteral">Enumeration Literal to be walked</param>
    /// <param name="innerStack">Stack being used</param>
    protected virtual void WalkEnumLiteral(IObject enumLiteral, CallStack innerStack)
    {
    }

    protected static string GetNameOfElement(IObject element)
    {
        var nameAsObject = element.get("name");
        var name = nameAsObject?.ToString() ?? string.Empty;
        return name;
    }

    /// <summary>
    ///     Defines the callstack
    /// </summary>
    public class CallStack
    {
        /// <summary>
        /// Initializes a new instance of the callback
        /// </summary>
        /// <param name="ownerStack">The owning callstack. It may be null, if this is the root
        /// call stack</param>
        public CallStack(CallStack? ownerStack)
        {
            Owner = ownerStack;
            Indentation = ownerStack == null ? string.Empty : $"{ownerStack.NextIndentation}";
            Level = ownerStack?.Level + 1 ?? 0;
            Fullname = ownerStack?.Fullname ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the current level of the call stack
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the full name of the call stack
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Gets or sets the indentation, which is just some space
        /// </summary>
        public string Indentation { get; set; }

        /// <summary>
        ///     Stores the owner stack
        /// </summary>
        public CallStack? Owner { get; set; }

        /// <summary>
        /// Gets the string for the next indentation
        /// </summary>
        public string NextIndentation => Indentation + "    ";

        /// <summary>
        /// Gets the callstack for the next level
        /// </summary>
        public CallStack Next => new(this);

        /// <summary>
        ///     Creates an indented callstack without increasing the level.
        /// </summary>
        public CallStack NextWithoutLevelIncrease
        {
            get
            {
                var result = new CallStack(this);
                result.Level--;
                return result;
            }
        }

        /// <summary>
        /// Converts the call stack to a string, containing level and full name
        /// </summary>
        /// <returns>The converted stack</returns>
        public override string ToString() =>
            $"Level: {Level} ({Fullname})";
    }
}