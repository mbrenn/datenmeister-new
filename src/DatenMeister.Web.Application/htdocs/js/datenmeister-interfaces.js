var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports"], function (require, exports) {
    "use strict";
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
                _super.apply(this, arguments);
            }
            return ItemReferenceModel;
        }(ExtentReferenceModel));
        PostModels.ItemReferenceModel = ItemReferenceModel;
        var ItemCreateModel = (function (_super) {
            __extends(ItemCreateModel, _super);
            function ItemCreateModel() {
                _super.apply(this, arguments);
            }
            return ItemCreateModel;
        }(ExtentReferenceModel));
        PostModels.ItemCreateModel = ItemCreateModel;
        var ItemUnsetPropertyModel = (function (_super) {
            __extends(ItemUnsetPropertyModel, _super);
            function ItemUnsetPropertyModel() {
                _super.apply(this, arguments);
            }
            return ItemUnsetPropertyModel;
        }(ItemReferenceModel));
        PostModels.ItemUnsetPropertyModel = ItemUnsetPropertyModel;
        var ItemDeleteModel = (function (_super) {
            __extends(ItemDeleteModel, _super);
            function ItemDeleteModel() {
                _super.apply(this, arguments);
            }
            return ItemDeleteModel;
        }(ItemReferenceModel));
        PostModels.ItemDeleteModel = ItemDeleteModel;
        var ItemSetPropertyModel = (function (_super) {
            __extends(ItemSetPropertyModel, _super);
            function ItemSetPropertyModel() {
                _super.apply(this, arguments);
            }
            return ItemSetPropertyModel;
        }(ItemReferenceModel));
        PostModels.ItemSetPropertyModel = ItemSetPropertyModel;
        var ItemSetPropertiesModel = (function (_super) {
            __extends(ItemSetPropertiesModel, _super);
            function ItemSetPropertiesModel() {
                _super.apply(this, arguments);
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
                _super.call(this, ColumnTypes.textbox, title, name);
                this.lineHeight = 1;
            }
            return TextDataField;
        }(DataField));
        Table.TextDataField = TextDataField;
        var DateTimeDataField = (function (_super) {
            __extends(DateTimeDataField, _super);
            function DateTimeDataField(title, name) {
                _super.call(this, ColumnTypes.dateTime, title, name);
                this.showDate = true;
                this.showTime = true;
            }
            return DateTimeDataField;
        }(DataField));
        Table.DateTimeDataField = DateTimeDataField;
        var DropDownDataField = (function (_super) {
            __extends(DropDownDataField, _super);
            function DropDownDataField(title, name) {
                _super.call(this, ColumnTypes.dropdown, title, name);
            }
            return DropDownDataField;
        }(DataField));
        Table.DropDownDataField = DropDownDataField;
        var SubElementsDataField = (function (_super) {
            __extends(SubElementsDataField, _super);
            function SubElementsDataField(title, name) {
                _super.call(this, ColumnTypes.subElements, title, name);
            }
            return SubElementsDataField;
        }(DataField));
        Table.SubElementsDataField = SubElementsDataField;
        var ColumnTypes = (function () {
            function ColumnTypes() {
            }
            ColumnTypes.textbox = "text";
            ColumnTypes.dropdown = "dropdown";
            ColumnTypes.dateTime = "datetime";
            ColumnTypes.subElements = "subelements";
            return ColumnTypes;
        }());
        Table.ColumnTypes = ColumnTypes;
    })(Table = exports.Table || (exports.Table = {}));
    var Api;
    (function (Api) {
        var ItemInExtentQuery = (function () {
            function ItemInExtentQuery() {
            }
            return ItemInExtentQuery;
        }());
        Api.ItemInExtentQuery = ItemInExtentQuery;
        (function (PageType) {
            PageType[PageType["Workspaces"] = 0] = "Workspaces";
            PageType[PageType["Extents"] = 1] = "Extents";
            PageType[PageType["Items"] = 2] = "Items";
            PageType[PageType["ItemDetail"] = 3] = "ItemDetail";
            PageType[PageType["Dialog"] = 4] = "Dialog";
        })(Api.PageType || (Api.PageType = {}));
        var PageType = Api.PageType;
        var PluginParameter = (function () {
            function PluginParameter() {
            }
            return PluginParameter;
        }());
        Api.PluginParameter = PluginParameter;
    })(Api = exports.Api || (exports.Api = {}));
});
//# sourceMappingURL=datenmeister-interfaces.js.map