import * as FormActions from "../FormActions"
import {DmObject} from "../Mof";
import * as FormClient from "../client/Forms";
import * as ActionClient from "../client/Actions";
import {IFormNavigation} from "../forms/Interfaces";
import {SubmitMethod} from "../forms/RowForm";
import * as Settings from "../Settings";
import {_DatenMeister} from "../models/DatenMeister.class";
import _MoveOrCopyAction = _DatenMeister._Actions._MoveOrCopyAction;
import * as ItemClient from "../client/Items";
import * as Navigator from "../Navigator";
import {moveItemInCollectionDown, moveItemInCollectionUp} from "../client/Actions.Items";

export function loadModules() {   
    FormActions.addModule(new ItemMoveOrCopyActionNavigate());
    FormActions.addModule(new ItemMoveOrCopyAction());
    FormActions.addModule(new ItemDeleteAction());
    FormActions.addModule(new ItemMoveUpItemAction());
    FormActions.addModule(new ItemMoveDownItemAction());
}

class ItemMoveOrCopyActionNavigate extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.MoveOrCopy.Navigate");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        document.location.href =
            Settings.baseUrl + "ItemAction/Item.MoveOrCopy?workspaceId=" + encodeURIComponent(element.workspace)
            + "&itemUri=" + encodeURIComponent(element.uri);
    }
}

class ItemMoveOrCopyAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.MoveOrCopy");
    }
    
    async loadObject(): Promise<DmObject> | undefined {

        let p = new URLSearchParams(window.location.search);
        
        const result = new DmObject();
        result.setMetaClassByUri(_DatenMeister._Actions.__MoveOrCopyAction_Uri);

        // TODO: Set Result
        const sourceWorkspace = p.get('workspaceId');
        const sourceItemUri = p.get('itemUri');

        const source = DmObject.createFromReference(sourceWorkspace, sourceItemUri);
        result.set(_MoveOrCopyAction.source, source);

        return Promise.resolve(result);
    }

    async loadForm(): Promise<DmObject> | undefined {
        return await FormClient.getForm("dm:///_internal/forms/internal#Item.MoveOrCopy");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        // Executes the action directly
        const result = await ActionClient.executeAction(
            element.workspace,
            element.uri
        );

        if (result.success) {
            alert('Success');
        } else {
            alert('Failure');
        }
    }
}

class ItemDeleteAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.Delete");
        this.requiresConfirmation = true;
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const data = await ItemClient.deleteItem(form.workspace, element.uri);

        const success = data.success;
        if (success) {
            Navigator.navigateToWorkspace(form.workspace);
        } else {
            alert('Deletion was not successful.');
        }
    }
}

class ItemMoveDownItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.MoveDownItem");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        await moveItemInCollectionDown(
            form.workspace,
            form.itemUrl,
            form.formElement.get(_DatenMeister._Forms._TableForm.property),
            element.uri);
        document.location.reload();
    }
}

class ItemMoveUpItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.MoveUpItem");
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        await moveItemInCollectionUp(
            form.workspace,
            form.itemUrl,
            form.formElement.get(_DatenMeister._Forms._TableForm.property),
            element.uri);
        document.location.reload();
    }
}