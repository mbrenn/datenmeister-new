using DatenMeister.EMOF.Interface.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.SourcecodeGenerator
{
    public class FillClassTreeByExtentCreator : WalkPackageClass
    {
        public string ClassNameOfTree
        {
            get;
            set;
        }

        public FillClassTreeByExtentCreator(string classNameOfTree)
        {
            this.ClassNameOfTree = classNameOfTree;
            this.FactoryVersion = new Version(1, 0, 0, 0);
        }
        
        /// <summary>
        /// Creates a C# source code. Not to be used for recursive 
        /// call since the namespace is just once created
        /// </summary>
        /// <param name="element">Regards the given element as a package
        /// and returns a full namespace for the package. 
        ///</param>
        protected override void Walk(IObject element, CallStack stack)
        {
            this.Result.AppendLine("using System.Collections.Generic;");
            this.Result.AppendLine("using DatenMeister.EMOF.Interface.Reflection;");
            base.WalkAndWriteNamespace(element, stack);
        }

        protected override void WalkPackage(IObject element, CallStack stack)
        {
            if (stack.Level == 1)
            {
                // Removes the name of the package of the first hierarchy level
                // since it is already included into the class name
                stack.Fullname = string.Empty;
            }

            var name = GetNameOfElement(element);

            var methodStack = stack;
            var innerStack = stack;
            var foreachStack = stack;

            if (stack.Level == 0)
            {
                methodStack = stack.NextWithoutLevelIncrease;
                foreachStack = methodStack.NextWithoutLevelIncrease;
                innerStack = foreachStack.NextWithoutLevelIncrease;

                this.Result.AppendLine($"{stack.Indentation}public class FillThe{name}");
                this.Result.AppendLine($"{stack.Indentation}{{");

                // Creates the GetNameOfElement helper method
                this.Result.AppendLine($"{methodStack.Indentation}private static object[] EmptyList = new object[] {{ }};");
                this.Result.AppendLine($"{methodStack.Indentation}private static string GetNameOfElement(IObject element)");
                this.Result.AppendLine($"{methodStack.Indentation}{{");
                this.Result.AppendLine($"{methodStack.Indentation}    var nameAsObject = element.get(\"name\");");
                this.Result.AppendLine($"{methodStack.Indentation}    return nameAsObject == null ? string.Empty : nameAsObject.ToString();");
                this.Result.AppendLine($"{methodStack.Indentation}}}");
                this.Result.AppendLine();

                this.Result.AppendLine($"{methodStack.Indentation}public static void DoFill(IEnumerable<object> collection, {ClassNameOfTree} tree)");
                this.Result.AppendLine($"{methodStack.Indentation}{{");
                this.Result.AppendLine($"{foreachStack.Indentation}string name;");
                this.Result.AppendLine($"{foreachStack.Indentation}IObject value;");
                this.Result.AppendLine($"{foreachStack.Indentation}bool isSet;");

                this.Result.AppendLine($"{foreachStack.Indentation}foreach (var item in collection)");
                this.Result.AppendLine($"{foreachStack.Indentation}{{");
                this.Result.AppendLine($"{innerStack.Indentation}value = item as IObject;");
                this.Result.AppendLine($"{innerStack.Indentation}name = GetNameOfElement(value);");
            }

            // First, go through the elements 
            this.Result.AppendLine($"{innerStack.Indentation}if (name == \"{name}\") // Looking for package");
            this.Result.AppendLine($"{innerStack.Indentation}{{");

            var ifStack = innerStack.NextWithoutLevelIncrease;
            var ifForeachStack = ifStack.NextWithoutLevelIncrease;
            this.Result.AppendLine($"{ifStack.Indentation}isSet = value.isSet(\"packagedElement\");");
            this.Result.AppendLine($"{ifStack.Indentation}collection = isSet ? (value.get(\"packagedElement\") as IEnumerable<object>) : EmptyList;");
            this.Result.AppendLine($"{ifStack.Indentation}foreach (var item{ifStack.Level} in collection)");
            this.Result.AppendLine($"{ifStack.Indentation}{{");
            this.Result.AppendLine($"{ifForeachStack.Indentation}value = item{ifStack.Level} as IObject;");
            this.Result.AppendLine($"{ifForeachStack.Indentation}name = GetNameOfElement(value);");

            base.WalkPackage(element, ifStack);

            this.Result.AppendLine($"{ifStack.Indentation}}}");
            this.Result.AppendLine($"{innerStack.Indentation}}}");

            if (stack.Level == 0)
            {
                this.Result.AppendLine($"{foreachStack.Indentation}}}");
                this.Result.AppendLine($"{methodStack.Indentation}}}");
                this.Result.AppendLine($"{stack.Indentation}}}");
            }
        }

        protected override void WalkClass(IObject classInstance, CallStack stack)
        {
            var name = GetNameOfElement(classInstance);
            this.Result.AppendLine($"{stack.Indentation}if(name == \"{name}\") // Looking for class");
            this.Result.AppendLine($"{stack.Indentation}{{");

            var ifStack = stack.NextWithoutLevelIncrease;
            var ifForeachStack = ifStack.NextWithoutLevelIncrease;
            this.Result.AppendLine($"{ifStack.Indentation}tree.{stack.Fullname}.@{name}Instance = value;");
            this.Result.AppendLine($"{ifStack.Indentation}isSet = value.isSet(\"ownedAttribute\");");
            this.Result.AppendLine($"{ifStack.Indentation}collection = isSet ? (value.get(\"ownedAttribute\") as IEnumerable<object>) : EmptyList;");
            this.Result.AppendLine($"{ifStack.Indentation}foreach (var item{ifStack.Level} in collection)");
            this.Result.AppendLine($"{ifStack.Indentation}{{");
            this.Result.AppendLine($"{ifForeachStack.Indentation}value = item{ifStack.Level} as IObject;");
            this.Result.AppendLine($"{ifForeachStack.Indentation}name = GetNameOfElement(value);");

            base.WalkClass(classInstance, ifStack);

            this.Result.AppendLine($"{ifStack.Indentation}}}");
            this.Result.AppendLine($"{stack.Indentation}}}");
        }

        protected override void WalkProperty(IObject propertyInstance, CallStack stack)
        {
            var name = GetNameOfElement(propertyInstance);
            this.Result.AppendLine($"{stack.Indentation}if(name == \"{name}\") // Looking for property");
            this.Result.AppendLine($"{stack.Indentation}{{");
            this.Result.AppendLine($"{stack.NextIndentation}tree.{stack.Fullname}.@{name} = value;");

            base.WalkProperty(propertyInstance, stack);

            this.Result.AppendLine($"{stack.Indentation}}}");
        }
    }
}
