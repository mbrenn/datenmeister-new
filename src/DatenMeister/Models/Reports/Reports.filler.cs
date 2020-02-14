#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Models.Reports
{
    public class FillTheReports : DatenMeister.Core.Filler.IFiller<_Reports>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject? element)
        {
            if (element == null) throw new System.ArgumentNullException(nameof(element));
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object> collection, _Reports tree)
        {
            FillTheReports.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object> collection, _Reports tree)
        {
            string? name;
            IElement? value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement ?? throw new System.InvalidOperationException("value == null");
                name = GetNameOfElement(value);
                if (name == "Reports") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement ?? throw new System.InvalidOperationException("value == null");
                        name = GetNameOfElement(value);
                        if(name == "ReportDefinition") // Looking for class
                        {
                            tree.__ReportDefinition = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.ReportDefinition._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.ReportDefinition._title = value;
                                }
                                if(name == "elements") // Looking for property
                                {
                                    tree.ReportDefinition._elements = value;
                                }
                            }
                        }
                        if(name == "ReportElement") // Looking for class
                        {
                            tree.__ReportElement = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.ReportElement._name = value;
                                }
                            }
                        }
                        if(name == "ReportHeadline") // Looking for class
                        {
                            tree.__ReportHeadline = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "title") // Looking for property
                                {
                                    tree.ReportHeadline._title = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.ReportHeadline._name = value;
                                }
                            }
                        }
                        if(name == "ReportParagraph") // Looking for class
                        {
                            tree.__ReportParagraph = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "paragraph") // Looking for property
                                {
                                    tree.ReportParagraph._paragraph = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.ReportParagraph._name = value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
