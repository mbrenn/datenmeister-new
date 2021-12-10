define(["require", "exports", "./Mof", "./DomHelper", "./Forms", "./FormActions"], function (require, exports, Mof, DomHelper_1, Forms, FormActions_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createActionFormForEmptyObject = void 0;
    function createActionFormForEmptyObject(parent, metaClass, configuration, actionName) {
        const tthis = this;
        if (configuration.refreshForm === undefined) {
            configuration.refreshForm = () => {
                tthis.createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
            };
        }
        const creator = new Forms.DetailFormCreator();
        creator.element = new Mof.DmObject();
        creator.element.setMetaClass(metaClass);
        configuration.onSubmit = (element) => {
            FormActions_1.DetailFormActions.execute(actionName, creator, undefined, creator.element);
        };
        let deferLoadObjectForAction = FormActions_1.DetailFormActions.loadObjectForAction(actionName);
        if (deferLoadObjectForAction === undefined) {
            deferLoadObjectForAction = $.Deferred();
            deferLoadObjectForAction.resolve(undefined);
        }
        let deferForm;
        if (configuration.formUri !== undefined) {
            deferForm = Forms.getForm(configuration.formUri);
        } else if (metaClass === undefined) {
            deferForm = $.Deferred();
            deferForm.resolve(Forms.FormModel.createEmptyFormWithDetail());
            // Create a total empty form object...
        } else {
            deferForm = Forms.getDefaultFormForMetaClass(metaClass);
        }
        $.when(deferForm, deferLoadObjectForAction).then((form, element) => {
            creator.element = element;
            creator.formElement = form;
            creator.createFormByObject(parent, configuration);
            (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
        });
    }
    exports.createActionFormForEmptyObject = createActionFormForEmptyObject;
});
//# sourceMappingURL=Forms.ActionForm.js.map