define(["require", "exports", "./Mof", "./DomHelper", "./Forms", "./FormActions"], function (require, exports, Mof, DomHelper_1, Forms, FormActions_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createActionFormForEmptyObject = void 0;
    function createActionFormForEmptyObject(parent, metaClass, configuration, actionName) {
        const creator = new Forms.DetailFormCreator();
        creator.element = new Mof.DmObject();
        configuration.onSubmit = (element) => {
            FormActions_1.DetailFormActions.execute(actionName, creator, undefined, creator.element);
        };
        if (metaClass === undefined) {
            // Create a total empty form object... 
            creator.formElement = Forms.FormModel.createEmptyFormWithDetail();
            creator.createFormByObject(parent, configuration);
        }
        else {
            const defer = Forms.getDefaultFormForMetaClass(metaClass);
            $.when(defer).then(function (form) {
                creator.formElement = form;
                creator.createFormByObject(parent, configuration);
                (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
            });
        }
    }
    exports.createActionFormForEmptyObject = createActionFormForEmptyObject;
});
//# sourceMappingURL=Forms.ActionForm.js.map