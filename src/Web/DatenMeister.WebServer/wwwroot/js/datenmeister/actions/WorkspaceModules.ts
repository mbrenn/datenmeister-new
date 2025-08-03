import * as FormActions from "../FormActions.js"
import {DmObject, DmObjectWithSync} from "../Mof.js";
import * as MofSync from "../MofSync.js";
import * as ActionClient from "../client/Actions.js";
import {IFormNavigation} from "../forms/Interfaces.js";
import {SubmitMethod} from "../forms/Forms.js";
import * as Settings from "../Settings.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as ItemClient from "../client/Items.js";
import * as FormClient from "../client/Forms.js";
import * as Navigator from "../Navigator.js";

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

class WorkspaceExtentLoadOrCreateNavigateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Workspace.Extent.LoadOrCreate.Navigate");
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const workspaceId = parameter?.get('workspaceId') ?? "";
        document.location.href =
            Settings.baseUrl + "ItemAction/Workspace.Extent.LoadOrCreate?workspaceId=" + encodeURIComponent(workspaceId);
    }
}

class WorkspaceExtentLoadOrCreateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Workspace.Extent.LoadOrCreate");
        this.actionHeading = 'Create/Load Extent - Select Extent Type';
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

    async loadObject(): Promise<DmObjectWithSync> | undefined {
        let p = new URLSearchParams(window.location.search);
        const workspaceId= p.get("workspaceId");
        const metaClassUri = p.get("metaclass");

        const result =
            await MofSync.createTemporaryDmObject(metaClassUri);
        result.set("workspaceId", workspaceId);

        return Promise.resolve(result);
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const extentCreationParameter = new DmObject();
        extentCreationParameter.set('configuration', element);
        extentCreationParameter.setMetaClassByUri(
            _DatenMeister._Actions.__LoadExtentAction_Uri, 
            'Types'
        );

        const result = await ActionClient.executeActionDirectly(
            "Execute",
            {
                parameter: extentCreationParameter
            }
        );

        if (result.success !== true) {
            alert('Extent was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
        } else {
            Navigator.navigateToExtentItems(element.get("workspaceId"), element.get("extentUri"));
        }
    }
}


class WorkspaceExtentXmiCreateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Workspace.Extent.Xmi.Create");
        this.actionVerb = "Create Xmi Extent";
    }
    
    async loadObject(): Promise<DmObjectWithSync> | undefined {
        let p = new URLSearchParams(window.location.search);
        
        const result = await MofSync.createTemporaryDmObject(_DatenMeister._ExtentLoaderConfigs.__XmiStorageLoaderConfig_Uri);
        result.set("workspaceId", p.get('workspaceId'));

        return Promise.resolve(result);
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {

        const extentCreationParameter = new DmObject();
        extentCreationParameter.set('configuration', element);
        extentCreationParameter.setMetaClassByUri(
            _DatenMeister._Actions.__LoadExtentAction_Uri,'Types'
        );

        const result = await ActionClient.executeActionDirectly(
            "Execute",
            {
                parameter: extentCreationParameter
            }
        );

        if (result.success) {
            Navigator.navigateToExtentItems(element.get("workspaceId"), element.get("extentUri"));
        } else {
            alert(result.reason);
        }
    }
}