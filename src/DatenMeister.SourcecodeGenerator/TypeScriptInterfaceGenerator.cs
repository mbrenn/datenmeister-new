 using System;
 using System.Collections.Generic;
 using System.Linq;
 using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourcecodeGenerator
{
    /// <summary>
    ///     Creates a class file which contains objects for all
    ///     items within the given extent.
    ///     It parses classes and packages.
    /// </summary>
    public class TypeScriptInterfaceGenerator : WalkPackageClass
    {

        /// <summary>
        ///     Initializes a new instance of the ClassTreeGenerator
        /// </summary>
        public TypeScriptInterfaceGenerator(ISourceParser? parser = null) : base(parser)
        {
            FactoryVersion = new Version(1, 0, 0, 0);
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

            Result.AppendLine($"{stack.Indentation}export module _{name}");
            Result.AppendLine($"{stack.Indentation}{{");

            var innerStack = new CallStack(stack)
            {
                Fullname = stack.Fullname == null ? name : $"{stack.Fullname}.{name}"
            };

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
            if (!(classInstance is IElement asElement)) return;
            var name = GetNameOfElement(classInstance);

            Result.AppendLine($"{stack.Indentation}export module _{name}");
            Result.AppendLine($"{stack.Indentation}{{");

            base.WalkClass(classInstance, stack);

            Result.AppendLine($"{stack.Indentation}}}");

            Result.AppendLine();
            Result.AppendLine($"{stack.Indentation}export const __{name}_Uri = \"{asElement.GetUri()}\";");
        }

        protected override void WalkProperty(IObject propertyObject, CallStack stack)
        {
            base.WalkProperty(propertyObject, stack);

            if (propertyObject is not IElement) return;

            var nameAsObject = propertyObject.get("name");
            var name = nameAsObject?.ToString() ?? string.Empty;
            Result.AppendLine($"{stack.Indentation}export const {EscapeKeyword(name)} = \"{name}\";");
        }

        /// <summary>
        ///     Parses the packages
        /// </summary>
        /// <param name="enumInstance">The class that shall be retrieved</param>
        /// <param name="stack">Stack being used</param>
        /// <param name="callee">The element to be called when tn enumeration is required to be called.
        /// May be null, then WalkEnumLiteral will be called</param>
        protected override void WalkEnum(IObject enumInstance, CallStack stack, Action<IObject, CallStack>? callee = null)
        {
            var asElement = (IElement) enumInstance;
            var name = GetNameOfElement(enumInstance);

            Result.AppendLine($"{stack.Indentation}export module _{name}");
            Result.AppendLine($"{stack.Indentation}{{");

            base.WalkEnum(enumInstance, stack, callee);

            Result.AppendLine($"{stack.Indentation}}}");
            Result.AppendLine();

          
            Result.AppendLine($"{stack.Indentation}export enum ___{name}");
            Result.AppendLine($"{stack.Indentation}{{");

            var first = true;

            base.WalkEnum(enumInstance, stack, (literal, innerStack) =>
            {
                if (!first)
                {
                    Result.AppendLine(",");
                }
                
                var nameAsObject = literal.get("name");
                var literalName = nameAsObject == null ? string.Empty : nameAsObject.ToString()!;

                Result.Append($"{innerStack.Indentation}{EscapeKeyword(literalName)}");
                first = false;
            });

            Result.AppendLine();
            Result.AppendLine($"{stack.Indentation}}}");
            Result.AppendLine();
        }

        protected override void WalkEnumLiteral(IObject enumLiteralObject, CallStack stack)
        {
            base.WalkEnumLiteral(enumLiteralObject, stack);

            var nameAsObject = enumLiteralObject.get("name");
            var name = nameAsObject == null ? string.Empty : nameAsObject.ToString()!;

            Result.AppendLine($"{stack.Indentation}export const {EscapeKeyword(name)} = \"{name}\";");
        }

        private string EscapeKeyword(string name)
        {
            if (GetReservedKeywords().Any(x => x == name))
            {
                return $"_{name}_";
            }

            return name;
        }
        
        private string[]? _reservedKeywords = null;
        
        /// <summary>
        /// Gets an array of the reserved keywords
        /// </summary>
        /// <returns></returns>
        string[] GetReservedKeywords()
        {
            _reservedKeywords ??= new[]
            {
                "abstract",
                "arguments",
                "await",
                "boolean",
                "break",
                "byte",
                "case",
                "catch",
                "char",
                "class",
                "const",
                "continue",
                "debugger",
                "default",
                "delete",
                "do",
                "double",
                "else",
                "enum",
                "eval",
                "export",
                "extends",
                "false",
                "final",
                "finally",
                "float",
                "for",
                "function",
                "goto",
                "if",
                "implements",
                "import",
                "in",
                "instanceof",
                "int",
                "interface",
                "let",
                "long",
                "native",
                "new",
                "null",
                "package",
                "private",
                "protected",
                "public",
                "return",
                "short",
                "static",
                "super",
                "switch",
                "synchronized",
                "this",
                "throw",
                "throws",
                "transient",
                "true",
                "try",
                "typeof",
                "var",
                "void",
                "volatile",
                "while",
                "with",
                "yield",
            };

            return _reservedKeywords;
        }
    }
}