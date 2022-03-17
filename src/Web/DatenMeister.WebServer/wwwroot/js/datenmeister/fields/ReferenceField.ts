import {BaseField, IFormField} from "../Interfaces.Fields";
import {DmObject} from "../Mof";

export class Field extends BaseField implements IFormField {
    _list: JQuery;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {

        const fieldName = this.field.get('name');
        let value = dmElement.get(fieldName);
        if (Array.isArray(value)) {
            if (value.length === 1) {
                value = value[0];
            } else {
                return $("<em>The value is an array and not supported by the referencefield</em>");
            }
        }
        
        if (this.isReadOnly === true) {
            this._list = $("<span></span>");
            if (value === undefined) {
                this._list.html("<em>undefined</em>");
            } else {
                this._list.text(value.get('name'));
            }

            return this._list;
        } else {

            this._list = $("<span></span>");
            this._list.text(value.get('name'));

            return this._list;
        }
    }

    evaluateDom(dmElement: DmObject) {

    }
}
