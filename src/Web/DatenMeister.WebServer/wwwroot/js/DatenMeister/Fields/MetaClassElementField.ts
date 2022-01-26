import * as Mof from "../Mof";
import {injectNameByUri} from "../DomHelper";
import {BaseField, IFormField} from "../Interfaces.Fields";
import {SelectItemControl} from "../Forms.SelectItemControl";
import {setMetaclass} from "../Client.Items";

export class Field extends BaseField implements IFormField
{
    _textBox: JQuery<HTMLInputElement>;

    createDom(dmElement: Mof.DmObject) {
        const tthis = this;

        const divContainer = $("<div />");
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

        divContainer.append(div);

        // Create button to change metaClass
        if (!this.isReadOnly) {
            var button = $("<button class='btn btn-secondary' type='button'></button>");
            button.text("Set MetaClass");
            button.on('click', () => {
                const selectItemCtrl = new SelectItemControl();
                const divSelectItem = selectItemCtrl.init(divContainer);

                selectItemCtrl.onItemSelected = (selectedItem) => {
                    setMetaclass(tthis.form.workspace, tthis.itemUrl, selectedItem.uri)
                        .done(() => divSelectItem.remove()).done(() => {
                            if (tthis.configuration.refreshForm !== undefined) {
                                tthis.configuration.refreshForm();
                            }
                        });
                }
            });

            divContainer.append(button);
        }

        return divContainer;
    }

    evaluateDom(dmElement: Mof.DmObject) {
    }
}
