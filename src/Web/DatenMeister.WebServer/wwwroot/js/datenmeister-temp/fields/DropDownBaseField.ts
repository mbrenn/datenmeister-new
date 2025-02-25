import { BaseField, IFormField } from "./Interfaces.js";
import { DmObject, ObjectType } from "../Mof.js";
import * as ClientItems from "../client/Items.js";

/**
 * Defines one value of the drop down field as it will be used to evaluate
 * the selected value after the user has selected a specific option value
 */
export interface DropDownOptionField {
    title: string;
    value?: string;
    itemUrl?: string;
    workspace?: string;

    // Key under which the item will be stored as value in option field
    key?: string;
}

export enum FieldType {
    Strings,
    References
}

export abstract class DropDownBaseField extends BaseField implements IFormField {
    loadedFields: DropDownOptionField[] = [];
    fieldType: FieldType = FieldType.Strings;

    _dropDown: JQuery;

    /**
     * Stores the element to which the DOM needs to be created
     */
    _element: DmObject | undefined;

    private counter: number = 0;

    private _loadedFields: DropDownOptionField[] = [];


    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {
        // Enumrates the fields
        this._loadedFields = await this.loadFields();
        for (const field of this._loadedFields) {
            field.key = "item_" + this.counter.toString();
            this.counter++;
        }

        this._element = dmElement;

        const fieldName = this.field.get('name')?.toString() ?? "";

        // Creates the dropdown
        if (this.fieldType === FieldType.Strings) {
            let value = dmElement.get(fieldName) as string;

            if (this.isReadOnly) {
                const result = $("<span></span>");
                if (value === undefined) {
                    result.text("<em>Not set</em>");
                }
                else {
                    result.text(value);
                }

                return result;
            } else {
                if (Array.isArray(this._loadedFields)) {
                    this._dropDown = $("<select></select>");
                    for (const field of this._loadedFields) {
                        const option = $("<option></option>");
                        option.text(field.title);
                        option.val(field.value);
                        this._dropDown.append(option);
                    }

                    if (value !== undefined) {
                        this._dropDown.val(value);
                    }

                    return this._dropDown;
                }
                else {
                    return $("<span><em>No values given</em></span>");
                }
            }
        }
        else if (this.fieldType === FieldType.References) {
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

                for (const n in this._loadedFields) {
                    const item = this._loadedFields[n];
                    const option = $("<option></option>");
                    option.attr('value', item.key);

                    if (value !== undefined && value.uri === item.itemUrl && value.workspace === item.workspace) {
                        option.attr('selected', 'selected');
                        anySelected = true;
                    }

                    option.text(item.title);
                    this._dropDown.append(option);
                }

                if (!anySelected) {
                    notSelected.attr('selected', 'selected');
                }

                return this._dropDown;
            }
        }
        else {
            return $("<span><em>Unknown field type</em></span>");
        }
    }

    async evaluateDom(dmElement: DmObject): Promise<void> {
        if (this.fieldType === FieldType.Strings) {
            const fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, this._dropDown.val());
        }
        else if (this.fieldType === FieldType.References && this._dropDown !== undefined) {
            const fieldName = this.field.get('name').toString();
            const fieldValue = this._dropDown.val();
            if (fieldValue === '' || fieldValue === undefined) {
                dmElement.unset(fieldName);
            }
            else {
                const field = this._loadedFields.find(x => x.key === fieldValue);
                if (field !== undefined) {
                    dmElement.set(fieldName, DmObject.createFromReference(field.workspace ?? "", field.itemUrl ?? ""));
                }
            }
        }        
    }

    abstract loadFields(): Promise<DropDownOptionField[]>;
}