
import * as Settings from "../Settings"
import { DetailFormActions } from "../FormActions";
import { BaseField, IFormField } from "../Interfaces.Fields";
import { DmObject } from "../Mof";

import { getItemDetailUri } from "../Website";

export class Field extends BaseField implements IFormField {

    _list: JQuery;
    createDom(dmElement: DmObject): JQuery<HTMLElement> {
        this._list = $("<ul class='list-unstyled'></ul>");

        const fieldName = this.field.get('name');
        const value = dmElement.get(fieldName);

        for (var m in value) {
            if (Object.prototype.hasOwnProperty.call(value, m)) {
                let innerValue = value[m] as DmObject;

                let item = $("<li><a></a></li>");
                const link = $("a", item);
                link.text(innerValue.get('name'));
                link.attr('href', getItemDetailUri(innerValue));
                this._list.append(item);
            }
        }

        return this._list;
    }

    evaluateDom(dmElement: DmObject) {
        
    }
}