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
    exports.DataField = DataField;
    var TextDataField = (function (_super) {
        __extends(TextDataField, _super);
        function TextDataField(title, name) {
            var _this = _super.call(this, ColumnTypes.textbox, title, name) || this;
            _this.lineHeight = 1;
            return _this;
        }
        return TextDataField;
    }(DataField));
    exports.TextDataField = TextDataField;
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
    exports.DateTimeDataField = DateTimeDataField;
    var DropDownDataField = (function (_super) {
        __extends(DropDownDataField, _super);
        function DropDownDataField(title, name) {
            return _super.call(this, ColumnTypes.dropdown, title, name) || this;
        }
        return DropDownDataField;
    }(DataField));
    exports.DropDownDataField = DropDownDataField;
    var SubElementsDataField = (function (_super) {
        __extends(SubElementsDataField, _super);
        function SubElementsDataField(title, name) {
            return _super.call(this, ColumnTypes.subElements, title, name) || this;
        }
        return SubElementsDataField;
    }(DataField));
    exports.SubElementsDataField = SubElementsDataField;
    var ColumnTypes = (function () {
        function ColumnTypes() {
        }
        return ColumnTypes;
    }());
    ColumnTypes.textbox = "text";
    ColumnTypes.dropdown = "dropdown";
    ColumnTypes.dateTime = "datetime";
    ColumnTypes.subElements = "subelements";
    exports.ColumnTypes = ColumnTypes;
});
//# sourceMappingURL=datenmeister-viewmodels.js.map