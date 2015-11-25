using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.XMI.UmlBootstrap;
using System;
using System.Text;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.XMI;

namespace DatenMeister.SourcecodeGenerator
{
    /// <summary>
    /// Creates a class tree out of an XML which can be used to fill the appropriate instance
    /// </summary>
    public class WalkPackageClass
    {
        public Version FactoryVersion = new Version(1, 0, 0, 0);

        /// <summary>
        /// Gets or sets the namespace
        /// </summary>
        public string Namespace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the result being delivered back
        /// </summary>
        public StringBuilder Result
        {
            get;
            set;
        }

        public WalkPackageClass()
        {
            Result = new StringBuilder();
        }

        /// <summary>
        /// Creates a C# class instance for all the packages and classes within the extent
        /// </summary>
        /// <param name="extent">Extent to be used</param>
        public void Walk(IUriExtent extent)
        {
            foreach (var element in extent.elements())
            {
                var elementAsObject = element as IObject;
                var attributeXmi = "{" + Namespaces.Xmi.ToString() + "}type";

                // Work only on the UML:Packages as the entry node
                if (elementAsObject.isSet(attributeXmi) &&
                    elementAsObject.get(attributeXmi).ToString() == "uml:Package")
                {
                    var stack = new CallStack(null);
                    Walk(elementAsObject, stack);
                }
            }
        }

        /// <summary>
        /// Creates a C# source code. Not to be used for recursive 
        /// call since the namespace is just once created
        /// </summary>
        /// <param name="element">Regards the given element as a package
        /// and returns a full namespace for the package. 
        ///</param>
        protected virtual void Walk(IObject element, CallStack stack)
        {
            WalkPackage(element, stack);
        }

        /// <summary>
        /// Creates a C# source code. Not to be used for recursive 
        /// call since the namespace is just once created
        /// </summary>
        /// <param name="element">Regards the given element as a package
        /// and returns a full namespace for the package. 
        ///</param>
        protected void WalkAndWriteNamespace(IObject element, CallStack stack)
        {
            Result.AppendLine($"{stack.Indentation}// Created by {this.GetType().FullName} Version {FactoryVersion}");

            // Check, if we have namespaces
            Action preAction = () => { };
            Action postAction = () => { };
            if (!string.IsNullOrEmpty(Namespace))
            {
                var indentation = stack.Indentation;
                preAction = () =>
                {
                    Result.AppendLine($"{indentation}namespace {Namespace}");
                    Result.AppendLine($"{indentation}{{");
                };
                postAction = () =>
                {
                    Result.AppendLine($"{indentation}}}");
                };

                stack = new CallStack(stack);
                stack.Level--;
            }

            // Actually executes the class tree creation
            preAction();
            WalkPackage(element, stack);
            postAction();
        }

        /// <summary>
        /// Parses the packages and creates the C# Code for all the
        /// packages by recursive calls to itself for packages and
        /// ParseClasses for classes.
        /// </summary>
        /// <param name="element">Element being parsed</param>
        protected virtual void WalkPackage(IObject element, CallStack stack)
        {
            var innerStack = new CallStack(stack);
            var name = GetNameOfElement(element);
            innerStack.Fullname = 
                string.IsNullOrEmpty(innerStack.Fullname) ? name : $"{innerStack.Fullname}.{name}";

            // Finds the subpackages
            foreach (var package in Helper.XmiGetPackages(element))
            {
                WalkPackage(package, innerStack);
            }

            // Finds the classes in the package

            foreach (var classInstance in Helper.XmiGetClass(element))
            {
                WalkClass(classInstance, innerStack);
            }
        }

        /// <summary>
        /// Parses the packages
        /// </summary>
        /// <param name="element">Element classInstance parsed</param>
        protected virtual void WalkClass(IObject classInstance, CallStack stack)
        {
            var innerStack = new CallStack(stack);
            var name = GetNameOfElement(classInstance);
            innerStack.Fullname += $".{name}";

            foreach (var propertyObject in Helper.XmiGetProperty(classInstance))
            {
                WalkProperty(propertyObject, innerStack);
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
        /// Defines the callstack
        /// </summary>
        public class CallStack
        {
            public CallStack(CallStack ownerStack)
            {
                _ownerStack = ownerStack;
                Indentation = ownerStack == null ? string.Empty : $"{ownerStack.Indentation}    ";
                Level = ownerStack == null ? 0 : ownerStack.Level + 1;
                Fullname = ownerStack?.Fullname;
            }

            /// <summary>
            /// Stores the owner stack
            /// </summary>
            private CallStack _ownerStack;

            public int Level
            {
                get;
                set;
            }

            public string Fullname
            {
                get;
                set;
            }

            public string Indentation
            {
                get;
                set;
            }

            public string NextIndentation
            {
                get { return this.Indentation + "    "; }
            }

            public CallStack Next
            {
                get { return new CallStack(this); }
            }

            /// <summary>
            /// Creates an indented callstack without increasing the level.
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

            public override string ToString()
            {
                return $"Level: {Level} ({Fullname})";
            }
        }
    }
}
