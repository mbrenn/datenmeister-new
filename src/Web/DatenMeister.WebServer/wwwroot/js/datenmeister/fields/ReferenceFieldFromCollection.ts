import {BaseField, IFormField} from "./Interfaces";
import {DmObject, ObjectType} from "../Mof";
import {ItemWithNameAndId} from "../ApiModels";
import * as ClientItems from "../client/Items"
import {_DatenMeister} from "../models/DatenMeister.class";
import * as Mof from "../Mof";

export class Field extends BaseField implements IFormField {

    _dropDown: JQuery;

    /**
     * Stores the element to which the DOM needs to be created
     */
    _element: DmObject | undefined;

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
        this._element = dmElement;
        const fieldName = this.field.get('name')?.toString() ?? "";

        
        if (this.isReadOnly) {
            let value = dmElement.get(fieldName, ObjectType.Object) as DmObject;
            
            // Checks, if there is a value set at all? 
            if (value === undefined) {                
                return $("<em>Not set</em>");
            }
            
            // If yes, get the value
            const textValue = await ClientItems.getItemWithNameAndId(value.workspace, value.uri);
            const result = $("<span></span>");
            result.text(textValue.name);
            return result;
        } else {

            let value = dmElement.get(fieldName, ObjectType.Object) as DmObject;

            this._dropDown = $("<select></select>");
            let anySelected = false;
            const notSelected = $("<option value=''>--- No selection ---</option>")
            this._dropDown.append(notSelected);

            const values = await this.loadValuesFromServer();
            for (const n in values) {
                const item = values[n];
                const option = $("<option></option>");
                option.attr('value', item.uri);

                if (value !== undefined && value.uri === item.uri) {
                    option.attr('selected', 'selected');
                    anySelected = true;
                }

                option.text(item.name);
                this._dropDown.append(option);
            }

            if (!anySelected) {
                notSelected.attr('selected', 'selected');
            }

            return this._dropDown;
        }
    }

    async evaluateDom(dmElement: DmObject) : Promise<void> {
        const fieldName = this.field.get('name').toString();
        const fieldValue = this._dropDown.val();
        if ( fieldValue === '' || fieldValue === undefined) {
            dmElement.unset(fieldName);
        }
        else {
            dmElement.set(fieldName,
                DmObject.createFromReference(
                    this.field.get(_DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace),
                    fieldValue.toString()));
        }
    }

    async loadValuesFromServer(): Promise<ItemWithNameAndId[]> {
        if (this._element === undefined) {
            throw "The element is not set. 'createDom' must be called in advance";
        }

        return await ClientItems.getRootElementsAsItem(
            this.field.get(
                _DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace, ObjectType.String) ?? "",
            this.field.get(
                _DatenMeister._Forms._ReferenceFieldFromCollectionData.collection, ObjectType.String));
    }
}