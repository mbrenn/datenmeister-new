import {BaseField, IFormField} from "../Interfaces.Fields";
import {DmObject} from "../Mof";

export class Field extends BaseField implements IFormField {
    _list: JQuery;

    createDom(dmElement: DmObject): JQuery<HTMLElement> {

        if (this.isReadOnly === true) {

            const fieldName = this.field.get('name');
            const value = dmElement.get(fieldName);

            this._list = $("<span></span>");
            this._list.text(value.get('name'));

            return this._list;
        } else {
            const fieldName = this.field.get('name');
            const value = dmElement.get(fieldName);

            this._list = $("<span></span>");
            this._list.text(value.get('name'));

            return this._list;
        }
    }

    evaluateDom(dmElement: DmObject) {

    }
}
