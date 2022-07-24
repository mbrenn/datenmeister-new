import {BaseField, IFormField} from "./Interfaces";
import {DmObject, ObjectType} from "../Mof";
import {_DatenMeister} from "../models/DatenMeister.class";

export class Field extends BaseField implements IFormField {

    /**
     * Stores the array of created checkboxes which are used to return back the
     * value of the selected checkboxes
     * @private
     */
    private _checkboxes = new Array<JQuery>();

    private _freeText: JQuery;

    private _separator: string;

    private _name: string;

    private _isReadOnly: boolean;

    constructor() {
        super();
    }

    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        // Ensure local availability of field information
        this._name = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData._name_, ObjectType.String);
        this._separator = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData.separator, ObjectType.String);
        if (this._separator === "" || this._separator === undefined) {
            this._separator = ",";
        }

        const containsFreeText = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData.containsFreeText, ObjectType.Boolean);

        const valuePairs = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData.values, ObjectType.Array);
        this._isReadOnly = this.field.get(_DatenMeister._Forms._CheckboxListTaggingFieldData.isReadOnly, ObjectType.Boolean);

        // Gets the value and splits it
        const currentValue = dmElement.get(this._name, ObjectType.String);
        const currentList = currentValue.split(this._separator);

        // Create the element       
        const result = $("<table></table>");

        // Now go through the values and create the option fields
        for (let n in valuePairs) {
            const valuePair = valuePairs[n] as DmObject;
            const valueName = valuePair.get(_DatenMeister._Forms._ValuePair.name, ObjectType.String);
            const valueContent = valuePair.get(_DatenMeister._Forms._ValuePair.value, ObjectType.String);

            // Creates the checkbox
            const checkbox = $("<input type='checkbox' />");
            checkbox.attr('name', valueName);
            checkbox.attr('data-value', valueContent);
            checkbox.attr('disabled', this._isReadOnly ? 'disabled' : '');

            const index = currentList.indexOf(valueContent);
            if (index !== -1) {
                checkbox.attr('checked', 'checked');

                currentList.splice(index, 1);
            }

            this._checkboxes.push(checkbox);

            // Creates the row
            const row = $("<tr><td></td></tr>");
            $("td", row).append(checkbox)
            result.append(row);
        }

        // If user also wants to have a freetext, add it
        if (containsFreeText) {
            this._freeText = $("<input type='text' />");

            // Combines the residual attributes
            let freeTextText = "";
            let komma = "";
            for (let n in currentList) {
                const listItem = currentList[n];
                freeTextText += komma;
                freeTextText += listItem;
                komma = ",";
            }

            this._freeText.text(freeTextText);


            // Creates the row
            const rowOptions = $("<tr><td class='small'>Other:</td></tr>");
            $("td", rowOptions).append(this._freeText)
            result.append(rowOptions);
            const row = $("<tr><td></td></tr>");
            $("td", row).append(this._freeText)
            result.append(row);
        }

        return result;
    }

    /**
     * Evaluates the user input and checks whether the data can be correctly set
     * @param dmElement Element to which the data shall be added
     */
    evaluateDom(dmElement: DmObject) {
        if (this._isReadOnly) return;

        let result = "";
        let komma = "";

        for (let n in this._checkboxes) {
            const checkbox = this._checkboxes[n];
            if (checkbox.prop('checked')) {
                result += komma;
                result += checkbox.attr('data-value');
                komma = this._separator;
            }
        }

        if (this._freeText !== undefined) {
            result += komma;
            result += this._freeText.text();
        }

        dmElement.set(this._name, result);
    }
}