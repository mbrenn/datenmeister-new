import * as FormActions from "../FormActions"
import {createJsonFromObject, DmObject} from "../Mof";
import {SubmitMethod} from "../forms/RowForm";
import {IFormNavigation} from "../forms/Interfaces";
import * as ApiConnection from "../ApiConnection";
import * as Settings from "../Settings";
import * as ECClient from "../client/Extents";
import * as ItemClient from "../client/Items";
import * as Navigator from "../Navigator";
import {moveItemInExtentDown, moveItemInExtentUp} from "../client/Actions.Items";

export function loadModules() {
    FormActions.addModule(new ExtentPropertiesUpdateAction());
    FormActions.addModule(new ExtentCreateItemAction());
    FormActions.addModule(new ExtentDeleteAction());
    FormActions.addModule(new ExtentNavigateToAction());
    FormActions.addModule(new ExtentPropertiesAction());
    FormActions.addModule(new ExtentCreateItemInPropertyAction());
    FormActions.addModule(new ExtentsListViewItemAction());
    FormActions.addModule(new ExtentsListDeleteItemAction());
    FormActions.addModule(new ExtentsListMoveUpItemAction());
    FormActions.addModule(new ExtentsListMoveDownItemAction());
    
}

class ExtentPropertiesUpdateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.Properties.Update");
    }

    async loadObject(): Promise<DmObject> | undefined {

        let p = new URLSearchParams(window.location.search);

        const workspace = p.get('workspace');
        const extentUri = p.get('extent');

        return await ECClient.getProperties(workspace, extentUri);
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        let p = new URLSearchParams(window.location.search);

        if (!p.has("extent") || !p.has("workspace")) {
            alert('There is no extent given');
        } else {
            const workspace = p.get('workspace');
            const extentUri = p.get('extent');

            await ECClient.setProperties(workspace, extentUri, element);
            Navigator.navigateToExtent(workspace, extentUri);
        }
    }
}

class ExtentCreateItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.CreateItem");
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        let p = new URLSearchParams(window.location.search);

        if (!p.has("extent") || !p.has("workspace")) {
            alert('There is no extent given');
        } else {
            const workspace = p.get('workspace');
            const extentUri = p.get('extent');
            await this.extentCreateItem(workspace, extentUri, element, undefined, submitMethod);
        }
    }

    async extentCreateItem(workspace: string, extentUri: string, element: DmObject, metaClass?: string, submitMethod?: SubmitMethod) {
        if (metaClass === undefined) {
            metaClass = element.metaClass?.uri;
        }

        const json = createJsonFromObject(element);

        const newItem = await ItemClient.createItemInExtent(
            workspace, extentUri,
            {
                metaClass: metaClass === undefined ? "" : metaClass,
                properties: element
            }
        );

        if (submitMethod === SubmitMethod.Save) {
            // If user has clicked on the save button (without closing), the form shall just be updated
            Navigator.navigateToItemByUrl(workspace, newItem.itemId);
        } else {
            // Else, move to the overall items overview
            document.location.href = Settings.baseUrl +
                "ItemsOverview/" +
                encodeURIComponent(workspace) +
                "/" +
                encodeURIComponent(extentUri);
        }
    }
}

class ExtentCreateItemInPropertyAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.CreateItemInProperty");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        let p = new URLSearchParams(window.location.search);
        
        if (!p.has("itemUrl") || !p.has("workspace") || !p.has("property")) {
            alert('There is no itemUrl given');
        } else {
            const workspace = p.get('workspace');
            const itemUrl = p.get('itemUrl');
            const property = p.get('property');
            const metaclass = p.get('metaclass');
            
            const json = createJsonFromObject(element);
            await ApiConnection.post(
                Settings.baseUrl + "api/items/create_child/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(itemUrl),
                {
                    metaClass: (metaclass === undefined || metaclass === null) ? "" : metaclass,
                    property: property,
                    asList: true,
                    properties: json
                }
            );

            Navigator.navigateToItemByUrl(workspace, itemUrl);
        }
    }
}
    
class ExtentDeleteAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.DeleteExtent");
        this.requiresConfirmation = true;
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        let extentUri = element.get('uri');
        let workspaceId = element.get('workspaceId');
        const deleteParameter = {
            workspace: workspaceId,
            extentUri: extentUri
        }

        await ApiConnection.deleteRequest(
            Settings.baseUrl + "api/extent/delete",
            deleteParameter
        );
        
        Navigator.navigateToWorkspace(workspaceId);
    }
}

class ExtentNavigateToAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.NavigateTo");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        let extentUri = element.get('uri');
        let workspaceId = element.get('workspaceId');
        Navigator.navigateToExtent(workspaceId, extentUri);
        
    }
}

class ExtentPropertiesAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.Properties");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        let extentUri = element.get('uri');
        let workspaceId = element.get('workspaceId');

        document.location.href =
            Settings.baseUrl +
            "ItemAction/Extent.Properties.Navigate/" +
            encodeURIComponent("dm:///_internal/forms/internal#DatenMeister.Extent.Properties") +
            "?workspace=" +
            encodeURIComponent(workspaceId) +
            "&extent=" +
            encodeURIComponent(extentUri);
    }
}


class ExtentsListViewItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ExtentsList.ViewItem");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        Navigator.navigateToItemByUrl(
            form.workspace,
            element.uri);
    }
}


interface IDeleteCallbackData {
    success: boolean;
}

class ExtentsListDeleteItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ExtentsList.DeleteItem");
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        const data = await ApiConnection.deleteRequest<IDeleteCallbackData>(
            Settings.baseUrl + "api/items/delete/"
            + encodeURIComponent(form.workspace) + "/" +
            encodeURIComponent(form.itemUrl),
            {}
        );

        const success = data.success;
        if (success) {
            document.location.reload();
        } else {
            alert('Deletion was not successful.');
        }
    }
}


class ExtentsListMoveUpItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ExtentsList.MoveUpItem");
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        await moveItemInExtentUp(form.workspace, form.extentUri, form.itemUrl);
        document.location.reload();
    }
}


class ExtentsListMoveDownItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ExtentsList.MoveDownItem");
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        await moveItemInExtentDown(form.workspace, form.extentUri, form.itemUrl);
        document.location.reload();
    }
}