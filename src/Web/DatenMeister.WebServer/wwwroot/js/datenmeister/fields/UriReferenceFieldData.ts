import * as Mof from "../Mof.js";
import {DmObject, ObjectType} from "../Mof.js";
import {BaseField, IFormField} from "./Interfaces.js";
import {_DatenMeister} from "../models/DatenMeister.class.js";
import _UriReferenceFieldData = _DatenMeister._Forms._UriReferenceFieldData;
import * as SIC from "../controls/SelectItemControl.js";

export class Field extends BaseField implements IFormField
{   
    _textBox: JQuery<HTMLElement>;
    private _elementSelection: JQuery<HTMLElement>;
    private _fullNameField: JQuery<HTMLElement>;
    private _compositeSelection: JQuery<HTMLElement>;
    private _propertyField: JQuery<HTMLElement>;
    private _buildUriButton: JQuery<HTMLElement>;
    private _selectField: SIC.SelectItemControl;

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
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

            } else {
                div.text(value ?? "undefined");
            }

            divContainer.append(div);
            return divContainer;
        } else {
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


            const table = $(
                "<table>" +
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
            this._elementSelection = $(".dm-urireference-element", table)
            this._selectField.init(this._elementSelection);

            this._propertyField = $(".dm-urireference-property", table)
            this._fullNameField = $(".dm-urireference-fullname", table)
            this._compositeSelection = $(".dm-urireference-composites", table)
            this._buildUriButton = $(".dm-urireference-buildUri", table)
            
            this._buildUriButton.on('click', async () => {
                await this.buildUrl();
            })
            
            return domContainer;
        }
    }
    
    async buildUrl()
    {
        // const url =this._selectField.
    }
    
    async fillFieldsFromUrl()
    {
        
    }

    async evaluateDom(dmElement: DmObject) : Promise<void> {
        if (this._textBox !== undefined && this._textBox !== null)
        {
            let fieldName: string;            
            dmElement.set(fieldName, this._textBox.val());
        }
    }
}