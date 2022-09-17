import * as Settings from "./Settings";
import * as ApiConnection from "./ApiConnection";
import * as Navigator from "./Navigator";
import {createJsonFromObject, DmObject} from "./Mof";
import * as IIForms from "./forms/Interfaces";
import * as ECClient from "./client/Extents";
import * as ItemClient from "./client/Items";
import * as FormClient from "./client/Forms";
import * as ActionClient from "./client/Actions";
import * as DatenMeisterModel from "./models/DatenMeister.class";

import {SubmitMethod} from "./forms/RowForm";
import {_DatenMeister} from "./models/DatenMeister.class";

export module DetailFormActions {

    // Loads the object being used for the action. 
    import _MoveOrCopyAction = _DatenMeister._Actions._MoveOrCopyAction;

    export async function loadObjectForAction(actionName: string): Promise<DmObject> | undefined {

        let p = new URLSearchParams(window.location.search);

        if (actionName === "Extent.Properties.Update") {
            const workspace = p.get('workspace');
            const extentUri = p.get('extent');

            return await ECClient.getProperties(workspace, extentUri);
        }

        if (actionName === "Extent.CreateItem"
            || actionName === "Extent.CreateItemInProperty"
            || actionName === "Workspace.Extent.LoadOrCreate.Step2") {
            const metaclass = p.get('metaclass');
            const result = new DmObject();
            if (metaclass !== undefined && metaclass !== null) {
                result.setMetaClassByUri(metaclass);
            }

            const workspaceId = p.get('workspaceId');
            if (workspaceId !== undefined) {
                result.set('workspaceId', workspaceId);
            }

            return Promise.resolve(result);
        }

        if (actionName === "Zipcode.Test") {

            const result = new DmObject();
            result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
            return Promise.resolve(result);
        }

        if (actionName === "Workspace.Extent.Xmi.Create") {

            const result = new DmObject();
            result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");
            result.set("workspaceId", p.get('workspaceId'));

            return Promise.resolve(result);
        }

        if (actionName === "Item.MoveOrCopy") {

            const result = new DmObject();
            result.setMetaClassByUri(_DatenMeister._Actions.__MoveOrCopyAction_Uri);

            // TODO: Set Result
            const sourceWorkspace = p.get('workspaceId');
            const sourceItemUri = p.get('itemUri');

            const source = DmObject.createFromReference(sourceWorkspace, sourceItemUri);
            result.set(_MoveOrCopyAction.source, source);

            return Promise.resolve(result);
        }

        /* Nothing has been found, so return an undefined */
        return Promise.resolve(undefined);
    }

    /* Finds the best form fitting for the action */
    export async function loadFormForAction(actionName: string) {
        if (actionName === 'Workspace.Extent.LoadOrCreate') {
            return await FormClient.getForm("dm:///_internal/forms/internal#WorkspacesAndExtents.Extent.SelectType");
        }
        if (actionName === 'Forms.Create.ByMetaClass') {
            return await FormClient.getForm("dm:///_internal/forms/internal#Forms.Create.ByMetaClass");
        }
        if (actionName === 'Item.MoveOrCopy') {
            return await FormClient.getForm("dm:///_internal/forms/internal#Item.MoveOrCopy");
        }

        return Promise.resolve(undefined);
    }

    export function requiresConfirmation(actionName: string): boolean {
        if (actionName === "Item.Delete"
            || actionName === "ExtentsList.DeleteItem"
            || actionName === "Extent.DeleteExtent") {
            return true;
        } else {
            return false;
        }
    }

