using System;
using System.Collections.Generic;
using System.Text;

namespace DatenMeister.SourcecodeGenerator
{
    /// <summary>
    /// Creates the integration class for the DotNet-Type integration
    /// </summary>
    public class DotNetIntegrationGenerator
    {
        public StringBuilder Result { get; private set; } = new();

        public void Create(
            string nameSpace,
            string packageName,
            IEnumerable<Type> types)
        {
            Result.Clear();

            var stack = new WalkPackageClass.CallStack(null);

            Result.AppendLine($"{stack.Indentation}using DatenMeister.Core;");
            Result.AppendLine($"{stack.Indentation}using DatenMeister.Core.EMOF.Implementation;");
            Result.AppendLine($"{stack.Indentation}using DatenMeister.Core.EMOF.Interface.Common;");
            Result.AppendLine($"{stack.Indentation}using DatenMeister.Core.EMOF.Interface.Reflection;");
            Result.AppendLine($"{stack.Indentation}using DatenMeister.Models.EMOF;");
            Result.AppendLine($"{stack.Indentation}using DatenMeister.Provider.DotNet;");
            Result.AppendLine($"{stack.Indentation}// Created by ${typeof(DotNetIntegrationGenerator).FullName}");
            Result.AppendLine("// ReSharper disable RedundantNameQualifier");
            Result.AppendLine();
            Result.AppendLine($"{stack.Indentation}namespace {nameSpace}");
            Result.AppendLine($"{stack.Indentation}{{");

            stack = stack.Next;
            Result.AppendLine($"{stack.Indentation}public static class Integrate{packageName}");
            Result.AppendLine($"{stack.Indentation}{{");

            stack = stack.Next;

            Result.AppendLine($"{stack.Indentation}/// <summary>");
            Result.AppendLine($"{stack.Indentation}/// Assigns the types of form and fields by converting the ");
            Result.AppendLine($"{stack.Indentation}/// .Net objects to DatenMeister elements and adds them into ");
            Result.AppendLine($"{stack.Indentation}/// the filler, the collection and also into the lookup. ");
            Result.AppendLine($"{stack.Indentation}/// </summary>");
            Result.AppendLine($"{stack.Indentation}/// <param name=\"uml\">The uml metamodel to be used</param>");
            Result.AppendLine($"{stack.Indentation}/// <param name=\"factory\">Factory being used for creation</param>");
            Result.AppendLine($"{stack.Indentation}/// <param name=\"collection\">Collection that shall be filled</param>");
            Result.AppendLine($"{stack.Indentation}/// <param name=\"extent\">And finally extent to which the types shall be registered</param>");
            Result.AppendLine(
                $"{stack.Indentation}public static void Assign(" +
                "IFactory factory, IReflectiveCollection collection, MofExtent extent)");
            Result.AppendLine($"{stack.Indentation}{{");

            stack = stack.Next;
            Result.AppendLine($"{stack.Indentation}var generator = new DotNetTypeGenerator(factory, extent);");

            foreach (var type in types)
            {
                Result.AppendLine($"{stack.Indentation}{{"); // Inner scope

                stack = stack.Next;
                var fullName = type.FullName?.Replace('+', '.');

                Result.AppendLine($"{stack.Indentation}var type = typeof({fullName});");
                Result.AppendLine($"{stack.Indentation}var typeAsElement = generator.CreateTypeFor(type);");
                Result.AppendLine($"{stack.Indentation}collection.add(typeAsElement);");
                Result.AppendLine($"{stack.Indentation}extent.TypeLookup.Add(typeAsElement, type);");

                stack = stack.Owner!;
                Result.AppendLine($"{stack.Indentation}}}"); // Inner scope
            }

            stack = stack.Owner!;
            Result.AppendLine($"{stack.Indentation}}}"); // method

            stack = stack.Owner!;
            Result.AppendLine($"{stack.Indentation}}}"); // class

            stack = stack.Owner!;
            Result.AppendLine($"{stack.Indentation}}}"); // namespace
        }
    }
}