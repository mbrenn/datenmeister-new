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
    })(ClientResponse = exports.ClientResponse || (exports.ClientResponse = {}));
    var PostModels;
    (function (PostModels) {
        var ItemInExtentQuery = (function () {
            function ItemInExtentQuery() {
            }
            return ItemInExtentQuery;
        }());
        PostModels.ItemInExtentQuery = ItemInExtentQuery;
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
    var View;
    (function (View) {
        var ItemViewSettings = (function () {
            function ItemViewSettings() {
            }
            return ItemViewSettings;
        }());
        View.ItemViewSettings = ItemViewSettings;
    })(View = exports.View || (exports.View = {}));
    var Table;
    (function (Table) {
        var DataField = (function () {
            function DataField(title, name) {
                this.type = ColumnTypes.textbox;
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
                _super.call(this, title, name);
                this.type = ColumnTypes.textbox;
                this.lineHeight = 1;
            }
            return TextDataField;
        }(DataField));
        Table.TextDataField = TextDataField;
        var DateTimeDataField = (function (_super) {
            __extends(DateTimeDataField, _super);
            function DateTimeDataField(title, name) {
                _super.call(this, title, name);
                this.type = ColumnTypes.dropdown;
                this.showDate = true;
                this.showTime = true;
            }
            return DateTimeDataField;
        }(DataField));
        Table.DateTimeDataField = DateTimeDataField;
        var DropDownDataField = (function (_super) {
            __extends(DropDownDataField, _super);
            function DropDownDataField(title, name) {
                _super.call(this, title, name);
                this.type = ColumnTypes.dropdown;
            }
            return DropDownDataField;
        }(DataField));
        Table.DropDownDataField = DropDownDataField;
        var ColumnTypes = (function () {
            function ColumnTypes() {
            }
            ColumnTypes.textbox = "textbox";
            ColumnTypes.dropdown = "dropdown";
            ColumnTypes.dateTime = "datetime";
            return ColumnTypes;
        }());
        Table.ColumnTypes = ColumnTypes;
        var DataTableItem = (function () {
            function DataTableItem() {
                this.uri = "local:///";
                this.v = new Array();
            }
            return DataTableItem;
        }());
        Table.DataTableItem = DataTableItem;
    })(Table = exports.Table || (exports.Table = {}));
    var Api;
    (function (Api) {
        var FormForItemConfiguration = (function () {
            function FormForItemConfiguration() {
                this.columns = new Array();
            }
            FormForItemConfiguration.prototype.addColumn = function (column) {
                this.columns[this.columns.length] = column;
            };
            return FormForItemConfiguration;
        }());
        Api.FormForItemConfiguration = FormForItemConfiguration;
        var DialogConfiguration = (function (_super) {
            __extends(DialogConfiguration, _super);
            function DialogConfiguration() {
                _super.apply(this, arguments);
            }
            return DialogConfiguration;
        }(FormForItemConfiguration));
        Api.DialogConfiguration = DialogConfiguration;
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