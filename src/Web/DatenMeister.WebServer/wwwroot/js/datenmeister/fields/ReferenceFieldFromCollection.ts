import {BaseField, IFormField} from "./Interfaces";
import {DmObject, ObjectType} from "../Mof";
import {ItemWithNameAndId} from "../ApiModels";
import * as ClientItems from "../client/Items"
import {_DatenMeister} from "../models/DatenMeister.class";

export class Field extends BaseField implements IFormField {

    _dropDown: JQuery;

    async createDom(dmElement: DmObject): Promise<JQuery<HTMLElement>> {

        this._dropDown = $("<select></select>");

        const values = await this.loadValuesFromServer();
        for (const n in values) {
            const item = values[n];
            const option = $("<option></option>");
            option.attr('value', item.uri);
            option.text(item.name);
            this._dropDown.append(option);
        }

        return this._dropDown;
    }

    evaluateDom(dmElement: DmObject) {
        const fieldName = this.field.get('name').toString();
        dmElement.set(fieldName, 
            DmObject.createFromReference(
                _DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace,
                this._dropDown.val().toString()));
    }

    async loadValuesFromServer(): Promise<ItemWithNameAndId[]> {
        return await ClientItems.getRootElements(
            _DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace, 
            _DatenMeister._Forms._ReferenceFieldFromCollectionData.collection);
    }
}