#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Provider.ManagementProviders.Model
{
    public class FillTheManagementProvider : DatenMeister.Core.Filler.IFiller<_ManagementProvider>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject? element)
        {
            if (element == null) throw new System.ArgumentNullException(nameof(element));
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object?> collection, _ManagementProvider tree)
        {
            FillTheManagementProvider.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object?> collection, _ManagementProvider tree)
        {
            string? name;
            IElement? value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement ?? throw new System.InvalidOperationException("value == null");
                name = GetNameOfElement(value);
                if (name == "ManagementProvider") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement ?? throw new System.InvalidOperationException("value == null");
                        name = GetNameOfElement(value);
                        if(name == "Extent") // Looking for class
                        {
                            tree.__Extent = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "uri") // Looking for property
                                {
                                    tree.Extent._uri = value;
                                }
                                if(name == "count") // Looking for property
                                {
                                    tree.Extent._count = value;
                                }
                                if(name == "type") // Looking for property
                                {
                                    tree.Extent._type = value;
                                }
                                if(name == "extentType") // Looking for property
                                {
                                    tree.Extent._extentType = value;
                                }
                                if(name == "isModified") // Looking for property
                                {
                                    tree.Extent._isModified = value;
                                }
                                if(name == "alternativeUris") // Looking for property
                                {
                                    tree.Extent._alternativeUris = value;
                                }
                            }
                        }
                        if(name == "Workspace") // Looking for class
                        {
                            tree.__Workspace = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "id") // Looking for property
                                {
                                    tree.Workspace._id = value;
                                }
                                if(name == "annotation") // Looking for property
                                {
                                    tree.Workspace._annotation = value;
                                }
                                if(name == "extents") // Looking for property
                                {
                                    tree.Workspace._extents = value;
                                }
                            }
                        }
                        if(name == "CreateNewWorkspaceModel") // Looking for class
                        {
                            tree.__CreateNewWorkspaceModel = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "id") // Looking for property
                                {
                                    tree.CreateNewWorkspaceModel._id = value;
                                }
                                if(name == "annotation") // Looking for property
                                {
                                    tree.CreateNewWorkspaceModel._annotation = value;
                                }
                            }
                        }
                        if(name == "ExtentTypeSetting") // Looking for class
                        {
                            tree.__ExtentTypeSetting = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.ExtentTypeSetting._name = value;
                                }
                            }
                        }
                        if(name == "ExtentSettings") // Looking for class
                        {
                            tree.__ExtentSettings = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "extentTypeSettings") // Looking for property
                                {
                                    tree.ExtentSettings._extentTypeSettings = value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
