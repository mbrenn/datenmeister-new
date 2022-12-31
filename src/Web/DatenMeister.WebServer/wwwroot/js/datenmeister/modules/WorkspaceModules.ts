import * as FormActions from "../FormActions"
import {DmObject} from "../Mof";
import * as ActionClient from "../client/Actions";
import {IFormNavigation} from "../forms/Interfaces";
import {SubmitMethod} from "../forms/RowForm";
import * as Settings from "../Settings";
import {_DatenMeister} from "../models/DatenMeister.class";
import * as ItemClient from "../client/Items";
import * as DatenMeisterModel from "../models/DatenMeister.class";
import * as FormClient from "../client/Forms";

export function loadModules() {
    FormActions.addModule(new WorkspaceExtentXmiCreateNavigateAction());
    FormActions.addModule(new WorkspaceExtentLoadOrCreateNavigateAction());
    FormActions.addModule(new WorkspaceExtentLoadOrCreateAction());
    FormActions.addModule(new WorkspaceExtentLoadOrCreateStep2Action());
    FormActions.addModule(new WorkspaceExtentXmiCreateAction());
}

class WorkspaceExtentXmiCreateNavigateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Workspace.Extent.Xmi.Create.Navigate");
        this.skipSaving = true;
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        const workspaceId = parameter?.get('workspaceId') ?? "";

        document.location.href =
            Settings.baseUrl + "ItemAction/Workspace.Extent.Xmi.Create?metaClass=" +
            encodeURIComponent(_DatenMeister._ExtentLoaderConfigs.__XmiStorageLoaderConfig_Uri) +
            "&workspaceId=" + encodeURIComponent(workspaceId);
    }
}

class WorkspaceExtentLoadOrCreateNavigateAction extends FormActions.ItemFormActionModuleBase
{
    constructor() {
        super("Workspace.Extent.LoadOrCreate.Navigate");
        this.skipSaving = true;
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        let p = new URLSearchParams(window.location.search);
        const workspaceId = p?.get('workspaceId') ?? "";

        document.location.href =
            Settings.baseUrl + "ItemAction/Workspace.Extent.LoadOrCreate?workspaceId=" + encodeURIComponent(workspaceId);
    }
}

class WorkspaceExtentLoadOrCreateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Workspace.Extent.LoadOrCreate");
        this.actionVerb = "Choose Extent Type";
    }
    
    async loadForm(): Promise<DmObject> | undefined {
        return await FormClient.getForm("dm:///_internal/forms/internal#WorkspacesAndExtents.Extent.SelectType");
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        let p = new URLSearchParams(window.location.search);
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
    }
}

class WorkspaceExtentLoadOrCreateStep2Action extends FormActions.ItemFormActionModuleBase {

    constructor() {
        super("Workspace.Extent.LoadOrCreate.Step2");
        this.actionVerb = "Create/Load Extent";
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
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
    }
}


class WorkspaceExtentXmiCreateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Workspace.Extent.Xmi.Create");
        this.actionVerb = "Create Xmi Extent";
    }
    
    async loadObject(): Promise<DmObject> | undefined {
        let p = new URLSearchParams(window.location.search);
        
        const result = new DmObject();
        result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");
        result.set("workspaceId", p.get('workspaceId'));

        return Promise.resolve(result);
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

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
}