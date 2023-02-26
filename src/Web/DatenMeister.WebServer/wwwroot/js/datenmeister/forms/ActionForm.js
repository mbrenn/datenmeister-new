var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Mof", "../DomHelper", "./Forms", "./ObjectForm", "../FormActions", "../client/Forms", "../client/Elements", "../client/Items", "../MofSync"], function (require, exports, Mof_1, DomHelper_1, Forms, ObjectForm, FormActions, ClientForms, ClientElements, ClientItems, MofSync) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.createActionFormForEmptyObject = void 0;
    function createActionFormForEmptyObject(parent, metaClass, configuration, actionName) {
        return __awaiter(this, void 0, void 0, function* () {
            const module = FormActions.getModule(actionName);
            if (module === undefined) {
                parent.text("Unknown action: " + actionName);
                return;
            }
            configuration.submitName = "Perform Action";
            configuration.showCancelButton = false;
            configuration.allowAddingNewProperties = false;
            if (configuration.formUri === "") {
                configuration.formUri = undefined;
            }
            if (configuration.refreshForm === undefined) {
                configuration.refreshForm = () => {
                    createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
                };
            }
            const creator = new ObjectForm.ObjectFormCreator();
            configuration.onSubmit = (element, method) => __awaiter(this, void 0, void 0, function* () {
                // Stores the most recent changes on the server        
                yield MofSync.sync(element);
                let loadedElement = yield ClientItems.getObjectByUri(element.workspace, element.uri);
                // Executes the detail form
                yield FormActions.execute(actionName, creator, loadedElement, undefined, // The action form cannot provide additional parameters as the ActionButton
                method);
            });
            /* Loads the object being used as a base for the new action.
            * Usually context information from GET-Query are retrieved. Or some default fields are filled out
            */
            let element = yield module.loadObject();
            if (element === undefined) {
                const temporaryElement = yield ClientElements.createTemporaryElement(metaClass);
                element = Mof_1.DmObjectWithSync.createFromReference(temporaryElement.workspace, temporaryElement.uri);
                // Sets the metaclass and workspace id upon url, if not created by Modules
                let p = new URLSearchParams(window.location.search);
                const metaclass = p.get('metaclass');
                if (metaclass !== undefined && metaclass !== null) {
                    element.setMetaClassByUri(metaclass);
                }
                const workspaceId = p.get('workspaceId');
                if (workspaceId !== undefined) {
                    element.set('workspaceId', workspaceId);
                }
            }
            else {
                // Checks whether the object has a copy on the server and checks the type of the object
                if (element.uri === undefined || element.workspace === undefined || element.propertiesSet === undefined) {
                    throw "Element is not linked to the server or is not of Type MofObjectWithSync";
                }
            }
            /* Now find the right form */
            let form;
            // Asks the detail form actions, whether we have a form for the action itself
            form = yield module.loadForm(metaClass);
            if (form === undefined) {
                // Defines the form
                if (configuration.formUri !== undefined) {
                    // Ok, we already have an explicit form
                    form = yield ClientForms.getForm(configuration.formUri);
                }
                else if (metaClass === undefined) {
                    // If there is no metaclass set, create a total empty form object...
                    form = Forms.FormModel.createEmptyFormObject();
                }
                else {
                    form = yield ClientForms.getObjectFormForMetaClass(metaClass);
                }
            }
            // Creates the object as being provided by the uri
            creator.element = element;
            creator.formElement = form;
            creator.workspace = "Data";
            creator.extentUri = creator.element.extentUri;
            configuration.submitName = module.actionVerb;
            // Finally, we have everything together, create the form
            yield creator.createFormByObject({
                itemContainer: parent
            }, configuration);
            // Asks the detail form actions, whether we have a form for the action itself
            yield module.preparePage(creator.element, form);
            (0, DomHelper_1.debugElementToDom)(form, "#debug_formelement");
        });
    }
    exports.createActionFormForEmptyObject = createActionFormForEmptyObject;
});
//# sourceMappingURL=ActionForm.js.map