    // Calls to execute the form actions.
    // actionName: Name of the action to be executed. This is a simple string describing the action
    // form: The form which was used to trigger the action
    // itemUrl: The url of the item whose action will be executed
    // element: The element which is reflected within the form
    // parameter: These parameter are retrieved from the actionForm definition from the server and are forwarded
    //    This supports the server to provide additional parameter for an action button
    // submitMethod: Describes which button the user has clicked
    export async function execute(actionName: string, form: IIForms.IFormNavigation, itemUrl: string, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod) {
        let workspaceId;
        let extentUri;
        let p = new URLSearchParams(window.location.search);
        switch (actionName) {
            case "Extent.NavigateTo":
                extentUri = element.get('uri');
                workspaceId = element.get('workspaceId');
                FormActions.extentNavigateTo(workspaceId, extentUri);
                break;
            case "Extent.DeleteExtent":
                extentUri = element.get('uri');
                workspaceId = element.get('workspaceId');
                FormActions.extentDelete(workspaceId, extentUri);
                break;
            case "Extent.Properties":
                extentUri = element.get('uri');
                workspaceId = element.get('workspaceId');
                FormActions.extentNavigateToProperties(workspaceId, extentUri);
                break;
            case "Extent.Properties.Update":
                if (!p.has("extent") || !p.has("workspace")) {
                    alert('There is no extent given');
                } else {
                    const workspace = p.get('workspace');
                    const extentUri = p.get('extent');
                    FormActions.extentUpdateExtentProperties(workspace, extentUri, element);
                }
                break;
            case "Extent.CreateItem":
                if (!p.has("extent") || !p.has("workspace")) {
                    alert('There is no extent given');
                } else {
                    const workspace = p.get('workspace');
                    const extentUri = p.get('extent');
                    await FormActions.extentCreateItem(workspace, extentUri, element, undefined, submitMethod);
                }
                break;
            case "Extent.CreateItemInProperty":
                if (!p.has("itemUrl") || !p.has("workspace") || !p.has("property")) {
                    alert('There is no itemUrl given');
                } else {
                    const workspace = p.get('workspace');
                    const itemUrl = p.get('itemUrl');
                    const property = p.get('property');
                    const metaclass = p.get('metaclass');
                    await FormActions.extentCreateItemInProperty(workspace, itemUrl, property, element, metaclass);
                }
                break;
            case "ExtentsList.ViewItem":
                FormActions.itemNavigateTo(form.workspace, element.uri);
                break;
            case "ExtentsList.DeleteItem":
                await FormActions.extentsListDeleteItem(form.workspace, form.extentUri, itemUrl);
                break;
            case "Item.Delete":
                await FormActions.itemDelete(form.workspace, form.extentUri, itemUrl);
                break;
            case "ZipExample.CreateExample":
                const id = element.get('id');
                await ApiConnection.post(
                    Settings.baseUrl + "api/zip/create",
                    {workspace: id})
                    .then(
                        data => {
                            document.location.reload();
                        });
                break;
            case "Workspace.Extent.Xmi.Create.Navigate": {
                const workspaceIdParameter = parameter?.get('workspaceId') ?? "";
                await FormActions.workspaceExtentCreateXmiNavigateTo(workspaceIdParameter);
                break;
            }

            case "Workspace.Extent.LoadOrCreate.Navigate": {
                const workspaceIdParameter = p?.get('workspaceId') ?? "";
                await FormActions.workspaceExtentLoadAndCreateNavigateTo(workspaceIdParameter);
                break;
            }

            case "Workspace.Extent.LoadOrCreate": {
                const workspaceIdParameter = p?.get('workspaceId') ?? "";
                const extentType = await ItemClient.getProperty("Data", element.uri, "extentType") as DmObject;

                if (extentType === null || extentType === undefined) {
                    alert('No Extent Type has been selected');
                } else {
                    document.location.href = Settings.baseUrl +
                        "ItemAction/Workspace.Extent.LoadOrCreate.Step2" +
                        "?metaclass=" + encodeURIComponent(extentType.uri) +
                        (workspaceIdParameter !== undefined
                            ? ("&workspaceId=" + encodeURIComponent(workspaceIdParameter))
                            : "");
                }

                break;
            }

            case "Workspace.Extent.LoadOrCreate.Step2": {
                const extentCreationParameter = new DmObject();
                extentCreationParameter.set('configuration', element);
                extentCreationParameter.setMetaClassByUri(
                    DatenMeisterModel._DatenMeister._Actions.__LoadExtentAction_Uri
                )

                const result = await ActionClient.executeActionDirectly(
                    "Execute",
                    {
                        parameter: extentCreationParameter
                    }
                );

                if (result.success !== true) {
                    alert('Extent was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
                } else {
                    alert('Extent was created successfully');
                }

                break;
            }

            case "Forms.Create.ByMetaClass": {
                const extentCreationParameter = new DmObject();
                extentCreationParameter.set('configuration', element);
                extentCreationParameter.setMetaClassByUri(
                    DatenMeisterModel._DatenMeister._Actions.__CreateFormByMetaClass_Uri
                );

                const result = await ActionClient.executeActionDirectly(
                    "Execute",
                    {
                        parameter: extentCreationParameter
                    }
                );

                if (result.success !== true) {
                    alert('Form was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
                } else {
                    alert('Form was created successfully');
                }

                break;
            }

            case "Workspace.Extent.Xmi.Create": {

                const extentCreationParameter = new DmObject();
                extentCreationParameter.set('configuration', element);
                extentCreationParameter.setMetaClassByUri(
                    DatenMeisterModel._DatenMeister._Actions.__LoadExtentAction_Uri
                );
                
                const result = await ActionClient.executeActionDirectly(
                    "Execute",
                    {
                            parameter: extentCreationParameter
                    }
                );

                if (result.success) {
                    document.location.href = Settings.baseUrl
                        + "ItemsOverview/" + encodeURIComponent(element.get("workspaceId")) +
                        "/" + encodeURIComponent(element.get("extentUri"))
                } else {
                    alert(result.reason);
                }
            }
                break;

            case "Item.MoveOrCopy.Navigate":
                await FormActions.itemMoveOrCopyNavigateTo(element.workspace, element.uri);
                break;
            case "JSON.Item.Alert":
                alert(JSON.stringify(createJsonFromObject(element)));
                break;
            case "Zipcode.Test":
                alert(element.get('zip')?.toString() ?? "No Zip Code given");
                break;

            case "Item.MoveOrCopy":
            case "Action.Execute":
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

                break;

            default:
                alert("Unknown action type: " + actionName);
                break;
        }
    }
}

interface IDeleteCallbackData {
    success: boolean;
}

export class FormActions {

    static workspaceExtentCreateXmiNavigateTo(workspaceId: string) {
        document.location.href =
            Settings.baseUrl + "ItemAction/Workspace.Extent.Xmi.Create?metaClass=" +
            encodeURIComponent(_DatenMeister._ExtentLoaderConfigs.__XmiStorageLoaderConfig_Uri) +
            "&workspaceId=" + encodeURIComponent(workspaceId);
    }
    
    static workspaceExtentLoadAndCreateNavigateTo(workspaceId: string) {
        document.location.href =
            Settings.baseUrl + "ItemAction/Workspace.Extent.LoadOrCreate?workspaceId=" + encodeURIComponent(workspaceId);
    }
    
    static itemMoveOrCopyNavigateTo(workspaceId: string, itemUri: string) {
        document.location.href =
            Settings.baseUrl + "ItemAction/Item.MoveOrCopy?workspaceId=" + encodeURIComponent(workspaceId) 
            + "&itemUri=" + encodeURIComponent(itemUri);
    }

    static async extentCreateItem(workspace: string, extentUri: string, element: DmObject, metaClass?: string, submitMethod?: SubmitMethod) {
        if (metaClass === undefined) {
            metaClass = element.metaClass.uri
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

    static workspaceNavigateTo(workspace: string) {
        document.location.href =
            Settings.baseUrl + "Item/Management/dm:%2F%2F%2F_internal%2Fworkspaces/" + encodeURIComponent(workspace);
    }

    static async extentCreateItemInProperty(workspace: string, itemUrl: string, property: string, element: DmObject, metaClass?: string) {
        const json = createJsonFromObject(element);
        await ApiConnection.post(
            Settings.baseUrl + "api/items/create_child/" + encodeURIComponent(workspace) + "/" + encodeURIComponent(itemUrl),
            {
                metaClass: (metaClass === undefined || metaClass === null) ? "" : metaClass,
                property: property,
                asList: true,
                properties: json
            }
        );

        Navigator.navigateToItemByUrl(workspace, itemUrl);
    }

    static extentNavigateTo(workspace: string, extentUri: string): void {
        document.location.href =
            Settings.baseUrl + "ItemsOverview/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri);
    }

    static extentNavigateToProperties(workspace: string, extentUri: string) {
        document.location.href =
            Settings.baseUrl +
            "ItemAction/Extent.Properties.Navigate/" +
            encodeURIComponent("dm:///_internal/forms/internal#DatenMeister.Extent.Properties") +
            "?workspace=" +
            encodeURIComponent(workspace) +
            "&extent=" +
            encodeURIComponent(extentUri);
    }

    static extentUpdateExtentProperties(workspace: string, extentUri: string, element: DmObject): void {
        ECClient.setProperties(workspace, extentUri, element).then(() => FormActions.extentNavigateTo(workspace, extentUri));
    }

    static extentDelete(workspace: string, extentUri: string): void {
        const parameter = {
            workspace: workspace,
            extentUri: extentUri
        }

        ApiConnection.deleteRequest(
            Settings.baseUrl + "api/extent/delete",
            parameter
        ).then(() => {
            FormActions.workspaceNavigateTo(workspace);
        });
    }

    static createZipExample(workspace: string) {
        ApiConnection.post(
            Settings.baseUrl + "api/zip/create",
            {workspace: workspace})
            .then(
                data => {
                    document.location.reload();
                });
    }

    // Performs the navigation to the given item. The ItemUrl may be a uri or just the id
    static itemNavigateTo(workspace: string, itemUrl: string) {
        Navigator.navigateToItemByUrl(
            workspace,
            itemUrl);
    }

    static itemNew(workspace: string, extentUri: string) {
        ApiConnection.post(
            Settings.baseUrl + "api/items/create",
            {
                workspace: workspace,
                extentUri: extentUri
            })
            .then(
                data => {
                    document.location.reload();
                });
    }

    static async itemDelete(workspace: string, extentUri: string, itemUri: string) {
        
        const data = await ItemClient.deleteItem(workspace, itemUri);

        const success = data.success;
        if (success) {
            Navigator.navigateToWorkspace(workspace);
        } else {
            alert('Deletion was not successful.');
        }
    }

    static extentsListViewItem(workspace: string, extentUri: string, itemId: string) {
        document.location.href = Settings.baseUrl + "Item/" +
            encodeURIComponent(workspace) + "/" +
            encodeURIComponent(extentUri) + "/" +
            encodeURIComponent(itemId);
    }

    static async extentsListDeleteItem(workspace: string, extentUri: string, itemId: string) {

        const data = await ApiConnection.deleteRequest<IDeleteCallbackData>(
            Settings.baseUrl + "api/items/delete/"
            + encodeURIComponent(workspace) + "/" +
            encodeURIComponent(itemId),
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