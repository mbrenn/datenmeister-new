import {IFormConfiguration} from "./IFormConfiguration";
import * as Mof from "./Mof";
import {debugElementToDom} from "./DomHelper";
import * as Forms from "./Forms";
import {DetailFormActions} from "./FormActions";


export function createActionFormForEmptyObject(
    parent: JQuery<HTMLElement>, 
    metaClass: string,
    configuration: IFormConfiguration,
    actionName: string)
{
    const tthis = this;

    if (configuration.refreshForm === undefined) {
        configuration.refreshForm = () => {
            tthis.createActionFormForEmptyObject(parent, metaClass, configuration, actionName);
        }
    }

    const creator = new Forms.DetailFormCreator();
    creator.element = new Mof.DmObject();
    creator.element.setMetaClass(metaClass);

    configuration.onSubmit = (element) => {
        DetailFormActions.execute(
            actionName, 
            creator, 
            undefined,
            creator.element);        
    };

    if (configuration.formUri !== undefined) {
        const defer = Forms.getForm(configuration.formUri);
        $.when(defer).then(form => {
            creator.formElement = form;
            creator.createFormByObject(parent, configuration);

            debugElementToDom(form, "#debug_formelement");
        });
    }
    if (metaClass === undefined) {
        // Create a total empty form object... 
        creator.formElement = Forms.FormModel.createEmptyFormWithDetail();
        creator.createFormByObject(parent, configuration);
    } else {
        const defer = Forms.getDefaultFormForMetaClass(metaClass);
        $.when(defer).then(form => {
            creator.formElement = form;
            creator.createFormByObject(parent, configuration);

            debugElementToDom(form, "#debug_formelement");
        });
    }
}