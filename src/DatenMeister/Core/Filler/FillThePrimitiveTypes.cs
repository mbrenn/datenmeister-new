#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Core.Filler
{
    public class FillThePrimitiveTypes : DatenMeister.Core.Filler.IFiller<DatenMeister.Core._PrimitiveTypes>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject? element)
        {
            if (element == null) throw new System.ArgumentNullException(nameof(element));
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object?> collection, DatenMeister.Core._PrimitiveTypes tree)
        {
            FillThePrimitiveTypes.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object?> collection, DatenMeister.Core._PrimitiveTypes tree)
        {
            string? name;
            IElement? value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement ?? throw new System.InvalidOperationException("value == null");
                name = GetNameOfElement(value);
                if (name == "PrimitiveTypes") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement ?? throw new System.InvalidOperationException("value == null");
                        name = GetNameOfElement(value);
                        if(name == "Boolean") // Looking for class
                        {
                            tree.__Boolean = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                            }
                        }
                        if(name == "Integer") // Looking for class
                        {
                            tree.__Integer = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                            }
                        }
                        if(name == "Real") // Looking for class
                        {
                            tree.__Real = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                            }
                        }
                        if(name == "String") // Looking for class
                        {
                            tree.__String = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                            }
                        }
                        if(name == "UnlimitedNatural") // Looking for class
                        {
                            tree.__UnlimitedNatural = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                            }
                        }
                    }
                }
            }
        }
    }
}
