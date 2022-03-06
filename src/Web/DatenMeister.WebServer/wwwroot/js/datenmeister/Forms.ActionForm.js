define(["require", "exports", "./Mof", "./DomHelper", "./Forms", "./FormActions"], function (require, exports, Mof_1, DomHelper_1, Forms, FormActions_1) {
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
            deferLoadObjectForAction = $.Deferred();
            deferLoadObjectForAction.resolve(new Mof_1.DmObject());
        }
        let deferForm;
        if (configuration.formUri !== undefined) {
            deferForm = Forms.getForm(configuration.formUri);
        }
        else if (metaClass === undefined) {
            deferForm = $.Deferred();
            deferForm.resolve(Forms.FormModel.createEmptyFormWithDetail());
            // Create a total empty form object...
        }
        else {
            deferForm = Forms.getDefaultFormForMetaClass(metaClass);
        }
        $.when(deferForm, deferLoadObjectForAction).then((form, element) => {
            // Updates the metaclass, if the metaclass is not set by the element itself
            if (element.metaClass === undefined) {
                element.setMetaClassByUri(metaClass);
            }
            creator.element = element;
            creator.formElement = form;
            creator.createFormByObject(parent, configuration);
            (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
        });
    }
    exports.createActionFormForEmptyObject = createActionFormForEmptyObject;
});
//# sourceMappingURL=Forms.ActionForm.js.map