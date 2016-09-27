﻿using System;
using System.Diagnostics;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.SourcecodeGenerator.SourceParser;

namespace DatenMeister.SourcecodeGenerator
{
    /// <summary>
    ///     Creates a class file which contains objects for all
    ///     items within the given extent.
    ///     It parses classes and packages.
    /// </summary>
    public class ClassTreeGenerator : WalkPackageClass
    {
        /// <summary>
        /// Gets or sets the name of the class that is used for the model
        /// </summary>
        public string UsedClassName { get; set; }

        /// <summary>
        ///     Initializes a new instance of the ClassTreeGenerator
        /// </summary>
        public ClassTreeGenerator(ISourceParser parser = null) : base(parser)
        {
            FactoryVersion = new Version(1, 1, 0, 0);
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
            WriteUsages(new[]
            {
                "DatenMeister.Core.EMOF.Interface.Reflection",
                "DatenMeister.Core.EMOF.InMemory"
            });
            
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

            var innerStack = new CallStack(stack);
            innerStack.Fullname = stack.Fullname == null ? name : $"{stack.Fullname}.{name}";

            base.WalkPackage(element, stack);

            if (stack.Level == 0)
            {
                UsedClassName = $"_{name}";
                Result.AppendLine($"{innerStack.Indentation}public static _{name} TheOne = new _{name}();");
                Result.AppendLine();
            }

            Result.AppendLine($"{stack.Indentation}}}");

            Result.AppendLine();

            if (stack.Level > 0)
            {
                Result.AppendLine($"{stack.Indentation}public _{name} {name} = new _{name}();");
                Result.AppendLine();
            }
        }

        /// <summary>
        ///     Parses the packages
        /// </summary>
        /// <param name="classInstance">The class that shall be retrieved</param>
        protected override void WalkClass(IObject classInstance, CallStack stack)
        {
            var name = GetNameOfElement(classInstance);

            Result.AppendLine($"{stack.Indentation}public class _{name}");
            Result.AppendLine($"{stack.Indentation}{{");

            base.WalkClass(classInstance, stack);

            Result.AppendLine($"{stack.Indentation}}}");

            Result.AppendLine();
            Result.AppendLine($"{stack.Indentation}public _{name} @{name} = new _{name}();");
            Result.AppendLine($"{stack.Indentation}public IElement @__{name} = new MofElement();");
            Result.AppendLine(); 
        }


        protected override void WalkProperty(IObject propertyObject, CallStack stack)
        {
            base.WalkProperty(propertyObject, stack);

            var nameAsObject = propertyObject.get("name");
            var name = nameAsObject == null ? string.Empty : nameAsObject.ToString();
            if (name != null)
            {
                Result.AppendLine($"{stack.Indentation}public static string @{name} = \"{name}\";");
                Result.AppendLine($"{stack.Indentation}public IElement _{name} = null;");
                Result.AppendLine();
            }
            else
            {
                Debug.WriteLine($"Found unknown property: {propertyObject}");
            }
        }
    }
}