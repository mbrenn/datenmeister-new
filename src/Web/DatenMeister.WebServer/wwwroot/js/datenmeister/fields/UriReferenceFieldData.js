import { ObjectType } from "../Mof.js";
import { BaseField } from "./Interfaces.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
var _UriReferenceFieldData = _DatenMeister._Forms._UriReferenceFieldData;
import * as SIC from "../controls/SelectItemControl.js";
export class Field extends BaseField {
    async createDom(dmElement) {
        const fieldName = this.field.get('name')?.toString() ?? "";
        /* Returns a list element in case an array is given */
        let value = dmElement.get(fieldName, ObjectType.String) ?? "";
        const originalValue = value;
        /* Otherwise just create the correct field type. */
        if (this.isReadOnly) {
            const divContainer = $("<div class='dm-urireference-container-readonly'></div>");
            const div = $("<div class='dm-textfield'/>");
            if (value === undefined) {
                div.append($("<em class='dm-undefined'>undefined</em>"));
            }
            else {
                div.text(value ?? "undefined");
            }
            divContainer.append(div);
            return divContainer;
        }
        else {
            const domContainer = $("<div class='dm-urireference-container'></div>");
            const textUri = "<div><strong>Uri:</strong></div>";
            domContainer.append(textUri);
            const defaultExtent = this.field.get(_UriReferenceFieldData.defaultExtent, ObjectType.String);
            const defaultWorkspace = this.field.get(_UriReferenceFieldData.defaultWorkspace, ObjectType.String);
            this._textBox = $("<input size='80' class='dm-textfield' />");
            this._textBox.val(value);
            domContainer.append(this._textBox);
            // Create the selection field to define a uri
            const builderText = $("<div><strong>Build your Uri:</strong></div>");
            domContainer.append(builderText);
            // Creates and initialize the selection field
            this._selectField = new SIC.SelectItemControl();
            if (defaultExtent !== undefined && defaultExtent !== "") {
                await this._selectField.setWorkspaceById(defaultWorkspace);
            }
            if (defaultExtent !== undefined && defaultExtent !== "") {
                await this._selectField.setWorkspaceById(defaultWorkspace);
            }
            const table = $("<table>" +
                "<tr><td>Element:</td><td><div class='dm-urireference-element' /></div></td></tr>" +
                "<tr><td>Property:</td><td><input type='text' class='dm-urireference-property' /></td></tr>" +
                "<tr><td>Full Name:</td><td><input type='text' class='dm-urireference-fullname' /></td></tr>" +
                "<tr><td>Composite:</td><td>" +
                "  <select class='dm-urireference-composites' >" +
                "    <option value=''>Only Self</option>" +
                "    <option value='includingSelf'>Composites and self</option>" +
                "    <option value='onlyComposites'>Composites</option>" +
                "    <option value='allReferenced'>Referenced</option>" +
                "    <option value='allReferencedIncludingSelf'>Referenced and self</option>" +
                "  </select>" +
                "</td></tr>" +
                "<tr><td> </td><td><button class='dm-urireference-buildUri' class='btn btn-secondary'>Build Uri</button></td></tr>" +
                "</table>");
            domContainer.append(table);
            // Append the html for the selection field
            this._elementSelection = $(".dm-urireference-element", table);
            this._selectField.init(this._elementSelection);
            this._propertyField = $(".dm-urireference-property", table);
            this._fullNameField = $(".dm-urireference-fullname", table);
            this._compositeSelection = $(".dm-urireference-composites", table);
            this._buildUriButton = $(".dm-urireference-buildUri", table);
            this._buildUriButton.on('click', async () => {
                await this.buildUrl();
            });
            return domContainer;
        }
    }
    async buildUrl() {
        const selectedItem = this._selectField.getSelectedItem();
        if (selectedItem === null || selectedItem === undefined) {
            this._textBox.val('No item is selected');
            return;
        }
        let url = this._selectField.getSelectedItem().uri;
        // Check, if we have an object id (which is located after the #). The object id must be put at the end of the url after
        // having added the parameters
        let objectId = "";
        const hashIndex = url.indexOf('#');
        if (hashIndex >= 0) {
            objectId = url.substring(hashIndex + 1);
            url = url.substring(0, hashIndex);
        }
        // Now add the properties
        let ampersand = '?';
        var propertyFieldValue = this._propertyField.val();
        if (propertyFieldValue !== undefined && propertyFieldValue !== null && propertyFieldValue !== "") {
            url += ampersand + "prop=" + encodeURIComponent(this._propertyField.val()?.toString() ?? "");
            ampersand = '&';
        }
        var fullNameFieldValue = this._fullNameField.val();
        if (fullNameFieldValue !== undefined && fullNameFieldValue !== null && fullNameFieldValue !== "") {
            url += ampersand + "fn=" + encodeURIComponent(this._fullNameField.val()?.toString() ?? "");
            ampersand = '&';
        }
        var compositeFieldValue = this._compositeSelection.val();
        if (compositeFieldValue !== undefined && compositeFieldValue !== null && compositeFieldValue !== "") {
            url += ampersand + "composites=" + encodeURIComponent(this._compositeSelection.val()?.toString() ?? "");
            ampersand = '&';
        }
        // Add the object id with #
        if (objectId !== "") {
            url += "#" + objectId;
        }
        this._textBox.val(url);
    }
    async fillFieldsFromUrl(url) {
        // First, we need to get the object id from the url
        let objectId = "";
        const hashIndex = url.indexOf('#');
        if (hashIndex >= 0) {
            objectId = url.substring(hashIndex + 1);
        }
        // Then we need to get the parameters prop, fn and composites
        let prop = "";
        let fn = "";
        let composites = "";
        let ampersandIndex = url.indexOf('?');
        if (ampersandIndex >= 0) {
            const parameters = new URLSearchParams(url.substring(ampersandIndex + 1));
            prop = parameters.get('prop') ?? "";
            fn = parameters.get('fn') ?? "";
            composites = parameters.get('composites') ?? "";
        }
        // Now, we need to get the extent before the # and the ? by calculating the first appearance of one of these characters
        let extent = url;
        const first = ampersandIndex > 0 && hashIndex > 0 ? Math.min(url.indexOf('#'), url.indexOf('?')) :
            ampersandIndex > 0 ? ampersandIndex : hashIndex;
        if (first >= 0) {
            extent = url.substring(0, hashIndex);
        }
        // Sets the object url, by combining extent and objectId, if later is available
        let objectUrl = extent;
        if (objectId !== "") {
            objectUrl += "#" + objectId;
        }
        // So, now put them into the Html elements
        this._fullNameField.val(fn);
        this._propertyField.val(prop);
        this._compositeSelection.val(composites);
        await this._selectField.setItemByUri("Data", objectUrl);
    }
    async evaluateDom(dmElement) {
        if (this._textBox !== undefined && this._textBox !== null) {
            const fieldName = this.field.get('name')?.toString() ?? "";
            dmElement.set(fieldName, this._textBox.val());
        }
    }
}
//# sourceMappingURL=UriReferenceFieldData.js.map