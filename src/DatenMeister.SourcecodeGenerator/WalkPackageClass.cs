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
        /// Parses the packages and creates the C# Code for all the
        /// packages by recursive calls to itself for packages and
        /// ParseClasses for classes.
        /// </summary>
        /// <param name="element">Element being parsed</param>
        protected virtual void WalkPackage(IObject element, CallStack stack)
        {
            var innerStack = new CallStack(stack);

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
            WalkProperties(classInstance, stack);
        }

        protected virtual void WalkProperties(IObject classInstance, CallStack stack)
        {
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
