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

        public void Fill(IEnumerable<object?> collection, _Reports tree)
        {
            FillTheReports.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object?> collection, _Reports tree)
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
                        if(name == "ReportTable") // Looking for class
                        {
                            tree.__ReportTable = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "viewNode") // Looking for property
                                {
                                    tree.ReportTable._viewNode = value;
                                }
                                if(name == "form") // Looking for property
                                {
                                    tree.ReportTable._form = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.ReportTable._name = value;
                                }
                            }
                        }
                        if(name == "SimpleReportConfiguration") // Looking for class
                        {
                            tree.__SimpleReportConfiguration = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "showDescendents") // Looking for property
                                {
                                    tree.SimpleReportConfiguration._showDescendents = value;
                                }
                                if(name == "rootElement") // Looking for property
                                {
                                    tree.SimpleReportConfiguration._rootElement = value;
                                }
                                if(name == "showRootElement") // Looking for property
                                {
                                    tree.SimpleReportConfiguration._showRootElement = value;
                                }
                                if(name == "showMetaClasses") // Looking for property
                                {
                                    tree.SimpleReportConfiguration._showMetaClasses = value;
                                }
                                if(name == "showFullName") // Looking for property
                                {
                                    tree.SimpleReportConfiguration._showFullName = value;
                                }
                                if(name == "form") // Looking for property
                                {
                                    tree.SimpleReportConfiguration._form = value;
                                }
                                if(name == "descendentMode") // Looking for property
                                {
                                    tree.SimpleReportConfiguration._descendentMode = value;
                                }
                                if(name == "typeMode") // Looking for property
                                {
                                    tree.SimpleReportConfiguration._typeMode = value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
