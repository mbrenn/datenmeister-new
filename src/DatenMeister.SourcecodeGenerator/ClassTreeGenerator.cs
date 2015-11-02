using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.XMI.UmlBootstrap;
using System.Text;

namespace DatenMeister.SourcecodeGenerator
{
    /// <summary>
    /// Creates a class tree out of an XML which can be used to fill the appropriate instance
    /// </summary>
    public class ClassTreeGenerator
    {
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
            this.Result = new StringBuilder();
        }

        public void CreateClassTree(IObject element)
        {
            var stack = new CallStack(null);
            ParsePackages(element, stack);
        }

        /// <summary>
        /// Parses the packages
        /// </summary>
        /// <param name="element">Element being parsed</param>
        private void ParsePackages(IObject element, CallStack stack)
        {
            var nameAsObject = element.get("name");
            var name = nameAsObject == null ? string.Empty : nameAsObject.ToString();            

            var innerStack = new CallStack(stack);
            innerStack.Fullname = stack.Fullname == null ? name : $"{stack.Fullname}.{name}";

            // Finds the subpackages
            foreach (var package in Helper.XmiGetPackages(element))
            {
                ParsePackages(package, innerStack);
            }

            // Finds the classes in the package
            ParseClasses(element, innerStack);
        }

        /// <summary>
        /// Parses the packages
        /// </summary>
        /// <param name="element">Element being parsed</param>
        private void ParseClasses(IObject element, CallStack stack)
        {
            foreach (var classInstance in Helper.XmiGetClass(element))
            {
                var nameAsObject = classInstance.get("name");
                var name = nameAsObject == null ? string.Empty : nameAsObject.ToString();

                var className = stack.Fullname == null ? name : $"{stack.Fullname}.{name}";
                this.Result.AppendLine(className);
            }
        }

        public class CallStack
        {
            public CallStack(CallStack ownerStack)
            {
                _ownerStack = ownerStack;
            }

            /// <summary>
            /// Stores the owner stack
            /// </summary>
            private CallStack _ownerStack;

            public string Fullname
            {
                get;
                set;
            }
        }
    }
}
