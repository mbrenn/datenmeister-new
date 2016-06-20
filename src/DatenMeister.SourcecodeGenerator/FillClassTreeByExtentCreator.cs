using System;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.SourcecodeGenerator
{
    public class FillClassTreeByExtentCreator : WalkPackageClass
    {
        public FillClassTreeByExtentCreator(string classNameOfTree)
        {
            ClassNameOfTree = classNameOfTree;
            FactoryVersion = new Version(1, 1, 0, 0);
        }

        public string ClassNameOfTree { get; set; }

        /// <summary>
        ///     Creates a C# source code. Not to be used for recursive
        ///     call since the namespace is just once created
        /// </summary>
        /// <param name="element">
        ///     Regards the given element as a package
        ///     and returns a full namespace for the package.
        /// </param>
        /// <param name="stack">Used as the callstack</param>
        protected override void Walk(IObject element, CallStack stack)
        {
            Result.AppendLine("using System.Collections.Generic;");
            Result.AppendLine("using DatenMeister.EMOF.Interface.Reflection;");
            WalkAndWriteNamespace(element, stack);
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

                Result.AppendLine($"{stack.Indentation}public class FillThe{name} : DatenMeister.Filler.IFiller<{ClassNameOfTree}>");
                Result.AppendLine($"{stack.Indentation}{{");

                // Creates the GetNameOfElement helper method
                Result.AppendLine($"{methodStack.Indentation}private static readonly object[] EmptyList = new object[] {{ }};");
                Result.AppendLine($"{methodStack.Indentation}private static string GetNameOfElement(IObject element)");
                Result.AppendLine($"{methodStack.Indentation}{{");
                Result.AppendLine($"{methodStack.Indentation}    var nameAsObject = element.get(\"name\");");
                Result.AppendLine(
                    $"{methodStack.Indentation}    return nameAsObject == null ? string.Empty : nameAsObject.ToString();");
                Result.AppendLine($"{methodStack.Indentation}}}");
                Result.AppendLine();

                Result.AppendLine(
                    $"{methodStack.Indentation}public void Fill(IEnumerable<object> collection, {ClassNameOfTree} tree)");
                Result.AppendLine($"{methodStack.Indentation}{{");
                Result.AppendLine($"{foreachStack.Indentation}FillThe{name}.DoFill(collection, tree);");
                Result.AppendLine($"{methodStack.Indentation}}}");
                Result.AppendLine();

                Result.AppendLine(
                    $"{methodStack.Indentation}public static void DoFill(IEnumerable<object> collection, {ClassNameOfTree} tree)");
                Result.AppendLine($"{methodStack.Indentation}{{");
                Result.AppendLine($"{foreachStack.Indentation}string name;");
                Result.AppendLine($"{foreachStack.Indentation}IElement value;");
                Result.AppendLine($"{foreachStack.Indentation}bool isSet;");

                Result.AppendLine($"{foreachStack.Indentation}foreach (var item in collection)");
                Result.AppendLine($"{foreachStack.Indentation}{{");
                Result.AppendLine($"{innerStack.Indentation}value = item as IElement;");
                Result.AppendLine($"{innerStack.Indentation}name = GetNameOfElement(value);");
            }

            // First, go through the elements 
            Result.AppendLine($"{innerStack.Indentation}if (name == \"{name}\") // Looking for package");
            Result.AppendLine($"{innerStack.Indentation}{{");

            var ifStack = innerStack.NextWithoutLevelIncrease;
            var ifForeachStack = ifStack.NextWithoutLevelIncrease;
            Result.AppendLine($"{ifStack.Indentation}isSet = value.isSet(\"packagedElement\");");
            Result.AppendLine(
                $"{ifStack.Indentation}collection = isSet ? (value.get(\"packagedElement\") as IEnumerable<object>) : EmptyList;");
            Result.AppendLine($"{ifStack.Indentation}foreach (var item{ifStack.Level} in collection)");
            Result.AppendLine($"{ifStack.Indentation}{{");
            Result.AppendLine($"{ifForeachStack.Indentation}value = item{ifStack.Level} as IElement;");
            Result.AppendLine($"{ifForeachStack.Indentation}name = GetNameOfElement(value);");

            base.WalkPackage(element, ifStack);

            Result.AppendLine($"{ifStack.Indentation}}}");
            Result.AppendLine($"{innerStack.Indentation}}}");

            if (stack.Level == 0)
            {
                Result.AppendLine($"{foreachStack.Indentation}}}");
                Result.AppendLine($"{methodStack.Indentation}}}");
                Result.AppendLine($"{stack.Indentation}}}");
            }
        }

        protected override void WalkClass(IObject classInstance, CallStack stack)
        {
            var name = GetNameOfElement(classInstance);
            Result.AppendLine($"{stack.Indentation}if(name == \"{name}\") // Looking for class");
            Result.AppendLine($"{stack.Indentation}{{");

            string fullName; 
            if (stack.Level == 1)
            {
                // Removes the name of the package of the first hierarchy level
                // since it is already included into the class name
                fullName = $"__{name}";
            }
            else
            {
                fullName = $"{stack.Fullname}.__{name}";
            }
            

            var ifStack = stack.NextWithoutLevelIncrease;
            var ifForeachStack = ifStack.NextWithoutLevelIncrease;
            Result.AppendLine($"{ifStack.Indentation}tree.{fullName} = value;");
            Result.AppendLine($"{ifStack.Indentation}isSet = value.isSet(\"ownedAttribute\");");
            Result.AppendLine(
                $"{ifStack.Indentation}collection = isSet ? (value.get(\"ownedAttribute\") as IEnumerable<object>) : EmptyList;");
            Result.AppendLine($"{ifStack.Indentation}foreach (var item{ifStack.Level} in collection)");
            Result.AppendLine($"{ifStack.Indentation}{{");
            Result.AppendLine($"{ifForeachStack.Indentation}value = item{ifStack.Level} as IElement;");
            Result.AppendLine($"{ifForeachStack.Indentation}name = GetNameOfElement(value);");

            base.WalkClass(classInstance, ifStack);

            Result.AppendLine($"{ifStack.Indentation}}}");
            Result.AppendLine($"{stack.Indentation}}}");
        }

        protected override void WalkProperty(IObject propertyInstance, CallStack stack)
        {
            var name = GetNameOfElement(propertyInstance);
            Result.AppendLine($"{stack.Indentation}if(name == \"{name}\") // Looking for property");
            Result.AppendLine($"{stack.Indentation}{{");
            Result.AppendLine($"{stack.NextIndentation}tree.{stack.Fullname}._{name} = value;");

            base.WalkProperty(propertyInstance, stack);

            Result.AppendLine($"{stack.Indentation}}}");
        }
    }
}