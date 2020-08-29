#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Models.FastViewFilter
{
    public class FillTheFastViewFilters : DatenMeister.Core.Filler.IFiller<_FastViewFilters>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject? element)
        {
            if (element == null) throw new System.ArgumentNullException(nameof(element));
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object?> collection, _FastViewFilters tree)
        {
            FillTheFastViewFilters.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object?> collection, _FastViewFilters tree)
        {
            string? name;
            IElement? value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement ?? throw new System.InvalidOperationException("value == null");
                name = GetNameOfElement(value);
                if (name == "FastViewFilters") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement ?? throw new System.InvalidOperationException("value == null");
                        name = GetNameOfElement(value);
                        if(name == "PropertyComparisonFilter") // Looking for class
                        {
                            tree.__PropertyComparisonFilter = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "Property") // Looking for property
                                {
                                    tree.PropertyComparisonFilter._Property = value;
                                }
                                if(name == "ComparisonType") // Looking for property
                                {
                                    tree.PropertyComparisonFilter._ComparisonType = value;
                                }
                                if(name == "Value") // Looking for property
                                {
                                    tree.PropertyComparisonFilter._Value = value;
                                }
                            }
                        }
                        if(name == "PropertyContainsFilter") // Looking for class
                        {
                            tree.__PropertyContainsFilter = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "Property") // Looking for property
                                {
                                    tree.PropertyContainsFilter._Property = value;
                                }
                                if(name == "Value") // Looking for property
                                {
                                    tree.PropertyContainsFilter._Value = value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
