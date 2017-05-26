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
    var ListDetailConfiguration = (function () {
        function ListDetailConfiguration() {
            this.fields = new Array();
        }
        return ListDetailConfiguration;
    }());
    exports.ListDetailConfiguration = ListDetailConfiguration;
    var DetailTableComposer = (function () {
        function DetailTableComposer(configuration, container) {
            this.configuration = configuration;
            this.container = container;
        }
        DetailTableComposer.prototype.composeTable = function (item) {
            this.item = item;
            this.domTable = $("<table class='table table-condensed'></table>");
            this.composeContent();
            this.container.append(this.domTable);
        };
        DetailTableComposer.prototype.refresh = function () {
            this.domTable.empty();
            this.composeContent();
        };
        DetailTableComposer.prototype.composeContent = function () {
            var tthis = this;
            var domRow;
            domRow = $("<tr><th>Title</th><th>Value</th><th></th></tr>");
            this.domTable.append(domRow);
            var field;
            this.domForEditArray = new Array();
            // Now, the items
            for (var f in this.configuration.fields) {
                field = this.configuration.fields[f];
                domRow = $("<tr></tr>");
                var domColumn = $("<td class='table_column_name'></td>");
                domColumn.data("column", "name");
                domColumn.text(field.title);
                domRow.append(domColumn);
                domColumn = $("<td class='table_column_value'></td>");
                domColumn.data("column", "value");
                var domForEdit = field.createDom(this.item);
                domColumn.append(domForEdit);
                domRow.append(domColumn);
                domColumn = $("<td></td>");
                domRow.append(domColumn);
                this.domForEditArray[field.name] = domForEdit;
                this.domTable.append(domRow);
            }
            // Add new property
            /*if (this.configuration.supportNewProperties) {
                this.offerNewProperty(domTable);
            }*/
            // Adds the OK, Cancel button
            var domOkCancel = $("<tr><td colspan='3' class='text-right'></td></tr>");
            var domOkCancelColumn = $("td", domOkCancel);
            /*        var domOk = $("<button class='btn btn-primary dm-button-ok'>OK</button>");
                    domOkCancelColumn.append(domOk);*/
            if (this.onClickOk !== undefined || this.onClickOk !== undefined) {
                var domEdit = $("<button class='btn btn-primary dm-button-ok'>OK</button>");
                domEdit.click(function () {
                    tthis.clickOnOk();
                });
                domOkCancelColumn.append(domEdit);
            }
            if (this.onClickCancel !== undefined || this.onClickCancel !== undefined) {
                var domCancel = $("<button class='btn btn-default dm-button-cancel'>Cancel</button>");
                domCancel.click(function () { return tthis.clickOnCancel(); });
                domOkCancelColumn.append(domCancel);
            }
            this.domTable.append(domOkCancel);
        };
        DetailTableComposer.prototype.clickOnOk = function () {
            if (this.onClickOk !== undefined && this.onClickOk !== null) {
                var newItem = $.extend({}, this.item);
                this.submitForm(newItem);
                this.onClickOk(newItem);
            }
        };
        DetailTableComposer.prototype.clickOnCancel = function () {
            if (this.onClickCancel !== undefined && this.onClickCancel !== null) {
                this.onClickCancel();
            }
        };
        DetailTableComposer.prototype.submitForm = function (newItem) {
            var fields = this.configuration.fields;
            for (var f in fields) {
                if (fields.hasOwnProperty(f)) {
                    var field = fields[f];
                    var domEdit = this.domForEditArray[field.name];
                    var value = domEdit.val();
                    newItem[field.name] = value;
                }
            }
        };
        return DetailTableComposer;
    }());
    exports.DetailTableComposer = DetailTableComposer;
    var Fields;
    (function (Fields) {
        function addEditButton(configuration, onClick) {
            var buttonField = new ButtonField("EDIT", onClick);
            configuration.fields[configuration.fields.length] = buttonField;
            return buttonField;
        }
        Fields.addEditButton = addEditButton;
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
        Fields.FieldBase = FieldBase;
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
        Fields.ButtonField = ButtonField;
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
        Fields.TextboxField = TextboxField;
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
        Fields.DropDownField = DropDownField;
    })(Fields = exports.Fields || (exports.Fields = {}));
});
//# sourceMappingURL=datenmeister-tables.js.map