using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Models.DataViews
{
    public class FillTheDataViews : DatenMeister.Core.Filler.IFiller<_DataViews>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject element)
        {
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object> collection, _DataViews tree)
        {
            FillTheDataViews.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object> collection, _DataViews tree)
        {
            string name;
            IElement value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement;
                name = GetNameOfElement(value);
                if (name == "DataViews") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement;
                        name = GetNameOfElement(value);
                        if(name == "DataView") // Looking for class
                        {
                            tree.__DataView = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
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
                                if(name == "ViewNode") // Looking for property
                                {
                                    tree.DataView._ViewNode = value;
                                }
                            }
                        }
                        if(name == "ViewNode") // Looking for class
                        {
                            tree.__ViewNode = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
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
                    }
                }
            }
        }
    }
}
