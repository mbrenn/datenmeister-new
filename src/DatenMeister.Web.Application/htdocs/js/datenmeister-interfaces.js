"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
exports.__esModule = true;
var ClientResponse;
(function (ClientResponse) {
    ;
    ;
    /**
     * Implements the IItemModel interface by containing all the fields
     */
    var ItemContentModel = (function () {
        function ItemContentModel() {
            this.v = [];
        }
        return ItemContentModel;
    }());
    ClientResponse.ItemContentModel = ItemContentModel;
})(ClientResponse = exports.ClientResponse || (exports.ClientResponse = {}));
var PostModels;
(function (PostModels) {
    /** This class is used to reference a single object within the database */
    var ExtentReferenceModel = (function () {
        function ExtentReferenceModel() {
        }
        return ExtentReferenceModel;
    }());
    PostModels.ExtentReferenceModel = ExtentReferenceModel;
    var ItemReferenceModel = (function (_super) {
        __extends(ItemReferenceModel, _super);
        function ItemReferenceModel() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return ItemReferenceModel;
    }(ExtentReferenceModel));
    PostModels.ItemReferenceModel = ItemReferenceModel;
    var ItemCreateModel = (function (_super) {
        __extends(ItemCreateModel, _super);
        function ItemCreateModel() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return ItemCreateModel;
    }(ExtentReferenceModel));
    PostModels.ItemCreateModel = ItemCreateModel;
    var ItemUnsetPropertyModel = (function (_super) {
        __extends(ItemUnsetPropertyModel, _super);
        function ItemUnsetPropertyModel() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return ItemUnsetPropertyModel;
    }(ItemReferenceModel));
    PostModels.ItemUnsetPropertyModel = ItemUnsetPropertyModel;
    var ItemDeleteModel = (function (_super) {
        __extends(ItemDeleteModel, _super);
        function ItemDeleteModel() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return ItemDeleteModel;
    }(ItemReferenceModel));
    PostModels.ItemDeleteModel = ItemDeleteModel;
    var ItemSetPropertyModel = (function (_super) {
        __extends(ItemSetPropertyModel, _super);
        function ItemSetPropertyModel() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return ItemSetPropertyModel;
    }(ItemReferenceModel));
    PostModels.ItemSetPropertyModel = ItemSetPropertyModel;
    var ItemSetPropertiesModel = (function (_super) {
        __extends(ItemSetPropertiesModel, _super);
        function ItemSetPropertiesModel() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        return ItemSetPropertiesModel;
    }(ItemReferenceModel));
    PostModels.ItemSetPropertiesModel = ItemSetPropertiesModel;
})(PostModels = exports.PostModels || (exports.PostModels = {}));
var Table;
(function (Table) {
    var DataField = (function () {
        function DataField(fieldType, title, name) {
            this.fieldType = fieldType;
            this.title = title;
            this.name = name;
        }
        DataField.prototype.withDefaultValue = function (value) {
            this.defaultValue = value;
            return this;
        };
        DataField.prototype.asReadOnly = function () {
            this.isReadOnly = true;
            return this;
        };
        return DataField;
    }());
    Table.DataField = DataField;
    var TextDataField = (function (_super) {
        __extends(TextDataField, _super);
        function TextDataField(title, name) {
            var _this = _super.call(this, ColumnTypes.textbox, title, name) || this;
            _this.lineHeight = 1;
            return _this;
        }
        return TextDataField;
    }(DataField));
    Table.TextDataField = TextDataField;
    var DateTimeDataField = (function (_super) {
        __extends(DateTimeDataField, _super);
        function DateTimeDataField(title, name) {
            var _this = _super.call(this, ColumnTypes.dateTime, title, name) || this;
            _this.showDate = true;
            _this.showTime = true;
            return _this;
        }
        return DateTimeDataField;
    }(DataField));
    Table.DateTimeDataField = DateTimeDataField;
    var DropDownDataField = (function (_super) {
        __extends(DropDownDataField, _super);
        function DropDownDataField(title, name) {
            return _super.call(this, ColumnTypes.dropdown, title, name) || this;
        }
        return DropDownDataField;
    }(DataField));
    Table.DropDownDataField = DropDownDataField;
    var SubElementsDataField = (function (_super) {
        __extends(SubElementsDataField, _super);
        function SubElementsDataField(title, name) {
            return _super.call(this, ColumnTypes.subElements, title, name) || this;
        }
        return SubElementsDataField;
    }(DataField));
    Table.SubElementsDataField = SubElementsDataField;
    var ColumnTypes = (function () {
        function ColumnTypes() {
        }
        return ColumnTypes;
    }());
    ColumnTypes.textbox = "text";
    ColumnTypes.dropdown = "dropdown";
    ColumnTypes.dateTime = "datetime";
    ColumnTypes.subElements = "subelements";
    Table.ColumnTypes = ColumnTypes;
})(Table = exports.Table || (exports.Table = {}));
var Api;
(function (Api) {
    var ItemTableQuery = (function () {
        function ItemTableQuery() {
        }
        return ItemTableQuery;
    }());
    Api.ItemTableQuery = ItemTableQuery;
    var PageType;
    (function (PageType) {
        PageType[PageType["Workspaces"] = 0] = "Workspaces";
        PageType[PageType["Extents"] = 1] = "Extents";
        PageType[PageType["Items"] = 2] = "Items";
        PageType[PageType["ItemDetail"] = 3] = "ItemDetail";
        PageType[PageType["Dialog"] = 4] = "Dialog";
    })(PageType = Api.PageType || (Api.PageType = {}));
    var PluginParameter = (function () {
        function PluginParameter() {
        }
        return PluginParameter;
    }());
    Api.PluginParameter = PluginParameter;
})(Api = exports.Api || (exports.Api = {}));
//# sourceMappingURL=datenmeister-interfaces.js.map