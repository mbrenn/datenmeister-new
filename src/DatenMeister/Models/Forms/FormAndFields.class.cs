using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models.Forms
{
    public class _FormAndFields
    {
        public class _Form
        {
            public static string @name = "name";
            public IElement _name = null;

            public static string @fields = "fields";
            public IElement _fields = null;

            public static string @fixView = "fixView";
            public IElement _fixView = null;

            public static string @inhibitNewItems = "inhibitNewItems";
            public IElement _inhibitNewItems = null;

            public static string @detailForm = "detailForm";
            public IElement _detailForm = null;

            public static string @hideMetaClass = "hideMetaClass";
            public IElement _hideMetaClass = null;

            public static string @minimizeDesign = "minimizeDesign";
            public IElement _minimizeDesign = null;

            public static string @defaultWidth = "defaultWidth";
            public IElement _defaultWidth = null;

            public static string @defaultHeight = "defaultHeight";
            public IElement _defaultHeight = null;

        }

        public _Form @Form = new _Form();
        public IElement @__Form = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.Form");

        public class _DetailForm
        {
            public static string @name = "name";
            public IElement _name = null;

            public static string @fields = "fields";
            public IElement _fields = null;

            public static string @fixView = "fixView";
            public IElement _fixView = null;

            public static string @inhibitNewItems = "inhibitNewItems";
            public IElement _inhibitNewItems = null;

            public static string @detailForm = "detailForm";
            public IElement _detailForm = null;

            public static string @hideMetaClass = "hideMetaClass";
            public IElement _hideMetaClass = null;

            public static string @minimizeDesign = "minimizeDesign";
            public IElement _minimizeDesign = null;

            public static string @defaultWidth = "defaultWidth";
            public IElement _defaultWidth = null;

            public static string @defaultHeight = "defaultHeight";
            public IElement _defaultHeight = null;

        }

        public _DetailForm @DetailForm = new _DetailForm();
        public IElement @__DetailForm = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.DetailForm");

        public class _ListForm
        {
            public static string @defaultTypesForNewElements = "defaultTypesForNewElements";
            public IElement _defaultTypesForNewElements = null;

            public static string @name = "name";
            public IElement _name = null;

            public static string @fields = "fields";
            public IElement _fields = null;

            public static string @fixView = "fixView";
            public IElement _fixView = null;

            public static string @inhibitNewItems = "inhibitNewItems";
            public IElement _inhibitNewItems = null;

            public static string @detailForm = "detailForm";
            public IElement _detailForm = null;

            public static string @hideMetaClass = "hideMetaClass";
            public IElement _hideMetaClass = null;

            public static string @minimizeDesign = "minimizeDesign";
            public IElement _minimizeDesign = null;

            public static string @defaultWidth = "defaultWidth";
            public IElement _defaultWidth = null;

            public static string @defaultHeight = "defaultHeight";
            public IElement _defaultHeight = null;

        }

        public _ListForm @ListForm = new _ListForm();
        public IElement @__ListForm = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.ListForm");

        public class _FieldData
        {
            public static string @fieldType = "fieldType";
            public IElement _fieldType = null;

            public static string @name = "name";
            public IElement _name = null;

            public static string @title = "title";
            public IElement _title = null;

            public static string @isEnumeration = "isEnumeration";
            public IElement _isEnumeration = null;

            public static string @defaultValue = "defaultValue";
            public IElement _defaultValue = null;

            public static string @isReadOnly = "isReadOnly";
            public IElement _isReadOnly = null;

        }

        public _FieldData @FieldData = new _FieldData();
        public IElement @__FieldData = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.FieldData");

        public class _TextFieldData
        {
            public static string @lineHeight = "lineHeight";
            public IElement _lineHeight = null;

            public static string @fieldType = "fieldType";
            public IElement _fieldType = null;

            public static string @name = "name";
            public IElement _name = null;

            public static string @title = "title";
            public IElement _title = null;

            public static string @isEnumeration = "isEnumeration";
            public IElement _isEnumeration = null;

            public static string @defaultValue = "defaultValue";
            public IElement _defaultValue = null;

            public static string @isReadOnly = "isReadOnly";
            public IElement _isReadOnly = null;

        }

        public _TextFieldData @TextFieldData = new _TextFieldData();
        public IElement @__TextFieldData = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.TextFieldData");

        public class _DateTimeFieldData
        {
            public static string @showDate = "showDate";
            public IElement _showDate = null;

            public static string @showTime = "showTime";
            public IElement _showTime = null;

            public static string @showOffsetButtons = "showOffsetButtons";
            public IElement _showOffsetButtons = null;

            public static string @fieldType = "fieldType";
            public IElement _fieldType = null;

            public static string @name = "name";
            public IElement _name = null;

            public static string @title = "title";
            public IElement _title = null;

            public static string @isEnumeration = "isEnumeration";
            public IElement _isEnumeration = null;

            public static string @defaultValue = "defaultValue";
            public IElement _defaultValue = null;

            public static string @isReadOnly = "isReadOnly";
            public IElement _isReadOnly = null;

        }

        public _DateTimeFieldData @DateTimeFieldData = new _DateTimeFieldData();
        public IElement @__DateTimeFieldData = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.DateTimeFieldData");

        public class _DropDownFieldData
        {
            public static string @values = "values";
            public IElement _values = null;

            public static string @fieldType = "fieldType";
            public IElement _fieldType = null;

            public static string @name = "name";
            public IElement _name = null;

            public static string @title = "title";
            public IElement _title = null;

            public static string @isEnumeration = "isEnumeration";
            public IElement _isEnumeration = null;

            public static string @defaultValue = "defaultValue";
            public IElement _defaultValue = null;

            public static string @isReadOnly = "isReadOnly";
            public IElement _isReadOnly = null;

        }

        public _DropDownFieldData @DropDownFieldData = new _DropDownFieldData();
        public IElement @__DropDownFieldData = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.DropDownFieldData");

        public class _SubElementFieldData
        {
            public static string @metaClassUri = "metaClassUri";
            public IElement _metaClassUri = null;

            public static string @form = "form";
            public IElement _form = null;

            public static string @defaultTypesForNewElements = "defaultTypesForNewElements";
            public IElement _defaultTypesForNewElements = null;

            public static string @fieldType = "fieldType";
            public IElement _fieldType = null;

            public static string @name = "name";
            public IElement _name = null;

            public static string @title = "title";
            public IElement _title = null;

            public static string @isEnumeration = "isEnumeration";
            public IElement _isEnumeration = null;

            public static string @defaultValue = "defaultValue";
            public IElement _defaultValue = null;

            public static string @isReadOnly = "isReadOnly";
            public IElement _isReadOnly = null;

        }

        public _SubElementFieldData @SubElementFieldData = new _SubElementFieldData();
        public IElement @__SubElementFieldData = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.SubElementFieldData");

        public class _DefaultViewForMetaclass
        {
            public static string @viewType = "viewType";
            public IElement _viewType = null;

            public static string @metaclass = "metaclass";
            public IElement _metaclass = null;

            public static string @view = "view";
            public IElement _view = null;

        }

        public _DefaultViewForMetaclass @DefaultViewForMetaclass = new _DefaultViewForMetaclass();
        public IElement @__DefaultViewForMetaclass = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.DefaultViewForMetaclass");

        public class _DefaultViewForExtentType
        {
            public static string @extentType = "extentType";
            public IElement _extentType = null;

            public static string @view = "view";
            public IElement _view = null;

        }

        public _DefaultViewForExtentType @DefaultViewForExtentType = new _DefaultViewForExtentType();
        public IElement @__DefaultViewForExtentType = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.DefaultViewForExtentType");

        public class _MetaClassElementFieldData
        {
            public static string @fieldType = "fieldType";
            public IElement _fieldType = null;

            public static string @name = "name";
            public IElement _name = null;

            public static string @title = "title";
            public IElement _title = null;

            public static string @isEnumeration = "isEnumeration";
            public IElement _isEnumeration = null;

            public static string @defaultValue = "defaultValue";
            public IElement _defaultValue = null;

            public static string @isReadOnly = "isReadOnly";
            public IElement _isReadOnly = null;

        }

        public _MetaClassElementFieldData @MetaClassElementFieldData = new _MetaClassElementFieldData();
        public IElement @__MetaClassElementFieldData = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.MetaClassElementFieldData");

        public class _ValuePair
        {
            public static string @value = "value";
            public IElement _value = null;

            public static string @name = "name";
            public IElement _name = null;

        }

        public _ValuePair @ValuePair = new _ValuePair();
        public IElement @__ValuePair = new MofObjectShadow("dm:///DatenMeister/Types/FormAndFields#DatenMeister.Models.Forms.DropDownFieldData%2BValuePair");

        public static _FormAndFields TheOne = new _FormAndFields();

    }

}
