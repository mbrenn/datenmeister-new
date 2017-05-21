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
define(["require", "exports"], function (require, exports) {
    "use strict";
    exports.__esModule = true;
    var In;
    (function (In) {
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
        In.ItemContentModel = ItemContentModel;
    })(In = exports.In || (exports.In = {}));
    var Out;
    (function (Out) {
        /** This class is used to reference a single object within the database */
        var ExtentReferenceModel = (function () {
            function ExtentReferenceModel() {
            }
            return ExtentReferenceModel;
        }());
        Out.ExtentReferenceModel = ExtentReferenceModel;
        var ItemReferenceModel = (function (_super) {
            __extends(ItemReferenceModel, _super);
            function ItemReferenceModel() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return ItemReferenceModel;
        }(ExtentReferenceModel));
        Out.ItemReferenceModel = ItemReferenceModel;
        var ItemCreateModel = (function (_super) {
            __extends(ItemCreateModel, _super);
            function ItemCreateModel() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return ItemCreateModel;
        }(ExtentReferenceModel));
        Out.ItemCreateModel = ItemCreateModel;
        var ItemUnsetPropertyModel = (function (_super) {
            __extends(ItemUnsetPropertyModel, _super);
            function ItemUnsetPropertyModel() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return ItemUnsetPropertyModel;
        }(ItemReferenceModel));
        Out.ItemUnsetPropertyModel = ItemUnsetPropertyModel;
        var ItemDeleteModel = (function (_super) {
            __extends(ItemDeleteModel, _super);
            function ItemDeleteModel() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return ItemDeleteModel;
        }(ItemReferenceModel));
        Out.ItemDeleteModel = ItemDeleteModel;
        var ItemSetPropertyModel = (function (_super) {
            __extends(ItemSetPropertyModel, _super);
            function ItemSetPropertyModel() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return ItemSetPropertyModel;
        }(ItemReferenceModel));
        Out.ItemSetPropertyModel = ItemSetPropertyModel;
        var ItemSetPropertiesModel = (function (_super) {
            __extends(ItemSetPropertiesModel, _super);
            function ItemSetPropertiesModel() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return ItemSetPropertiesModel;
        }(ItemReferenceModel));
        Out.ItemSetPropertiesModel = ItemSetPropertiesModel;
        var ItemTableQuery = (function () {
            function ItemTableQuery() {
            }
            return ItemTableQuery;
        }());
        Out.ItemTableQuery = ItemTableQuery;
    })(Out = exports.Out || (exports.Out = {}));
});
//# sourceMappingURL=datenmeister-clientinterface.js.map