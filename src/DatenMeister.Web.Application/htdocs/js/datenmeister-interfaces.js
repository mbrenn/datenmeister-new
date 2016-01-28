var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports"], function (require, exports) {
    ;
    ;
    var DataTableItem = (function () {
        function DataTableItem() {
            this.uri = "local:///";
            this.v = new Array();
        }
        return DataTableItem;
    })();
    exports.DataTableItem = DataTableItem;
    var ItemInExtentQuery = (function () {
        function ItemInExtentQuery() {
        }
        return ItemInExtentQuery;
    })();
    exports.ItemInExtentQuery = ItemInExtentQuery;
    var PostModels;
    (function (PostModels) {
        /** This class is used to reference a single object within the database */
        var ExtentReferenceModel = (function () {
            function ExtentReferenceModel() {
            }
            return ExtentReferenceModel;
        })();
        PostModels.ExtentReferenceModel = ExtentReferenceModel;
        var ItemReferenceModel = (function (_super) {
            __extends(ItemReferenceModel, _super);
            function ItemReferenceModel() {
                _super.apply(this, arguments);
            }
            return ItemReferenceModel;
        })(ExtentReferenceModel);
        PostModels.ItemReferenceModel = ItemReferenceModel;
        var ItemCreateModel = (function (_super) {
            __extends(ItemCreateModel, _super);
            function ItemCreateModel() {
                _super.apply(this, arguments);
            }
            return ItemCreateModel;
        })(ExtentReferenceModel);
        PostModels.ItemCreateModel = ItemCreateModel;
        var ItemUnsetPropertyModel = (function (_super) {
            __extends(ItemUnsetPropertyModel, _super);
            function ItemUnsetPropertyModel() {
                _super.apply(this, arguments);
            }
            return ItemUnsetPropertyModel;
        })(ItemReferenceModel);
        PostModels.ItemUnsetPropertyModel = ItemUnsetPropertyModel;
        var ItemDeleteModel = (function (_super) {
            __extends(ItemDeleteModel, _super);
            function ItemDeleteModel() {
                _super.apply(this, arguments);
            }
            return ItemDeleteModel;
        })(ItemReferenceModel);
        PostModels.ItemDeleteModel = ItemDeleteModel;
        var ItemSetPropertyModel = (function (_super) {
            __extends(ItemSetPropertyModel, _super);
            function ItemSetPropertyModel() {
                _super.apply(this, arguments);
            }
            return ItemSetPropertyModel;
        })(ItemReferenceModel);
        PostModels.ItemSetPropertyModel = ItemSetPropertyModel;
        var ItemSetPropertiesModel = (function (_super) {
            __extends(ItemSetPropertiesModel, _super);
            function ItemSetPropertiesModel() {
                _super.apply(this, arguments);
            }
            return ItemSetPropertiesModel;
        })(ItemReferenceModel);
        PostModels.ItemSetPropertiesModel = ItemSetPropertiesModel;
    })(PostModels = exports.PostModels || (exports.PostModels = {}));
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
        })();
        Api.FormForItemConfiguration = FormForItemConfiguration;
    })(Api = exports.Api || (exports.Api = {}));
});
//# sourceMappingURL=datenmeister-interfaces.js.map