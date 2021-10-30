import * as Mof from "../Mof";
import {injectNameByUri} from "../DomHelper";
import {BaseField, IFormField} from "../Interfaces.Fields";

export class Field extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    createDom(dmElement: Mof.DmObject) {

        const div = $("<div />");
        if (dmElement !== undefined && dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
            div.text(dmElement.metaClass.id);
            injectNameByUri(div, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
        } else {
            div.append($("<em>unknown</em>"));
        }

        return div;
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}
