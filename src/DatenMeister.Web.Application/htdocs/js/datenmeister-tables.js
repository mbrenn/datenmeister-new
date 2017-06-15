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
                // Apply style to headline cell
                field.applyStandardStyles(domColumn);
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
                    var fieldInstance = field.createInstance(item);
                    var domField = fieldInstance.getDom();
                    domCell.append(domField);
                    domRow.append(domCell);
                    // Apply style to headline cell
                    field.applyStandardStyles(domCell);
                }
                this.domTable.append(domRow);
            }
        };
        return ListTableComposer;
    }());
    exports.ListTableComposer = ListTableComposer;
    var DetailTableConfiguration = (function () {
        function DetailTableConfiguration() {
            this.fields = new Array();
        }
        return DetailTableConfiguration;
    }());
    exports.DetailTableConfiguration = DetailTableConfiguration;
    var DetailTableComposer = (function () {
        function DetailTableComposer(configuration, container) {
            this.configuration = configuration;
            this.container = container;
        }
        DetailTableComposer.prototype.composeTable = function (item) {
            if (item === undefined || item === null) {
                item = {};
            }
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
            // Creates the 
            domRow = $("<tr><th>Title</th><th>Value</th><th></th></tr>");
            this.domTable.append(domRow);
            var field;
            this.domForEditArray = new Array();
            // Now, the items
            for (var f in this.configuration.fields) {
                field = this.configuration.fields[f];
                // Creates the row
                domRow = $("<tr></tr>");
                // Title column
                var domColumn = $("<td class='table_column_name'></td>");
                domColumn.data("column", "name");
                domColumn.text(field.title);
                domRow.append(domColumn);
                // Value column
                domColumn = $("<td class='table_column_value'></td>");
                domColumn.data("column", "value");
                var fieldInstance = field.createInstance(this.item);
                var domForEdit = fieldInstance.getDom();
                domColumn.append(domForEdit);
                domRow.append(domColumn);
                // Third column
                domColumn = $("<td></td>");
                domRow.append(domColumn);
                // Stores the fieldinstance
                this.domForEditArray[field.name] = fieldInstance;
                this.domTable.append(domRow);
            }
            // Add new property
            /*if (this.configuration.supportNewProperties) {
                this.offerNewProperty(domTable);
            }*/
            // Adds the OK and Cancel button
            var domOkCancel = $("<tr><td colspan='3' class='text-right'></td></tr>");
            var domOkCancelColumn = $("td", domOkCancel);
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
        /**
         * Sets the focus on the first field of the table
         */
        DetailTableComposer.prototype.setFocusOnFirstRow = function () {
            if (this.configuration.fields.length > 0) {
                var firstProperty = this.configuration.fields[0].name;
                if (firstProperty != undefined && this.domForEditArray[firstProperty] != undefined) {
                    this.domForEditArray[firstProperty].focus();
                }
            }
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
            buttonField.horizontalAlignment = Alignments.Right;
            buttonField.width = -1;
            configuration.fields[configuration.fields.length] = buttonField;
            return buttonField;
        }
        Fields.addEditButton = addEditButton;
        function addDeleteButton(configuration, onClick) {
            var buttonField = new ButtonField("DELETE");
            buttonField.horizontalAlignment = Alignments.Right;
            buttonField.width = -1;
            buttonField.click(function (item, button) {
                if (button !== undefined && button.state === true) {
                    onClick(item);
                }
                else {
                    button.state = true;
                    button.setText("CONFIRM");
                }
            });
            configuration.fields[configuration.fields.length] = buttonField;
            return buttonField;
        }
        Fields.addDeleteButton = addDeleteButton;
        /**
         * Enumerates the alignments
         */
        var Alignments;
        (function (Alignments) {
            Alignments[Alignments["Left"] = 0] = "Left";
            Alignments[Alignments["Center"] = 1] = "Center";
            Alignments[Alignments["Right"] = 2] = "Right";
            Alignments[Alignments["Top"] = 3] = "Top";
            Alignments[Alignments["Middle"] = 4] = "Middle";
            Alignments[Alignments["Bottom"] = 5] = "Bottom";
        })(Alignments = Fields.Alignments || (Fields.Alignments = {}));
        var FieldBase = (function () {
            function FieldBase() {
            }
            FieldBase.prototype.getFieldType = function () { throw new Error("Not implemented"); };
            FieldBase.prototype.createInstance = function (item) { throw new Error("Not implemented"); };
            FieldBase.prototype.readOnly = function () {
                this.isReadOnly = true;
                return this;
            };
            FieldBase.prototype.applyStandardStyles = function (dom) {
                if (this.horizontalAlignment === Alignments.Right) {
                    dom.css("text-align", "right");
                }
                // Defines the width of the cell
                if (this.width === -1) {
                    dom.css("width", "1%");
                }
                else if (this.width !== undefined && this.width !== null) {
                    dom.css("width", this.width + "%");
                }
            };
            return FieldBase;
        }());
        Fields.FieldBase = FieldBase;
        var FieldInstanceBase = (function () {
            function FieldInstanceBase(dom) {
                this.dom = dom;
            }
            FieldInstanceBase.prototype.getDom = function () {
                return this.dom;
            };
            FieldInstanceBase.prototype.focus = function () { };
            return FieldInstanceBase;
        }());
        Fields.FieldInstanceBase = FieldInstanceBase;
        var ButtonField = (function (_super) {
            __extends(ButtonField, _super);
            function ButtonField(title, onClick) {
                var _this = _super.call(this) || this;
                _this.title = title;
                _this.onClick = onClick;
                return _this;
            }
            ButtonField.prototype.click = function (onClick) {
                this.onClick = onClick;
            };
            ButtonField.prototype.createInstance = function (item) {
                var _this = this;
                var domButton = $("<button href='#' class='btn btn-primary'></button>");
                var instance = new ButtonFieldInstance(domButton);
                domButton.click(function () {
                    _this.onClick(item, instance);
                    return false;
                });
                domButton.text(this.title);
                return instance;
            };
            ;
            return ButtonField;
        }(FieldBase));
        Fields.ButtonField = ButtonField;
        /**
         * Implements the instance of a field which can be used to trigger the instance.
         */
        var ButtonFieldInstance = (function (_super) {
            __extends(ButtonFieldInstance, _super);
            function ButtonFieldInstance(dom) {
                var _this = _super.call(this, dom) || this;
                _this.state = false;
                return _this;
            }
            ButtonFieldInstance.prototype.setText = function (text) {
                this.domContainer.text(text);
            };
            ButtonFieldInstance.prototype.setState = function (state) {
                this.state = state;
            };
            ButtonFieldInstance.prototype.getState = function () {
                return this.state;
            };
            ButtonFieldInstance.prototype.focus = function () {
            };
            return ButtonFieldInstance;
        }(FieldInstanceBase));
        Fields.ButtonFieldInstance = ButtonFieldInstance;
        var TextboxField = (function (_super) {
            __extends(TextboxField, _super);
            function TextboxField(name, title) {
                var _this = _super.call(this) || this;
                _this.title = title;
                _this.name = name;
                return _this;
            }
            TextboxField.prototype.getFieldType = function () { return DMVM.ColumnTypes.textbox; };
            TextboxField.prototype.createInstance = function (item) {
                var contentValue = item[this.name];
                var isReadonly = this.isReadOnly;
                // We have a textbox, so check if we have multiple line
                if (this.lineHeight !== undefined && this.lineHeight > 1) {
                    var domTextBoxMultiple = $("<textarea class='form-control'></textarea>")
                        .attr("rows", this.lineHeight);
                    domTextBoxMultiple.val(contentValue);
                    if (isReadonly) {
                        domTextBoxMultiple.attr("readonly", "readonly");
                    }
                    return new TextboxFieldInstance(domTextBoxMultiple, domTextBoxMultiple);
                }
                else {
                    // Single line
                    if (isReadonly) {
                        var domSpan = $("<span class='dm-itemtable-data'></span>");
                        domSpan.text(contentValue);
                        return new TextboxFieldInstance(domSpan, undefined);
                    }
                    else {
                        var domTextBox = $("<input type='textbox' class='form-control' />");
                        domTextBox.val(contentValue);
                        if (this.isReadOnly) {
                            domTextBox.attr("readonly", "readonly");
                        }
                        return new TextboxFieldInstance(domTextBox, domTextBox);
                    }
                }
            };
            return TextboxField;
        }(FieldBase));
        Fields.TextboxField = TextboxField;
        var TextboxFieldInstance = (function (_super) {
            __extends(TextboxFieldInstance, _super);
            function TextboxFieldInstance(dom, domFocus) {
                var _this = _super.call(this, dom) || this;
                _this.domFocus = domFocus;
                return _this;
            }
            TextboxFieldInstance.prototype.focus = function () {
                if (this.domFocus != undefined) {
                    this.domFocus.focus();
                }
            };
            return TextboxFieldInstance;
        }(FieldInstanceBase));
        Fields.TextboxFieldInstance = TextboxFieldInstance;
        var DropDownField = (function (_super) {
            __extends(DropDownField, _super);
            function DropDownField() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            DropDownField.prototype.getFieldType = function () {
                return DMVM.ColumnTypes.dropdown;
            };
            DropDownField.prototype.createInstance = function (item) {
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
                return new FieldInstanceBase(domDD);
            };
            return DropDownField;
        }(FieldBase));
        Fields.DropDownField = DropDownField;
    })(Fields = exports.Fields || (exports.Fields = {}));
    /**
     * Converts field data structure to real fields
     * @param fieldDatas The data to be converted as an array
     */
    function convertFieldDataToFields(fieldDatas) {
        var result = new Array();
        for (var n in fieldDatas) {
            result[n] = convertFieldDataToField(fieldDatas[n]);
        }
        return result;
    }
    exports.convertFieldDataToFields = convertFieldDataToFields;
    /**
     * Converts one instance of field data to a real field
     * @param data Data to be converted
     */
    function convertFieldDataToField(data) {
        var field;
        switch (data.fieldType) {
            case DMVM.ColumnTypes.textbox:
                field = new Fields.TextboxField();
                break;
            default:
                throw "Unknown fieldtype: " + data.fieldType;
        }
        field.title = data.title;
        field.isReadOnly = data.isReadOnly;
        field.isEnumeration = data.isEnumeration;
        field.defaultValue = data.defaultValue;
        field.name = data.name;
        return field;
    }
    exports.convertFieldDataToField = convertFieldDataToField;
});
//# sourceMappingURL=datenmeister-tables.js.map