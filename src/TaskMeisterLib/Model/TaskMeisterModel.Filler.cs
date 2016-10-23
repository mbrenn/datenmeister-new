using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;

// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace TaskMeister.Model
{
    public class FillTheTaskMeisterModel : IFiller<_TaskMeisterModel>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject element)
        {
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object> collection, _TaskMeisterModel tree)
        {
            DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object> collection, _TaskMeisterModel tree)
        {
            string name;
            IElement value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement;
                name = GetNameOfElement(value);
                if (name == "TaskMeisterModel") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement;
                        name = GetNameOfElement(value);
                        if(name == "IActivity") // Looking for class
                        {
                            tree.__IActivity = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "Name") // Looking for property
                                {
                                    tree.IActivity._Name = value;
                                }
                                if(name == "Description") // Looking for property
                                {
                                    tree.IActivity._Description = value;
                                }
                                if(name == "State") // Looking for property
                                {
                                    tree.IActivity._State = value;
                                }
                                if(name == "Created") // Looking for property
                                {
                                    tree.IActivity._Created = value;
                                }
                                if(name == "StartDate") // Looking for property
                                {
                                    tree.IActivity._StartDate = value;
                                }
                                if(name == "FinishDate") // Looking for property
                                {
                                    tree.IActivity._FinishDate = value;
                                }
                                if(name == "EstmatedDuration") // Looking for property
                                {
                                    tree.IActivity._EstmatedDuration = value;
                                }
                            }
                        }
                        if(name == "IPerson") // Looking for class
                        {
                            tree.__IPerson = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.IPerson._name = value;
                                }
                                if(name == "prename") // Looking for property
                                {
                                    tree.IPerson._prename = value;
                                }
                                if(name == "department") // Looking for property
                                {
                                    tree.IPerson._department = value;
                                }
                                if(name == "role") // Looking for property
                                {
                                    tree.IPerson._role = value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
