var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "../DomHelper", "./Forms", "../FormActions", "../client/Forms", "../client/Elements", "../client/Items"], function (require, exports, Mof_1, DomHelper_1, Forms, FormActions_1, ClientForms, ClientElements, ClientItems) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createActionFormForEmptyObject = void 0;
    function createActionFormForEmptyObject(parent, metaClass, configuration, actionName) {
        var _a;
        return __awaiter(this, void 0, void 0, function* () {
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
                };
            }
            const creator = new Forms.DetailFormCreator();
            configuration.onSubmit = (element, method) => __awaiter(this, void 0, void 0, function* () {
                const loadedElement = yield ClientItems.getObjectByUri("Data", creator.element.uri);
                yield FormActions_1.DetailFormActions.execute(actionName, creator, undefined, loadedElement, undefined, // The action form cannot provide additional parameters as the ActionButton
                method);
            });
            /* Loads the object being used as a base for the new action.
            * Usually context information from GET-Query are retrieved. Or some default fields are filled out
            */
            let element = yield FormActions_1.DetailFormActions.loadObjectForAction(actionName);
            if (element === undefined) {
                element = new Mof_1.DmObject();
            }
            // If, we have created the element, we will now have to create the temporary object on the server
            const temporaryElement = yield ClientElements.createTemporaryElement();
            yield ClientItems.setProperties("Data", temporaryElement.uri, element);
            /* Now find the right form */
            let form;
            // After having loaded the object, load the form
            if (metaClass === undefined && ((_a = element.metaClass) === null || _a === void 0 ? void 0 : _a.uri) !== undefined) {
                // If the returned element has a metaclass, then set the metaClass being used to 
                // find the right form to the one by the element
                metaClass = element.metaClass.uri;
            }
            else if (element.metaClass === undefined) {
                // Updates the metaclass, if the metaclass is not set by the element itself
                element.setMetaClassByUri(metaClass);
            }
            // Asks the detail form actions, whether we have a form for the action itself
            form = yield FormActions_1.DetailFormActions.loadFormForAction(actionName);
            if (form === undefined) {
                // Defines the form
                if (configuration.formUri !== undefined) {
                    // Ok, we already have an explicit form
                    form = yield ClientForms.getForm(configuration.formUri);
                }
                else if (metaClass === undefined) {
                    // If there is no metaclass set, create a total empty form object...
                    form = Forms.FormModel.createEmptyFormWithDetail();
                }
                else {
                    form = yield ClientForms.getDefaultFormForMetaClass(metaClass);
                }
            }
            creator.element = yield ClientItems.getObjectByUri("Data", temporaryElement.uri);
            creator.formElement = form;
            creator.createFormByObject(parent, configuration);
            (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
        });
    }
    exports.createActionFormForEmptyObject = createActionFormForEmptyObject;
});
//# sourceMappingURL=ActionForm.js.map