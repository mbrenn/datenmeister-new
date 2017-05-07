using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0 created at 07.05.2017 11:58:21
namespace DatenMeister.Core.Filler
{
    public class FillTheMOF : DatenMeister.Core.Filler.IFiller<DatenMeister.Core._MOF>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject element)
        {
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object> collection, DatenMeister.Core._MOF tree)
        {
            FillTheMOF.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object> collection, DatenMeister.Core._MOF tree)
        {
            string name;
            IElement value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement;
                name = GetNameOfElement(value);
                if (name == "MOF") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement;
                        name = GetNameOfElement(value);
                        if (name == "Identifiers") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "URIExtent") // Looking for class
                                {
                                    tree.Identifiers.__URIExtent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Extent") // Looking for class
                                {
                                    tree.Identifiers.__Extent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                            }
                        }
                        if (name == "EMOF") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                            }
                        }
                        if (name == "CMOFExtension") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "Tag") // Looking for class
                                {
                                    tree.CMOFExtension.__Tag = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "tagOwner") // Looking for property
                                        {
                                            tree.CMOFExtension.Tag._tagOwner = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Extension") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "Tag") // Looking for class
                                {
                                    tree.Extension.__Tag = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "name") // Looking for property
                                        {
                                            tree.Extension.Tag._name = value;
                                        }
                                        if(name == "value") // Looking for property
                                        {
                                            tree.Extension.Tag._value = value;
                                        }
                                        if(name == "element") // Looking for property
                                        {
                                            tree.Extension.Tag._element = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Common") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "ReflectiveSequence") // Looking for class
                                {
                                    tree.Common.__ReflectiveSequence = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "ReflectiveCollection") // Looking for class
                                {
                                    tree.Common.__ReflectiveCollection = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                            }
                        }
                        if (name == "CMOF") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                            }
                        }
                        if (name == "CMOFReflection") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "Factory") // Looking for class
                                {
                                    tree.CMOFReflection.__Factory = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Element") // Looking for class
                                {
                                    tree.CMOFReflection.__Element = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Argument") // Looking for class
                                {
                                    tree.CMOFReflection.__Argument = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "name") // Looking for property
                                        {
                                            tree.CMOFReflection.Argument._name = value;
                                        }
                                        if(name == "value") // Looking for property
                                        {
                                            tree.CMOFReflection.Argument._value = value;
                                        }
                                    }
                                }
                                if(name == "Extent") // Looking for class
                                {
                                    tree.CMOFReflection.__Extent = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Link") // Looking for class
                                {
                                    tree.CMOFReflection.__Link = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "firstElement") // Looking for property
                                        {
                                            tree.CMOFReflection.Link._firstElement = value;
                                        }
                                        if(name == "secondElement") // Looking for property
                                        {
                                            tree.CMOFReflection.Link._secondElement = value;
                                        }
                                        if(name == "association") // Looking for property
                                        {
                                            tree.CMOFReflection.Link._association = value;
                                        }
                                    }
                                }
                                if(name == "Exception") // Looking for class
                                {
                                    tree.CMOFReflection.__Exception = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "objectInError") // Looking for property
                                        {
                                            tree.CMOFReflection.Exception._objectInError = value;
                                        }
                                        if(name == "elementInError") // Looking for property
                                        {
                                            tree.CMOFReflection.Exception._elementInError = value;
                                        }
                                        if(name == "description") // Looking for property
                                        {
                                            tree.CMOFReflection.Exception._description = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Reflection") // Looking for package
                        {
                            isSet = value.isSet("packagedElement");
                            collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "Factory") // Looking for class
                                {
                                    tree.Reflection.__Factory = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "package") // Looking for property
                                        {
                                            tree.Reflection.Factory._package = value;
                                        }
                                    }
                                }
                                if(name == "Type") // Looking for class
                                {
                                    tree.Reflection.__Type = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Object") // Looking for class
                                {
                                    tree.Reflection.__Object = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if(name == "Element") // Looking for class
                                {
                                    tree.Reflection.__Element = value;
                                    isSet = value.isSet("ownedAttribute");
                                    collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IElement;
                                        name = GetNameOfElement(value);
                                        if(name == "metaclass") // Looking for property
                                        {
                                            tree.Reflection.Element._metaclass = value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
