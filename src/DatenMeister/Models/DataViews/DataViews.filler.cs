#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Models.DataViews
{
    public class FillTheDataViews : DatenMeister.Core.Filler.IFiller<_DataViews>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject? element)
        {
            if (element == null) throw new System.ArgumentNullException(nameof(element));
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object?> collection, _DataViews tree)
        {
            FillTheDataViews.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object?> collection, _DataViews tree)
        {
            string? name;
            IElement? value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement ?? throw new System.InvalidOperationException("value == null");
                name = GetNameOfElement(value);
                if (name == "DataViews") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement ?? throw new System.InvalidOperationException("value == null");
                        name = GetNameOfElement(value);
                        if(name == "DataView") // Looking for class
                        {
                            tree.__DataView = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.DataView._name = value;
                                }
                                if(name == "workspace") // Looking for property
                                {
                                    tree.DataView._workspace = value;
                                }
                                if(name == "uri") // Looking for property
                                {
                                    tree.DataView._uri = value;
                                }
                                if(name == "viewNode") // Looking for property
                                {
                                    tree.DataView._viewNode = value;
                                }
                            }
                        }
                        if(name == "ViewNode") // Looking for class
                        {
                            tree.__ViewNode = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.ViewNode._name = value;
                                }
                            }
                        }
                        if(name == "SourceExtentNode") // Looking for class
                        {
                            tree.__SourceExtentNode = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "extentUri") // Looking for property
                                {
                                    tree.SourceExtentNode._extentUri = value;
                                }
                                if(name == "workspace") // Looking for property
                                {
                                    tree.SourceExtentNode._workspace = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.SourceExtentNode._name = value;
                                }
                            }
                        }
                        if(name == "FlattenNode") // Looking for class
                        {
                            tree.__FlattenNode = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "input") // Looking for property
                                {
                                    tree.FlattenNode._input = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.FlattenNode._name = value;
                                }
                            }
                        }
                        if(name == "FilterPropertyNode") // Looking for class
                        {
                            tree.__FilterPropertyNode = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "input") // Looking for property
                                {
                                    tree.FilterPropertyNode._input = value;
                                }
                                if(name == "property") // Looking for property
                                {
                                    tree.FilterPropertyNode._property = value;
                                }
                                if(name == "value") // Looking for property
                                {
                                    tree.FilterPropertyNode._value = value;
                                }
                                if(name == "comparisonMode") // Looking for property
                                {
                                    tree.FilterPropertyNode._comparisonMode = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.FilterPropertyNode._name = value;
                                }
                            }
                        }
                        if(name == "FilterTypeNode") // Looking for class
                        {
                            tree.__FilterTypeNode = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "input") // Looking for property
                                {
                                    tree.FilterTypeNode._input = value;
                                }
                                if(name == "type") // Looking for property
                                {
                                    tree.FilterTypeNode._type = value;
                                }
                                if(name == "includeInherits") // Looking for property
                                {
                                    tree.FilterTypeNode._includeInherits = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.FilterTypeNode._name = value;
                                }
                            }
                        }
                        if(name == "SelectPathNode") // Looking for class
                        {
                            tree.__SelectPathNode = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "input") // Looking for property
                                {
                                    tree.SelectPathNode._input = value;
                                }
                                if(name == "path") // Looking for property
                                {
                                    tree.SelectPathNode._path = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.SelectPathNode._name = value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
