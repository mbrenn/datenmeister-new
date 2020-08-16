#nullable enable
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.FillClassTreeByExtentCreator Version 1.1.0.0
namespace DatenMeister.Models.Forms
{
    public class FillTheFormAndFields : DatenMeister.Core.Filler.IFiller<_FormAndFields>
    {
        private static readonly object[] EmptyList = new object[] { };
        private static string GetNameOfElement(IObject? element)
        {
            if (element == null) throw new System.ArgumentNullException(nameof(element));
            var nameAsObject = element.get("name");
            return nameAsObject == null ? string.Empty : nameAsObject.ToString();
        }

        public void Fill(IEnumerable<object?> collection, _FormAndFields tree)
        {
            FillTheFormAndFields.DoFill(collection, tree);
        }

        public static void DoFill(IEnumerable<object?> collection, _FormAndFields tree)
        {
            string? name;
            IElement? value;
            bool isSet;
            foreach (var item in collection)
            {
                value = item as IElement ?? throw new System.InvalidOperationException("value == null");
                name = GetNameOfElement(value);
                if (name == "FormAndFields") // Looking for package
                {
                    isSet = value.isSet("packagedElement");
                    collection = isSet ? ((value.get("packagedElement") as IEnumerable<object>) ?? EmptyList) : EmptyList;
                    foreach (var item0 in collection)
                    {
                        value = item0 as IElement ?? throw new System.InvalidOperationException("value == null");
                        name = GetNameOfElement(value);
                        if(name == "FieldData") // Looking for class
                        {
                            tree.__FieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.FieldData._isAttached = value;
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
                        if(name == "SortingOrder") // Looking for class
                        {
                            tree.__SortingOrder = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "field") // Looking for property
                                {
                                    tree.SortingOrder._field = value;
                                }
                                if(name == "isDescending") // Looking for property
                                {
                                    tree.SortingOrder._isDescending = value;
                                }
                            }
                        }
                        if(name == "AnyDataFieldData") // Looking for class
                        {
                            tree.__AnyDataFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.AnyDataFieldData._isAttached = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.AnyDataFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.AnyDataFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.AnyDataFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.AnyDataFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.AnyDataFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "CheckboxFieldData") // Looking for class
                        {
                            tree.__CheckboxFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "lineHeight") // Looking for property
                                {
                                    tree.CheckboxFieldData._lineHeight = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.CheckboxFieldData._isAttached = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.CheckboxFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.CheckboxFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.CheckboxFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.CheckboxFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.CheckboxFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "DateTimeFieldData") // Looking for class
                        {
                            tree.__DateTimeFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "hideDate") // Looking for property
                                {
                                    tree.DateTimeFieldData._hideDate = value;
                                }
                                if(name == "hideTime") // Looking for property
                                {
                                    tree.DateTimeFieldData._hideTime = value;
                                }
                                if(name == "showOffsetButtons") // Looking for property
                                {
                                    tree.DateTimeFieldData._showOffsetButtons = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.DateTimeFieldData._isAttached = value;
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
                        if(name == "FormAssociation") // Looking for class
                        {
                            tree.__FormAssociation = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.FormAssociation._name = value;
                                }
                                if(name == "formType") // Looking for property
                                {
                                    tree.FormAssociation._formType = value;
                                }
                                if(name == "metaClass") // Looking for property
                                {
                                    tree.FormAssociation._metaClass = value;
                                }
                                if(name == "extentType") // Looking for property
                                {
                                    tree.FormAssociation._extentType = value;
                                }
                                if(name == "viewModeId") // Looking for property
                                {
                                    tree.FormAssociation._viewModeId = value;
                                }
                                if(name == "parentMetaClass") // Looking for property
                                {
                                    tree.FormAssociation._parentMetaClass = value;
                                }
                                if(name == "parentProperty") // Looking for property
                                {
                                    tree.FormAssociation._parentProperty = value;
                                }
                                if(name == "form") // Looking for property
                                {
                                    tree.FormAssociation._form = value;
                                }
                            }
                        }
                        if(name == "DropDownFieldData") // Looking for class
                        {
                            tree.__DropDownFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "values") // Looking for property
                                {
                                    tree.DropDownFieldData._values = value;
                                }
                                if(name == "valuesByEnumeration") // Looking for property
                                {
                                    tree.DropDownFieldData._valuesByEnumeration = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.DropDownFieldData._isAttached = value;
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
                        if(name == "ValuePair") // Looking for class
                        {
                            tree.__ValuePair = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
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
                        if(name == "MetaClassElementFieldData") // Looking for class
                        {
                            tree.__MetaClassElementFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.MetaClassElementFieldData._isAttached = value;
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
                        if(name == "ReferenceFieldData") // Looking for class
                        {
                            tree.__ReferenceFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "isSelectionInline") // Looking for property
                                {
                                    tree.ReferenceFieldData._isSelectionInline = value;
                                }
                                if(name == "defaultExtentUri") // Looking for property
                                {
                                    tree.ReferenceFieldData._defaultExtentUri = value;
                                }
                                if(name == "defaultWorkspace") // Looking for property
                                {
                                    tree.ReferenceFieldData._defaultWorkspace = value;
                                }
                                if(name == "showAllChildren") // Looking for property
                                {
                                    tree.ReferenceFieldData._showAllChildren = value;
                                }
                                if(name == "showWorkspaceSelection") // Looking for property
                                {
                                    tree.ReferenceFieldData._showWorkspaceSelection = value;
                                }
                                if(name == "showExtentSelection") // Looking for property
                                {
                                    tree.ReferenceFieldData._showExtentSelection = value;
                                }
                                if(name == "metaClassFilter") // Looking for property
                                {
                                    tree.ReferenceFieldData._metaClassFilter = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.ReferenceFieldData._isAttached = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.ReferenceFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.ReferenceFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.ReferenceFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.ReferenceFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.ReferenceFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "SubElementFieldData") // Looking for class
                        {
                            tree.__SubElementFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "metaClassUri") // Looking for property
                                {
                                    tree.SubElementFieldData._metaClassUri = value;
                                }
                                if(name == "form") // Looking for property
                                {
                                    tree.SubElementFieldData._form = value;
                                }
                                if(name == "defaultTypesForNewElements") // Looking for property
                                {
                                    tree.SubElementFieldData._defaultTypesForNewElements = value;
                                }
                                if(name == "includeSpecializationsForDefaultTypes") // Looking for property
                                {
                                    tree.SubElementFieldData._includeSpecializationsForDefaultTypes = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.SubElementFieldData._isAttached = value;
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
                        if(name == "TextFieldData") // Looking for class
                        {
                            tree.__TextFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "lineHeight") // Looking for property
                                {
                                    tree.TextFieldData._lineHeight = value;
                                }
                                if(name == "width") // Looking for property
                                {
                                    tree.TextFieldData._width = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.TextFieldData._isAttached = value;
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
                        if(name == "SeparatorLineFieldData") // Looking for class
                        {
                            tree.__SeparatorLineFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "Height") // Looking for property
                                {
                                    tree.SeparatorLineFieldData._Height = value;
                                }
                            }
                        }
                        if(name == "FileSelectionFieldData") // Looking for class
                        {
                            tree.__FileSelectionFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "defaultExtension") // Looking for property
                                {
                                    tree.FileSelectionFieldData._defaultExtension = value;
                                }
                                if(name == "isSaving") // Looking for property
                                {
                                    tree.FileSelectionFieldData._isSaving = value;
                                }
                                if(name == "initialPathToDirectory") // Looking for property
                                {
                                    tree.FileSelectionFieldData._initialPathToDirectory = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.FileSelectionFieldData._isAttached = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.FileSelectionFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.FileSelectionFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.FileSelectionFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.FileSelectionFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.FileSelectionFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "DefaultTypeForNewElement") // Looking for class
                        {
                            tree.__DefaultTypeForNewElement = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.DefaultTypeForNewElement._name = value;
                                }
                                if(name == "metaClass") // Looking for property
                                {
                                    tree.DefaultTypeForNewElement._metaClass = value;
                                }
                                if(name == "parentProperty") // Looking for property
                                {
                                    tree.DefaultTypeForNewElement._parentProperty = value;
                                }
                            }
                        }
                        if(name == "FullNameFieldData") // Looking for class
                        {
                            tree.__FullNameFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.FullNameFieldData._isAttached = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.FullNameFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.FullNameFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.FullNameFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.FullNameFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.FullNameFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "CheckboxListTaggingFieldData") // Looking for class
                        {
                            tree.__CheckboxListTaggingFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "values") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._values = value;
                                }
                                if(name == "separator") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._separator = value;
                                }
                                if(name == "containsFreeText") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._containsFreeText = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._isAttached = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.CheckboxListTaggingFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "NumberFieldData") // Looking for class
                        {
                            tree.__NumberFieldData = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "format") // Looking for property
                                {
                                    tree.NumberFieldData._format = value;
                                }
                                if(name == "isInteger") // Looking for property
                                {
                                    tree.NumberFieldData._isInteger = value;
                                }
                                if(name == "isAttached") // Looking for property
                                {
                                    tree.NumberFieldData._isAttached = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.NumberFieldData._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.NumberFieldData._title = value;
                                }
                                if(name == "isEnumeration") // Looking for property
                                {
                                    tree.NumberFieldData._isEnumeration = value;
                                }
                                if(name == "defaultValue") // Looking for property
                                {
                                    tree.NumberFieldData._defaultValue = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.NumberFieldData._isReadOnly = value;
                                }
                            }
                        }
                        if(name == "Form") // Looking for class
                        {
                            tree.__Form = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.Form._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.Form._title = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.Form._isReadOnly = value;
                                }
                                if(name == "hideMetaInformation") // Looking for property
                                {
                                    tree.Form._hideMetaInformation = value;
                                }
                            }
                        }
                        if(name == "DetailForm") // Looking for class
                        {
                            tree.__DetailForm = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "buttonApplyText") // Looking for property
                                {
                                    tree.DetailForm._buttonApplyText = value;
                                }
                                if(name == "allowNewProperties") // Looking for property
                                {
                                    tree.DetailForm._allowNewProperties = value;
                                }
                                if(name == "defaultWidth") // Looking for property
                                {
                                    tree.DetailForm._defaultWidth = value;
                                }
                                if(name == "defaultHeight") // Looking for property
                                {
                                    tree.DetailForm._defaultHeight = value;
                                }
                                if(name == "tab") // Looking for property
                                {
                                    tree.DetailForm._tab = value;
                                }
                                if(name == "field") // Looking for property
                                {
                                    tree.DetailForm._field = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.DetailForm._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.DetailForm._title = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.DetailForm._isReadOnly = value;
                                }
                                if(name == "hideMetaInformation") // Looking for property
                                {
                                    tree.DetailForm._hideMetaInformation = value;
                                }
                            }
                        }
                        if(name == "ListForm") // Looking for class
                        {
                            tree.__ListForm = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "property") // Looking for property
                                {
                                    tree.ListForm._property = value;
                                }
                                if(name == "metaClass") // Looking for property
                                {
                                    tree.ListForm._metaClass = value;
                                }
                                if(name == "includeDescendents") // Looking for property
                                {
                                    tree.ListForm._includeDescendents = value;
                                }
                                if(name == "noItemsWithMetaClass") // Looking for property
                                {
                                    tree.ListForm._noItemsWithMetaClass = value;
                                }
                                if(name == "inhibitNewItems") // Looking for property
                                {
                                    tree.ListForm._inhibitNewItems = value;
                                }
                                if(name == "inhibitDeleteItems") // Looking for property
                                {
                                    tree.ListForm._inhibitDeleteItems = value;
                                }
                                if(name == "defaultTypesForNewElements") // Looking for property
                                {
                                    tree.ListForm._defaultTypesForNewElements = value;
                                }
                                if(name == "fastViewFilters") // Looking for property
                                {
                                    tree.ListForm._fastViewFilters = value;
                                }
                                if(name == "field") // Looking for property
                                {
                                    tree.ListForm._field = value;
                                }
                                if(name == "sortingOrder") // Looking for property
                                {
                                    tree.ListForm._sortingOrder = value;
                                }
                                if(name == "viewNode") // Looking for property
                                {
                                    tree.ListForm._viewNode = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.ListForm._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.ListForm._title = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.ListForm._isReadOnly = value;
                                }
                                if(name == "hideMetaInformation") // Looking for property
                                {
                                    tree.ListForm._hideMetaInformation = value;
                                }
                            }
                        }
                        if(name == "ExtentForm") // Looking for class
                        {
                            tree.__ExtentForm = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "tab") // Looking for property
                                {
                                    tree.ExtentForm._tab = value;
                                }
                                if(name == "autoTabs") // Looking for property
                                {
                                    tree.ExtentForm._autoTabs = value;
                                }
                                if(name == "name") // Looking for property
                                {
                                    tree.ExtentForm._name = value;
                                }
                                if(name == "title") // Looking for property
                                {
                                    tree.ExtentForm._title = value;
                                }
                                if(name == "isReadOnly") // Looking for property
                                {
                                    tree.ExtentForm._isReadOnly = value;
                                }
                                if(name == "hideMetaInformation") // Looking for property
                                {
                                    tree.ExtentForm._hideMetaInformation = value;
                                }
                            }
                        }
                        if(name == "ViewMode") // Looking for class
                        {
                            tree.__ViewMode = value;
                            isSet = value.isSet("ownedAttribute");
                            collection = isSet ? ((value.get("ownedAttribute") as IEnumerable<object>) ?? EmptyList): EmptyList;
                            foreach (var item1 in collection)
                            {
                                value = item1 as IElement ?? throw new System.InvalidOperationException ("Not OfType IElement");
                                name = GetNameOfElement(value);
                                if(name == "name") // Looking for property
                                {
                                    tree.ViewMode._name = value;
                                }
                                if(name == "id") // Looking for property
                                {
                                    tree.ViewMode._id = value;
                                }
                                if(name == "defaultExtentType") // Looking for property
                                {
                                    tree.ViewMode._defaultExtentType = value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
