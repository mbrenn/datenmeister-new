import * as Mof from "../Mof";
import {injectNameByUri} from "../DomHelper";
import {BaseField, IFormField} from "../Interfaces.Fields";

export class Field extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    createDom(dmElement: Mof.DmObject) {

        const div = $("<div />");
        if (dmElement !== undefined && dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
            if (dmElement.metaClass.uri !== null) {
                div.text(dmElement.metaClass.id ?? dmElement.metaClass.uri);
                injectNameByUri(div, encodeURIComponent(dmElement.metaClass.uri));
            } else if (dmElement.metaClass.id !== null && dmElement.metaClass.extentUri !== null) {
                div.text(dmElement.metaClass.id);
                injectNameByUri(div, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
            } else {
                div.append($("<em>unknown</em>"));
            }
        } else {
            div.append($("<em>unknown</em>"));
        }

        return div;
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}
