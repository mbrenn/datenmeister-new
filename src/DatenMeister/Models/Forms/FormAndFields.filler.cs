using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Models.Forms
{
    public class FillTheFormAndFields : DatenMeister.Core.Filler.IFiller<_FormAndFields>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject element)
        {
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object> collection, _FormAndFields tree)
        {
            FillTheFormAndFields.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object> collection, _FormAndFields tree)
        {
            string name;
            IElement value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement;
                name = GetNameOfElement(value);
                if (name == "FormAndFields") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? (value.get("packagedElement") as IEnumerable<object>) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement;
                        name = GetNameOfElement(value);
                        if(name == "Form") // Looking for class
                        {
                            tree.__Form = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.Form._name = value;
                                }
                                if(name == "fields") // Looking for property
                                {
                                    tree.Form._fields = value;
                                }
                                if(name == "fixView") // Looking for property
                                {
                                    tree.Form._fixView = value;
                                }
                                if(name == "inhibitNewItems") // Looking for property
                                {
                                    tree.Form._inhibitNewItems = value;
                                }
                                if(name == "detailForm") // Looking for property
                                {
                                    tree.Form._detailForm = value;
                                }
                                if(name == "hideMetaClass") // Looking for property
                                {
                                    tree.Form._hideMetaClass = value;
                                }
                                if(name == "minimizeDesign") // Looking for property
                                {
                                    tree.Form._minimizeDesign = value;
                                }
                                if(name == "defaultWidth") // Looking for property
                                {
                                    tree.Form._defaultWidth = value;
                                }
                                if(name == "defaultHeight") // Looking for property
                                {
                                    tree.Form._defaultHeight = value;
                                }
                            }
                        }
                        if(name == "FieldData") // Looking for class
                        {
                            tree.__FieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "fieldType") // Looking for property
                                {
                                    tree.FieldData._fieldType = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.FieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.FieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.FieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.FieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.FieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "TextFieldData") // Looking for class
                        {
                            tree.__TextFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "lineHeight") // Looking for property
                                {
                                    tree.TextFieldData._lineHeight = value;
                                }
                                if(name == "fieldType") // Looking for property
                                {
                                    tree.TextFieldData._fieldType = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.TextFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.TextFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.TextFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.TextFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.TextFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "DateTimeFieldData") // Looking for class
                        {
                            tree.__DateTimeFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "showDate") // Looking for property
                                {
                                    tree.DateTimeFieldData._showDate = value;
                                }
                                if(name == "showTime") // Looking for property
                                {
                                    tree.DateTimeFieldData._showTime = value;
                                }
                                if(name == "showOffsetButtons") // Looking for property
                                {
                                    tree.DateTimeFieldData._showOffsetButtons = value;
                                }
                                if(name == "fieldType") // Looking for property
                                {
                                    tree.DateTimeFieldData._fieldType = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.DateTimeFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.DateTimeFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.DateTimeFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.DateTimeFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.DateTimeFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "DropDownFieldData") // Looking for class
                        {
                            tree.__DropDownFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "values") // Looking for property
                                {
                                    tree.DropDownFieldData._values = value;
                                }
                                if(name == "fieldType") // Looking for property
                                {
                                    tree.DropDownFieldData._fieldType = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.DropDownFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.DropDownFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.DropDownFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.DropDownFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.DropDownFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "SubElementFieldData") // Looking for class
                        {
                            tree.__SubElementFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "metaClassUri") // Looking for property
                                {
                                    tree.SubElementFieldData._metaClassUri = value;
                                }
                                if(name == "form") // Looking for property
                                {
                                    tree.SubElementFieldData._form = value;
                                }
                                if(name == "fieldType") // Looking for property
                                {
                                    tree.SubElementFieldData._fieldType = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.SubElementFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.SubElementFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.SubElementFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.SubElementFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.SubElementFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "DefaultViewForMetaclass") // Looking for class
                        {
                            tree.__DefaultViewForMetaclass = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "viewType") // Looking for property
                                {
                                    tree.DefaultViewForMetaclass._viewType = value;
                                }
                                if(name == "metaclass") // Looking for property
                                {
                                    tree.DefaultViewForMetaclass._metaclass = value;
                                }
                                if(name == "view") // Looking for property
                                {
                                    tree.DefaultViewForMetaclass._view = value;
                                }
                            }
                        }
                        if(name == "DefaultViewForExtentType") // Looking for class
                        {
                            tree.__DefaultViewForExtentType = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "extentType") // Looking for property
                                {
                                    tree.DefaultViewForExtentType._extentType = value;
                                }
                                if(name == "view") // Looking for property
                                {
                                    tree.DefaultViewForExtentType._view = value;
                                }
                            }
                        }
                        if(name == "MetaClassElementFieldData") // Looking for class
                        {
                            tree.__MetaClassElementFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "fieldType") // Looking for property
                                {
                                    tree.MetaClassElementFieldData._fieldType = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.MetaClassElementFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.MetaClassElementFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.MetaClassElementFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.MetaClassElementFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.MetaClassElementFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "ValuePair") // Looking for class
                        {
                            tree.__ValuePair = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? (value.get("ownedAttribute") as IEnumerable<object>) : EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement;
                                name = GetNameOfElement(value);
                                if(name == "value") // Looking for property
                                {
                                    tree.ValuePair._value = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.ValuePair._name = value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
