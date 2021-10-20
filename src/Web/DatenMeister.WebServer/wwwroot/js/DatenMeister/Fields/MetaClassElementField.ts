import * as Mof from "../Mof";
import {injectNameByUri} from "../DomHelper";
import {BaseField, IFormField} from "../Interfaces.Fields";

export class Field extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    createDom(dmElement: Mof.DmObject) {

        const div = $("<div />");
        if (dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
            div.text(dmElement.metaClass.id);
        } else {
            div.text("unknown");
        }

        injectNameByUri(div, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
        return div;
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}
