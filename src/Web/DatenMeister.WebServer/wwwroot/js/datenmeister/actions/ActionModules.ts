﻿import * as FormActions from "../FormActions.js"
import * as Mof from "../Mof.js";
import * as MofSync from "../MofSync.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import {SubmitMethod} from "../forms/Forms.js";
import {IFormNavigation} from "../forms/Interfaces.js";
import * as ActionClient from "../client/Actions.js";


export function loadModules() {
    FormActions.addModule(new AlertClientAction());
    FormActions.addModule(new JsonItemAlertAction());
    FormActions.addModule(new ActionExecuteAction());
    FormActions.addModule(new ActionExecuteOnItemAction());
}


class AlertClientAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Alert", _DatenMeister._Actions._ClientActions.__AlertClientAction_Uri);
        this.actionVerb = "Alert";
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        alert(parameter.get(_DatenMeister._Actions._ClientActions._AlertClientAction.messageText, Mof.ObjectType.String));
    }
}

class JsonItemAlertAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("JSON.Item.Alert");
        this.actionVerb = "Trigger JSON Alert";
        this.skipSaving = true;
    }
    
    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        alert(JSON.stringify(Mof.createJsonFromObject(element)));
    }
}

class ActionExecuteAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Action.Execute");
    }

    async execute(
        form: IFormNavigation, 
        element: Mof.DmObject, 
        parameter?: Mof.DmObject, 
        submitMethod?: SubmitMethod): Promise<void> {

        // Executes the action directly
        const result = await ActionClient.executeAction(
            element.workspace,
            element.uri
        );

        if (result.success) {
            alert('Success: ' + result.result);
        } else {
            alert("Unfortunately, the action failed: \n\n" + result.reason);
        }
    }
}

class ActionExecuteOnItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Action.ExecuteOnItem");
        this.actionVerb = "Perform Action";
    }

    async loadObject() {
        const result = await MofSync.createTemporaryDmObject(_DatenMeister._CommonTypes._Default.__XmiExportContainer_Uri);

        let p = new URLSearchParams(window.location.search);

        const workspace = p.get('workspace');
        const itemUri = p.get('itemUri');
        const metaClass = p.get('metaClass');

        // Sets the reference on which the action shall be executed
        const reference = Mof.DmObject.createFromReference(workspace, itemUri);
        result.set('item', reference);

        // Sets the metaclass of the action itself
        result.setMetaClassByUri(metaClass);

        return result;
    }

    async execute(
        form: IFormNavigation,
        element: Mof.DmObject,
        parameter?: Mof.DmObject,
        submitMethod?: SubmitMethod): Promise<Mof.DmObject|undefined> {

        // Executes the action directly
        const result = await ActionClient.executeAction(
            element.workspace,
            element.uri
        );

        if (result.success) {
            return result.resultAsDmObject;
        } else {
            alert("Unfortunately, the action failed: \n\n" + result.reason);
        }
    }
}