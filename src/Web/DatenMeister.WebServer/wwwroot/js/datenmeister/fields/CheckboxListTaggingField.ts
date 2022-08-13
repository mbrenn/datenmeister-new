import {BaseField, IFormField} from "./Interfaces";
import {DmObject, ObjectType} from "../Mof";
import {_DatenMeister} from "../models/DatenMeister.class";

export class Field extends BaseField implements IFormField {

    /**
     * Stores the array of created checkboxes which are used to return back the
     * value of the selected checkboxes
     * @private
     */
    private checkboxes = new Array<JQuery>();

    private freeText: JQuery;

    private separator: string;

    private name: string;

    private isFieldReadOnly: boolean;

    constructor() {
        super();
    }

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
        // Ensure local availability of field information
        this.name = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData._name_, ObjectType.String);
        this.separator = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData.separator, ObjectType.String);

        if (this.separator === "" || this.separator === undefined) {
            this.separator = ",";
        }

        const containsFreeText = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData.containsFreeText, ObjectType.Boolean);

        const valuePairs = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData.values, ObjectType.Array);
        this.isFieldReadOnly = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData.isReadOnly, ObjectType.Boolean);

        // Gets the value and splits it
        const currentValue = dmElement.get(this.name, ObjectType.String);
        const currentList = currentValue.split(this.separator);

        // Create the element       
        const result = $("<table></table>");

        // Now go through the values and create the option fields
        for (let n in valuePairs) {
            if (!Object.prototype.hasOwnProperty.call(valuePairs, n))
                continue;

            const valuePair = valuePairs[n] as DmObject;
            const valueName = valuePair.get(_DatenMeister._Forms._ValuePair._name_, ObjectType.String);
            const valueContent = valuePair.get(_DatenMeister._Forms._ValuePair.value, ObjectType.String);

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
            $("td", rowOptions).append(this.freeText)
            result.append(rowOptions);
            const row = $("<tr><td></td></tr>");
            $("td", row).append(this.freeText)
            result.append(row);
        }

        return result;
    }

    /**
     * Evaluates the user input and checks whether the data can be correctly set
     * @param dmElement Element to which the data shall be added
     */
    evaluateDom(dmElement: DmObject) {
        if (this.isReadOnly || this.isFieldReadOnly) return;

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
    }
}