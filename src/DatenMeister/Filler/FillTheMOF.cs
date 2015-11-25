using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.0.0.0
namespace DatenMeister.Filler
{
    public class FillTheMOF
    {
        private static string GetNameOfElement(IObject element)
        {
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public static void DoFill(IEnumerable<object> collection, DatenMeister._MOF tree)
        {
            string name;
            IObject value;
            foreach (var item in collection)
            {
                value = item as IObject;
                name = GetNameOfElement(value);
                if (name == "MOF") // Looking for package
                {
                    collection = value.get("packagedElement") as IEnumerable<object>;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IObject;
                        name = GetNameOfElement(value);
                        if (name == "Identifiers") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "URIExtent") // Looking for class
                                {
                                    tree.Identifiers.@URIExtentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Extent") // Looking for class
                                {
                                    tree.Identifiers.@ExtentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                            }
                        }
                        if (name == "EMOF") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                            }
                        }
                        if (name == "CMOFExtension") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Tag") // Looking for class
                                {
                                    tree.CMOFExtension.@TagInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "tagOwner") // Looking for property
                                        {
                                            tree.CMOFExtension.Tag.@tagOwner = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Extension") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Tag") // Looking for class
                                {
                                    tree.Extension.@TagInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "name") // Looking for property
                                        {
                                            tree.Extension.Tag.@name = value;
                                        }
                                        if (name == "value") // Looking for property
                                        {
                                            tree.Extension.Tag.@value = value;
                                        }
                                        if (name == "element") // Looking for property
                                        {
                                            tree.Extension.Tag.@element = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Common") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "ReflectiveSequence") // Looking for class
                                {
                                    tree.Common.@ReflectiveSequenceInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "ReflectiveCollection") // Looking for class
                                {
                                    tree.Common.@ReflectiveCollectionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                            }
                        }
                        if (name == "CMOF") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                            }
                        }
                        if (name == "CMOFReflection") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Factory") // Looking for class
                                {
                                    tree.CMOFReflection.@FactoryInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Element") // Looking for class
                                {
                                    tree.CMOFReflection.@ElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Argument") // Looking for class
                                {
                                    tree.CMOFReflection.@ArgumentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "name") // Looking for property
                                        {
                                            tree.CMOFReflection.Argument.@name = value;
                                        }
                                        if (name == "value") // Looking for property
                                        {
                                            tree.CMOFReflection.Argument.@value = value;
                                        }
                                    }
                                }
                                if (name == "Extent") // Looking for class
                                {
                                    tree.CMOFReflection.@ExtentInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Link") // Looking for class
                                {
                                    tree.CMOFReflection.@LinkInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "firstElement") // Looking for property
                                        {
                                            tree.CMOFReflection.Link.@firstElement = value;
                                        }
                                        if (name == "secondElement") // Looking for property
                                        {
                                            tree.CMOFReflection.Link.@secondElement = value;
                                        }
                                        if (name == "association") // Looking for property
                                        {
                                            tree.CMOFReflection.Link.@association = value;
                                        }
                                    }
                                }
                                if (name == "Exception") // Looking for class
                                {
                                    tree.CMOFReflection.@ExceptionInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "objectInError") // Looking for property
                                        {
                                            tree.CMOFReflection.Exception.@objectInError = value;
                                        }
                                        if (name == "elementInError") // Looking for property
                                        {
                                            tree.CMOFReflection.Exception.@elementInError = value;
                                        }
                                        if (name == "description") // Looking for property
                                        {
                                            tree.CMOFReflection.Exception.@description = value;
                                        }
                                    }
                                }
                            }
                        }
                        if (name == "Reflection") // Looking for package
                        {
                            collection = value.get("packagedElement") as IEnumerable<object>;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IObject;
                                name = GetNameOfElement(value);
                                if (name == "Factory") // Looking for class
                                {
                                    tree.Reflection.@FactoryInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "package") // Looking for property
                                        {
                                            tree.Reflection.Factory.@package = value;
                                        }
                                    }
                                }
                                if (name == "Type") // Looking for class
                                {
                                    tree.Reflection.@TypeInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Object") // Looking for class
                                {
                                    tree.Reflection.@ObjectInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                    }
                                }
                                if (name == "Element") // Looking for class
                                {
                                    tree.Reflection.@ElementInstance = value;
                                    collection = value.get("ownedAttribute") as IEnumerable<object>;
                                    foreach (var item2 in collection)
                                    {
                                        value = item2 as IObject;
                                        name = GetNameOfElement(value);
                                        if (name == "metaclass") // Looking for property
                                        {
                                            tree.Reflection.Element.@metaclass = value;
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
