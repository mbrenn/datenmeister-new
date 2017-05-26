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
define(["require", "exports", "./datenmeister-viewmodels"], function (require, exports, DMVM) {
    "use strict";
    exports.__esModule = true;
    var ListTableConfiguration = (function () {
        function ListTableConfiguration() {
            this.fields = new Array();
        }
        return ListTableConfiguration;
    }());
    exports.ListTableConfiguration = ListTableConfiguration;
    /**
     * Composes the table as a list view
     */
    var ListTableComposer = (function () {
        function ListTableComposer(configuration, container) {
            this.configuration = configuration;
            this.container = container;
        }
        ListTableComposer.prototype.composeTable = function (items) {
            this.items = items;
            this.rows = new Array();
            this.domTable = $("<table class='table table-condensed'></table>");
            this.composeContent();
            this.container.append(this.domTable);
        };
        ListTableComposer.prototype.refresh = function () {
            this.domTable.empty();
            this.composeContent();
        };
        ListTableComposer.prototype.composeContent = function () {
            this.rows = new Array();
            // Creates the headrow
            var domHeadRow = $("<tr></tr>");
            var field;
            var n;
            for (n in this.configuration.fields) {
                field = this.configuration.fields[n];
                var domColumn = $("<th></th>");
                domColumn.text(field.title);
                domHeadRow.append(domColumn);
            }
            this.domTable.append(domHeadRow);
            // Create content
            for (n in this.items) {
                var item = this.items[n];
                if (this.filterItem !== undefined && this.filterItem !== null && this.filterItem(item) === false) {
                    // Item can be filtered out
                    continue;
                }
                var domRow = $("<tr></tr>");
                this.rows[n] = domRow;
                for (var f in this.configuration.fields) {
                    field = this.configuration.fields[f];
                    var domCell = $("<td></td>");
                    domCell.append(field.createDom(item));
                    domRow.append(domCell);
                }
                this.domTable.append(domRow);
            }
        };
        return ListTableComposer;
    }());
    exports.ListTableComposer = ListTableComposer;
    var Fields;
    (function (Fields) {
        function addEditButton(configuration, onClick) {
            var buttonField = new ButtonField("EDIT", onClick);
            configuration.fields[configuration.fields.length] = buttonField;
            return buttonField;
        }
        Fields.addEditButton = addEditButton;
    })(Fields = exports.Fields || (exports.Fields = {}));
    var FieldBase = (function () {
        function FieldBase() {
        }
        FieldBase.prototype.getFieldType = function () { throw new Error("Not implemented"); };
        FieldBase.prototype.createDom = function (item) { throw new Error("Not implemented"); };
        FieldBase.prototype.readOnly = function () {
            this.isReadOnly = true;
            return this;
        };
        return FieldBase;
    }());
    exports.FieldBase = FieldBase;
    var ButtonField = (function (_super) {
        __extends(ButtonField, _super);
        function ButtonField(title, onClick) {
            var _this = _super.call(this) || this;
            _this.title = title;
            _this.onClick = onClick;
            return _this;
        }
        ButtonField.prototype.createDom = function (item) {
            var _this = this;
            var button = $("<button href='#' class='btn btn-primary'></button>");
            button.click(function () {
                _this.onClick(item);
                return false;
            });
            button.text(this.title);
            return button;
        };
        ;
        return ButtonField;
    }(FieldBase));
    exports.ButtonField = ButtonField;
    var TextboxField = (function (_super) {
        __extends(TextboxField, _super);
        function TextboxField(name, title) {
            var _this = _super.call(this) || this;
            _this.title = title;
            _this.name = name;
            return _this;
        }
        TextboxField.prototype.getFieldType = function () { return DMVM.ColumnTypes.textbox; };
        TextboxField.prototype.createDom = function (item) {
            var contentValue = item[this.name];
            var isReadonly = this.isReadOnly;
            // We have a textbox, so check if we have multiple line
            if (this.lineHeight !== undefined && this.lineHeight > 1) {
                var domTextBoxMultiple = $("<textarea class='form-control'></textarea>")
                    .attr('rows', this.lineHeight);
                domTextBoxMultiple.val(contentValue);
                if (isReadonly) {
                    domTextBoxMultiple.attr("readonly", "readonly");
                }
                return domTextBoxMultiple;
            }
            else {
                // Single line
                if (isReadonly) {
                    var domResult = $("<span class='dm-itemtable-data'></span>");
                    domResult.text(contentValue);
                    return domResult;
                }
                else {
                    var domTextBox = $("<input type='textbox' class='form-control' />");
                    domTextBox.val(contentValue);
                    if (this.isReadOnly) {
                        domTextBox.attr("readonly", "readonly");
                    }
                    return domTextBox;
                }
            }
        };
        return TextboxField;
    }(FieldBase));
    exports.TextboxField = TextboxField;
    var DropDownField = (function (_super) {
        __extends(DropDownField, _super);
        function DropDownField() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        DropDownField.prototype.getFieldType = function () {
            return DMVM.ColumnTypes.dropdown;
        };
        DropDownField.prototype.createDom = function (item) {
            var contentValue = item[this.name];
            if (contentValue === undefined) {
                contentValue = this.defaultValue;
            }
            var domDD = $("<select></select>");
            for (var name in this.values) {
                var displayText = this.values[name];
                var domOption = $("<option></option>").attr("value", name).text(displayText);
                domDD.append(domOption);
            }
            domDD.val(contentValue);
            return domDD;
        };
        return DropDownField;
    }(FieldBase));
    exports.DropDownField = DropDownField;
});
//# sourceMappingURL=datenmeister-tables.js.map