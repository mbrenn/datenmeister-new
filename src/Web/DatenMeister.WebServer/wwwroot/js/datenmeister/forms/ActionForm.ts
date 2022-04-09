import {IFormConfiguration} from "./IFormConfiguration";
import {DmObject} from "../Mof";
import {debugElementToDom} from "../DomHelper";
import * as Forms from "./Forms";
import {DetailFormActions} from "../FormActions";

export function createActionFormForEmptyObject(
    parent: JQuery<HTMLElement>,
    metaClass: string,
    configuration: IFormConfiguration,
    actionName: string) {
    const tthis = this;

    if (configuration.refreshForm === undefined) {
        configuration.refreshForm = () => {
            createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
        }
    }

    const creator = new Forms.DetailFormCreator();

    configuration.isNewItem = true;
    configuration.onSubmit = (element) => {
        DetailFormActions.execute(
            actionName,
            creator,
            undefined,
            creator.element);
    };

    let deferLoadObjectForAction = DetailFormActions.loadObjectForAction(actionName);
    if (deferLoadObjectForAction === undefined) {
        deferLoadObjectForAction = new Promise<DmObject>(resolve => {
            resolve(new DmObject());
        });
    }

    let deferForm;

    deferLoadObjectForAction.then((element) => {
        if (metaClass === undefined && element.metaClass?.uri !== undefined) {
            // If the returned element has a metaclass, then set the metaClass being used to 
            // find the right form to the one by the element
            metaClass = element.metaClass.uri;
        } else if (element.metaClass === undefined) {
            // Updates the metaclass, if the metaclass is not set by the element itself
            element.setMetaClassByUri(metaClass);
        }

        // Defines the form
        if (configuration.formUri !== undefined) {
            // Ok, we already have an explicit form
            deferForm = Forms.getForm(configuration.formUri);
        } else if (metaClass === undefined) {
            // If there is no metaclass set, create a total empty form object...
            deferForm = new Promise<DmObject>(resolve => {
                deferForm.resolve(Forms.FormModel.createEmptyFormWithDetail());
            });
        } else {
            deferForm = Forms.getDefaultFormForMetaClass(metaClass);
        }

        deferForm.then((form) => {
            creator.element = element;
            creator.formElement = form;
            creator.createFormByObject(parent, configuration);

            debugElementToDom(form, "#debug_formelement");
        });
    });
}