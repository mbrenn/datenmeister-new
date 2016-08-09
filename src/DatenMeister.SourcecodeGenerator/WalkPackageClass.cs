﻿using System;
using System.Collections.Generic;
using System.Text;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.SourcecodeGenerator.SourceParser;
using DatenMeister.XMI.UmlBootstrap;

namespace DatenMeister.SourcecodeGenerator
{
    /// <summary>
    ///     Creates a class tree out of an XML which can be used to fill the appropriate instance
    /// </summary>
    public class WalkPackageClass
    {
        public Version FactoryVersion = new Version(1, 0, 1, 0);

        /// <summary>
        /// Stores the source parser to find out the elements and attributes
        /// </summary>
        private readonly ISourceParser _parser;


        public WalkPackageClass(ISourceParser parser)
        {
            _parser = parser ?? new XmiSourceParser();
            Result = new StringBuilder();
        }

        /// <summary>
        ///     Gets or sets the namespace
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        ///     Gets or sets the result being delivered back
        /// </summary>
        public StringBuilder Result { get; set; }

        /// <summary>
        ///     Creates a C# class instance for all the packages and classes within the extent
        /// </summary>
        /// <param name="extent">Extent to be used</param>
        public virtual void Walk(IUriExtent extent)
        {
            var stack = new CallStack(null);

            StartNamespace(ref stack);
            foreach (var element in extent.elements())
            {
                var elementAsObject = element as IObject;
                Walk(elementAsObject, stack);
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
        private void Walk(IObject element, CallStack stack)
        {
            if (_parser.IsPackage(element))
            {
                WalkPackage(element, stack);
            }

            if (_parser.IsClass(element))
            {
                WalkClass(element, stack);
            }
        }

        /// <summary>
        /// Writes the usage decleration for each element as given in the namespaces parameter
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

        /// <summary>
        ///     Creates a C# source code. Not to be used for recursive
        ///     call since the namespace is just once created
        /// </summary>
        /// <param name="element">
        ///     Regards the given element as a package
        ///     and returns a full namespace for the package.
        /// </param>
        private void StartNamespace(ref CallStack stack)
        {
            Result.AppendLine($"{stack.Indentation}// Created by {GetType().FullName} Version {FactoryVersion}");

            // Check, if we have namespaces
            if (!string.IsNullOrEmpty(Namespace))
            {
                var indentation = stack.Indentation;
                    Result.AppendLine($"{indentation}namespace {Namespace}");
                    Result.AppendLine($"{indentation}{{");

                stack = new CallStack(stack);
                stack.Level--;
            }
        }

        /// <summary>
        ///     Creates a C# source code. Not to be used for recursive
        ///     call since the namespace is just once created
        /// </summary>
        /// <param name="element">
        ///     Regards the given element as a package
        ///     and returns a full namespace for the package.
        /// </param>
        private void EndNamespace(ref CallStack stack)
        {
            // Check, if we have namespaces
            Action preAction = () => { };
            Action postAction = () => { };
            if (!string.IsNullOrEmpty(Namespace))
            {
                stack = stack.Owner;
                var indentation = stack.Indentation;
                Result.AppendLine($"{indentation}}}");

                stack = new CallStack(stack);
                stack.Level--;
            }
        }

        /// <summary>
        ///     Parses the packages and creates the C# Code for all the
        ///     packages by recursive calls to itself for packages and
        ///     ParseClasses for classes.
        /// </summary>
        /// <param name="element">Element being parsed</param>
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
                }

                if (_parser.IsClass(subElement) || _parser.IsPrimitiveType(subElement))
                {
                    WalkClass(subElement, innerStack);
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

            // Needs to be updated
            foreach (var propertyObject in Helper.GetSubProperties(classInstance))
            {
                if (_parser.IsProperty(propertyObject))
                {
                    WalkProperty(propertyObject, innerStack);
                }
            }
        }

        protected virtual void WalkProperty(IObject classInstance, CallStack stack)
        {
        }

        protected static string GetNameOfElement(IObject element)
        {
            var nameAsObject = element.get("name");
            var name = nameAsObject == null ? string.Empty : nameAsObject.ToString();
            return name;
        }

        /// <summary>
        ///     Defines the callstack
        /// </summary>
        public class CallStack
        {
            /// <summary>
            ///     Stores the owner stack
            /// </summary>
            private CallStack _ownerStack;

            /// <summary>
            /// Initializes a new instance of the callback
            /// </summary>
            /// <param name="ownerStack">The owning callstack. It may be null, if this is the root
            /// call stack</param>
            public CallStack(CallStack ownerStack)
            {
                _ownerStack = ownerStack;
                Indentation = ownerStack == null ? string.Empty : $"{ownerStack.NextIndentation}";
                Level = ownerStack?.Level + 1 ?? 0;
                Fullname = ownerStack?.Fullname;
            }

            public int Level { get; set; }

            public string Fullname { get; set; }

            public string Indentation { get; set; }

            public string NextIndentation
            {
                get { return Indentation + "    "; }
            }

            public CallStack Next
            {
                get { return new CallStack(this); }
            }

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
            ///     Stores the owner stack
            /// </summary>
            public CallStack Owner
            {
                get { return _ownerStack; }
                set { _ownerStack = value; }
            }

            public override string ToString()
            {
                return $"Level: {Level} ({Fullname})";
            }
        }
    }
}