define(["require", "exports", "../Mof", "../DomHelper", "./Forms", "../FormActions"], function (require, exports, Mof_1, DomHelper_1, Forms, FormActions_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createActionFormForEmptyObject = void 0;
    function createActionFormForEmptyObject(parent, metaClass, configuration, actionName) {
        const tthis = this;
        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
            };
        }
        const creator = new Forms.DetailFormCreator();
        configuration.isNewItem = true;
        configuration.onSubmit = (element) => {
            FormActions_1.DetailFormActions.execute(actionName, creator, undefined, creator.element);
        };
        let deferLoadObjectForAction = FormActions_1.DetailFormActions.loadObjectForAction(actionName);
        if (deferLoadObjectForAction === undefined) {
            deferLoadObjectForAction = new Promise(resolve => {
                resolve(new Mof_1.DmObject());
            });
        }
        let deferForm;
        deferLoadObjectForAction.then((element) => {
            var _a;
            if (metaClass === undefined && ((_a = element.metaClass) === null || _a === void 0 ? void 0 : _a.uri) !== undefined) {
                // If the returned element has a metaclass, then set the metaClass being used to 
                // find the right form to the one by the element
                metaClass = element.metaClass.uri;
            }
            else if (element.metaClass === undefined) {
                // Updates the metaclass, if the metaclass is not set by the element itself
                element.setMetaClassByUri(metaClass);
            }
            // Defines the form
            if (configuration.formUri !== undefined) {
                // Ok, we already have an explicit form
                deferForm = Forms.getForm(configuration.formUri);
            }
            else if (metaClass === undefined) {
                // If there is no metaclass set, create a total empty form object...
                deferForm = new Promise(resolve => {
                    deferForm.resolve(Forms.FormModel.createEmptyFormWithDetail());
                });
            }
            else {
                deferForm = Forms.getDefaultFormForMetaClass(metaClass);
            }
            deferForm.then((form) => {
                creator.element = element;
                creator.formElement = form;
                creator.createFormByObject(parent, configuration);
                (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
            });
        });
    }
    exports.createActionFormForEmptyObject = createActionFormForEmptyObject;
});
//# sourceMappingURL=ActionForm.js.map