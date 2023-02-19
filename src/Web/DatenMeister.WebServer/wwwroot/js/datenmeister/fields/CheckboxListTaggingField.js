var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Interfaces", "../Mof", "../models/DatenMeister.class"], function (require, exports, Interfaces_1, Mof_1, DatenMeister_class_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        constructor() {
            super();
            /**
             * Stores the array of created checkboxes which are used to return back the
             * value of the selected checkboxes
             * @private
             */
            this.checkboxes = new Array();
        }
        createDom(dmElement) {
            return __awaiter(this, void 0, void 0, function* () {
                // Ensure local availability of field information
                this.name = this.field.get(DatenMeister_class_1._DatenMeister._Forms._CheckboxListTaggingFieldData._name_, Mof_1.ObjectType.String);
                this.separator = this.field.get(DatenMeister_class_1._DatenMeister._Forms._CheckboxListTaggingFieldData.separator, Mof_1.ObjectType.String);
                if (this.separator === "" || this.separator === undefined) {
                    this.separator = ",";
                }
                const containsFreeText = this.field.get(DatenMeister_class_1._DatenMeister._Forms._CheckboxListTaggingFieldData.containsFreeText, Mof_1.ObjectType.Boolean);
                const valuePairs = this.field.get(DatenMeister_class_1._DatenMeister._Forms._CheckboxListTaggingFieldData.values, Mof_1.ObjectType.Array);
                this.isFieldReadOnly = this.field.get(DatenMeister_class_1._DatenMeister._Forms._CheckboxListTaggingFieldData.isReadOnly, Mof_1.ObjectType.Boolean);
                // Gets the value and splits it
                const currentValue = dmElement.get(this.name, Mof_1.ObjectType.String);
                const currentList = currentValue.split(this.separator);
                // Create the element       
                const result = $("<table></table>");
                // Now go through the values and create the option fields
                for (let n in valuePairs) {
                    if (!Object.prototype.hasOwnProperty.call(valuePairs, n))
                        continue;
                    const valuePair = valuePairs[n];
                    const valueName = valuePair.get(DatenMeister_class_1._DatenMeister._Forms._ValuePair._name_, Mof_1.ObjectType.String);
                    const valueContent = valuePair.get(DatenMeister_class_1._DatenMeister._Forms._ValuePair.value, Mof_1.ObjectType.String);
                    // Creates the checkbox
                    const checkbox = $("<input type='checkbox' />");
                    checkbox.attr('name', valueName);
                    checkbox.attr('data-value', valueContent);
                    if (this.isFieldReadOnly || this.isReadOnly) {
                        checkbox.attr('disabled', 'disabled');
                    }
                    const label = $("<label></label>");
                    label.text(valueName);
                    const index = currentList.indexOf(valueContent);
                    if (index !== -1) {
                        checkbox.attr('checked', 'checked');
                        currentList.splice(index, 1);
                    }
                    this.checkboxes.push(checkbox);
                    // Creates the row
                    const row = $("<tr><td></td></tr>");
                    $("td", row).append(checkbox);
                    $("td", row).append(label);
                    result.append(row);
                }
                // If user also wants to have a freetext, add it
                if (containsFreeText) {
                    this.freeText = $("<input type='text' />");
                    if (this.isFieldReadOnly || this.isReadOnly) {
                        this.freeText.attr('disabled', 'disabled');
                    }
                    // Combines the residual attributes
                    let freeTextText = "";
                    let komma = "";
                    for (let n in currentList) {
                        const listItem = currentList[n];
                        freeTextText += komma;
                        freeTextText += listItem;
                        komma = ",";
                    }
                    this.freeText.val(freeTextText);
                    // Creates the row
                    const rowOptions = $("<tr><td class='small'>Other:</td></tr>");
                    $("td", rowOptions).append(this.freeText);
                    result.append(rowOptions);
                    const row = $("<tr><td></td></tr>");
                    $("td", row).append(this.freeText);
                    result.append(row);
                }
                return result;
            });
        }
        /**
         * Evaluates the user input and checks whether the data can be correctly set
         * @param dmElement Element to which the data shall be added
         */
        evaluateDom(dmElement) {
            return __awaiter(this, void 0, void 0, function* () {
                if (this.isReadOnly || this.isFieldReadOnly)
                    return;
                let result = "";
                let komma = "";
                for (let n in this.checkboxes) {
                    const checkbox = this.checkboxes[n];
                    if (checkbox.prop('checked')) {
                        result += komma;
                        result += checkbox.attr('data-value');
                        komma = this.separator;
                    }
                }
                if (this.freeText !== undefined) {
                    result += komma;
                    result += this.freeText.val();
                }
                dmElement.set(this.name, result);
            });
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=CheckboxListTaggingField.js.map