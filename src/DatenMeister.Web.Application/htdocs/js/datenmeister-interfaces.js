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
    var Navigation;
    (function (Navigation) {
        var FormForItemConfiguration = (function () {
            function FormForItemConfiguration() {
                this.columns = new Array();
            }
            FormForItemConfiguration.prototype.addColumn = function (column) {
                this.columns[this.columns.length] = column;
            };
            return FormForItemConfiguration;
        }());
        Navigation.FormForItemConfiguration = FormForItemConfiguration;
        var DialogConfiguration = (function (_super) {
            __extends(DialogConfiguration, _super);
            function DialogConfiguration() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return DialogConfiguration;
        }(FormForItemConfiguration));
        Navigation.DialogConfiguration = DialogConfiguration;
        var ItemViewSettings = (function () {
            function ItemViewSettings() {
            }
            return ItemViewSettings;
        }());
        Navigation.ItemViewSettings = ItemViewSettings;
    })(Navigation = exports.Navigation || (exports.Navigation = {}));
    var Api;
    (function (Api) {
        var PageType;
        (function (PageType) {
            PageType[PageType["Workspaces"] = 0] = "Workspaces";
            PageType[PageType["Extents"] = 1] = "Extents";
            PageType[PageType["Items"] = 2] = "Items";
            PageType[PageType["ItemDetail"] = 3] = "ItemDetail";
            PageType[PageType["Dialog"] = 4] = "Dialog";
        })(PageType = Api.PageType || (Api.PageType = {}));
        var ViewState = (function () {
            function ViewState() {
            }
            return ViewState;
        }());
        Api.ViewState = ViewState;
    })(Api = exports.Api || (exports.Api = {}));
    var Plugin;
    (function (Plugin) {
        var PluginParameter = (function () {
            function PluginParameter() {
            }
            return PluginParameter;
        }());
        Plugin.PluginParameter = PluginParameter;
    })(Plugin = exports.Plugin || (exports.Plugin = {}));
});
//# sourceMappingURL=datenmeister-interfaces.js.map