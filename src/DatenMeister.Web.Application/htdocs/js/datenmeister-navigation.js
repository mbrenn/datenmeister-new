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
var FormForItemConfiguration = (function () {
    function FormForItemConfiguration() {
        this.columns = new Array();
    }
    FormForItemConfiguration.prototype.addColumn = function (column) {
        this.columns[this.columns.length] = column;
    };
    return FormForItemConfiguration;
}());
exports.FormForItemConfiguration = FormForItemConfiguration;
var DialogConfiguration = (function (_super) {
    __extends(DialogConfiguration, _super);
    function DialogConfiguration() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return DialogConfiguration;
}(FormForItemConfiguration));
exports.DialogConfiguration = DialogConfiguration;
var ItemViewSettings = (function () {
    function ItemViewSettings() {
    }
    return ItemViewSettings;
}());
exports.ItemViewSettings = ItemViewSettings;
//# sourceMappingURL=datenmeister-navigation.js.map