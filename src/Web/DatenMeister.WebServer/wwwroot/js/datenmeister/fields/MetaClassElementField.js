define(["require", "exports", "../DomHelper", "./Interfaces", "../controls/SelectItemControl", "../client/Items"], function (require, exports, DomHelper_1, Interfaces_1, SelectItemControl_1, Items_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            var _a;
            const tthis = this;
            const divContainer = $("<div />");
            const div = $("<div />");
            if (dmElement !== undefined && dmElement.metaClass !== undefined && dmElement.metaClass !== null) {
                if (dmElement.metaClass.uri !== null) {
                    div.text((_a = dmElement.metaClass.id) !== null && _a !== void 0 ? _a : dmElement.metaClass.uri);
                    (0, DomHelper_1.injectNameByUri)(div, dmElement.metaClass.workspace, encodeURIComponent(dmElement.metaClass.uri));
                }
                else if (dmElement.metaClass.id !== null && dmElement.metaClass.extentUri !== null) {
                    div.text(dmElement.metaClass.id);
                    (0, DomHelper_1.injectNameByUri)(div, dmElement.metaClass.workspace, encodeURIComponent(dmElement.metaClass.extentUri + "#" + dmElement.metaClass.id));
                }
                else {
                    div.append($("<em>unknown</em>"));
                }
            }
            else {
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
                    const selectItemCtrl = new SelectItemControl_1.SelectItemControl();
                    const divSelectItem = selectItemCtrl.init(changeMetaClassDiv);
                    selectItemCtrl.setWorkspaceById('Types');
                    selectItemCtrl.setExtentByUri("dm:///_internal/types/internal");
                    selectItemCtrl.itemSelected.addListener((selectedItem) => {
                        (0, Items_1.setMetaclass)(tthis.form.workspace, tthis.itemUrl, selectedItem.uri)
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
        evaluateDom(dmElement) {
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=MetaClassElementField.js.map