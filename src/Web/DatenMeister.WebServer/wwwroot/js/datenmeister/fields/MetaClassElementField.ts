import {injectNameByUri} from "../DomHelper.js";
import {BaseField, IFormField} from "./Interfaces.js";
import {SelectItemControl} from "../controls/SelectItemControl.js";
import {setMetaclass} from "../client/Items.js";
import * as Mof from "../Mof.js";

export class Field extends BaseField implements IFormField {

    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {
        const tthis = this;

        const divContainer = $("<div />");
        const div = $("<div />");
        if (dmElement !== undefined && dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
            if (dmElement.metaClass.uri !== null) {
                div.text(dmElement.metaClass.id ?? dmElement.metaClass.uri);
                injectNameByUri(div, dmElement.metaClass.workspace, encodeURIComponent(dmElement.metaClass.uri));
            } else if (dmElement.metaClass.id !== null && dmElement.metaClass.extentUri !== null) {
                div.text(dmElement.metaClass.id);
                injectNameByUri(div, dmElement.metaClass.workspace, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
            } else {
                div.append($("<em>unknown</em>"));
            }
        } else {
            div.append($("<em>unknown</em>"));
        }

        divContainer.append(div);

        // Create button to change metaClass
        if (!this.isReadOnly) {
            const changeMetaClassDiv = $("<div></div>");

            const button = $("<button class='btn btn-secondary' type='button'></button>");
            button.text("Change MetaClass");
            button.on('click', () => {

                changeMetaClassDiv.empty();
                const selectItemCtrl = new SelectItemControl();
                const divSelectItem = selectItemCtrl.init(changeMetaClassDiv);

                selectItemCtrl.setExtentByUri("Types", "dm:///_internal/types/internal");

                selectItemCtrl.itemSelected.addListener(
                    (selectedItem) => {
                        setMetaclass(tthis.form.workspace, tthis.itemUrl, selectedItem.uri)
                            .then(() => divSelectItem.remove()).then(() => {
                            if (tthis.configuration.refreshForm !== undefined) {
                                tthis.configuration.refreshForm();
                            }
                        });
                    });
            });

            divContainer.append(button);
            divContainer.append(changeMetaClassDiv);
        }
        return divContainer;
    }

    async evaluateDom(dmElement: Mof.DmObject) : Promise<void> {
    }
}