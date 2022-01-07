import {IFormConfiguration} from "./IFormConfiguration";
import * as Mof from "./Mof";
import {DmObject} from "./Mof";
import {debugElementToDom} from "./DomHelper";
import * as Forms from "./Forms";
import {DetailFormActions} from "./FormActions";


export function createActionFormForEmptyObject(
    parent: JQuery<HTMLElement>,
    metaClass: string,
    configuration: IFormConfiguration,
    actionName: string) {
    const tthis = this;

    if (configuration.refreshForm === undefined) {
        configuration.refreshForm = () => {
            tthis.createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
        }
    }

    const creator = new Forms.DetailFormCreator();
    creator.element = new Mof.DmObject();
    creator.element.setMetaClassByUri(metaClass);

    configuration.onSubmit = (element) => {
        DetailFormActions.execute(
            actionName,
            creator,
            undefined,
            creator.element);
    };

    let deferLoadObjectForAction = DetailFormActions.loadObjectForAction(actionName);
    if (deferLoadObjectForAction === undefined) {
        deferLoadObjectForAction = $.Deferred<DmObject>();
        deferLoadObjectForAction.resolve(undefined);
    }

    let deferForm;

    if (configuration.formUri !== undefined) {
        deferForm = Forms.getForm(configuration.formUri);
    } else if (metaClass === undefined) {
        deferForm = $.Deferred<DmObject>();
        deferForm.resolve(Forms.FormModel.createEmptyFormWithDetail());
        // Create a total empty form object...
    } else {
        deferForm = Forms.getDefaultFormForMetaClass(metaClass);
    }

    $.when(deferForm, deferLoadObjectForAction).then((form, element) => {
        creator.element = element;
        creator.formElement = form;
        creator.createFormByObject(parent, configuration);

        debugElementToDom(form, "#debug_formelement");
    });
}