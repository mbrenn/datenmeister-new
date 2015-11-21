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
    public class ClassTreeGenerator
    {
        public static Version FactoryVersion = new Version(1, 0, 0, 0);

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

        /// <summary>
        /// Initializes a new instance of the ClassTreeGenerator
        /// </summary>
        public ClassTreeGenerator()
        {
            Result = new StringBuilder();
        }

        /// <summary>
        /// Creates a C# class instance for all the packages and classes within the extent
        /// </summary>
        /// <param name="extent">Extent to be used</param>
        public void CreateClassTree(IUriExtent extent)
        {
            foreach (var element in extent.elements())
            {
                var elementAsObject = element as IObject;
                var attributeXmi = "{" + Namespaces.Xmi.ToString() + "}type";

                // Work only on the UML:Packages as the entry node
                if (elementAsObject.isSet(attributeXmi) && 
                    elementAsObject.get(attributeXmi).ToString() == "uml:Package")
                {
                    CreateClassTree(elementAsObject);
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
        public void CreateClassTree(IObject element)
        {
            var stack = new CallStack(null);
            Result.AppendLine($"{stack.Indentation}// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version {FactoryVersion}");

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
            ParsePackages(element, stack);
            postAction();
        }

        /// <summary>
        /// Parses the packages and creates the C# Code for all the
        /// packages by recursive calls to itself for packages and
        /// ParseClasses for classes.
        /// </summary>
        /// <param name="element">Element being parsed</param>
        private void ParsePackages(IObject element, CallStack stack)
        {
            var nameAsObject = element.get("name");
            var name = nameAsObject == null ? string.Empty : nameAsObject.ToString();

            Result.AppendLine($"{stack.Indentation}public class _{name}");
            Result.AppendLine($"{stack.Indentation}{{");
            var innerStack = new CallStack(stack);
            innerStack.Fullname = stack.Fullname == null ? name : $"{stack.Fullname}.{name}";

            // Finds the subpackages
            foreach (var package in Helper.XmiGetPackages(element))
            {
                ParsePackages(package, innerStack);
            }

            // Finds the classes in the package
            ParseClasses(element, innerStack);

            if (stack.Level == 0)
            {
                Result.AppendLine($"{innerStack.Indentation}public _{name} TheOne = new _{name}();");
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
        /// Parses the packages
        /// </summary>
        /// <param name="element">Element being parsed</param>
        private void ParseClasses(IObject element, CallStack stack)
        {
            var innerStack = new CallStack(stack);
            foreach (var classInstance in Helper.XmiGetClass(element))
            {
                var nameAsObject = classInstance.get("name");
                var name = nameAsObject == null ? string.Empty : nameAsObject.ToString();

                Result.AppendLine($"{innerStack.Indentation}public class _{name}");
                Result.AppendLine($"{innerStack.Indentation}{{");

                ParseProperties(classInstance, innerStack);

                Result.AppendLine($"{innerStack.Indentation}}}");
                Result.AppendLine();

                Result.AppendLine($"{innerStack.Indentation}public _{name} {name} = new _{name}();");
                Result.AppendLine();
            }
        }

        private void ParseProperties(IObject classInstance, CallStack stack)
        {
            var innerStack = new CallStack(stack);

            foreach (var propertyObject in Helper.XmiGetProperty(classInstance))
            {
                var nameAsObject = propertyObject.get("name");
                var name = nameAsObject == null ? string.Empty : nameAsObject.ToString();
                Result.AppendLine($"{innerStack.Indentation}public object @{name} = new object();");
                Result.AppendLine();
            }
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
        }
    }
}
