using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.0.0.0
namespace DatenMeister.Filler
{
    public class FillThePrimitiveTypes
    {
        private static object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject element)
        {
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public static void DoFill(IEnumerable<object> collection, DatenMeister._PrimitiveTypes tree)
        {
            string name;
            IObject value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IObject;
                name = GetNameOfElement(value);
                if (name == "PrimitiveTypes") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IObject;
                        name = GetNameOfElement(value);
                        if(name == "Boolean") // Looking for class
                        {
                            tree.__Boolean = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                            }
                        }
                        if(name == "Integer") // Looking for class
                        {
                            tree.__Integer = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                            }
                        }
                        if(name == "Real") // Looking for class
                        {
                            tree.__Real = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                            }
                        }
                        if(name == "String") // Looking for class
                        {
                            tree.__String = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                            }
                        }
                        if(name == "UnlimitedNatural") // Looking for class
                        {
                            tree.__UnlimitedNatural = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                            }
                        }
                    }
                }
            }
        }
    }
}
