import * as FormActions from "../FormActions.js"
import {DmObject, DmObjectWithSync, ObjectType} from "../Mof.js";
import * as MofSync from "../MofSync.js";
import * as FormClient from "../client/Forms.js";
import * as ActionClient from "../client/Actions.js";
import {IFormNavigation} from "../forms/Interfaces.js";
import {SubmitMethod} from "../forms/RowForm.js";
import * as Settings from "../Settings.js";
import {_DatenMeister} from "../models/DatenMeister.class.js";
import * as ItemClient from "../client/Items.js";
import * as Navigator from "../Navigator.js";
import {moveItemInCollectionDown, moveItemInCollectionUp} from "../client/Actions.Items.js";
import _MoveOrCopyAction = _DatenMeister._Actions._MoveOrCopyAction;
import * as ClientElements from "../client/Elements.js";

export function loadModules() {   
    FormActions.addModule(new ItemMoveOrCopyActionNavigate());
    FormActions.addModule(new ItemMoveOrCopyAction());
    FormActions.addModule(new ItemDeleteAction());
    FormActions.addModule(new ItemMoveUpItemAction());
    FormActions.addModule(new ItemMoveDownItemAction());
    FormActions.addModule(new ItemXmiExportNavigate());
    FormActions.addModule(new ItemXmiExport());
    FormActions.addModule(new ItemXmiImportNavigate());
    FormActions.addModule(new ItemXmiImport());
    FormActions.addModule(new ItemCreateTemporarySetMetaclass());
}

class ItemMoveOrCopyActionNavigate extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.MoveOrCopy.Navigate");
        this.skipSaving = true;   
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
        this.actionVerb = "Move/Copy Item";
    }
    
    async loadObject(): Promise<DmObjectWithSync> | undefined {

        let p = new URLSearchParams(window.location.search);
        
        const result = await MofSync.createTemporaryDmObject(_DatenMeister._Actions.__MoveOrCopyAction_Uri);

        // TODO: Set Result
        const sourceWorkspace = p.get('workspaceId');
        const sourceItemUri = p.get('itemUri');

        const source = DmObject.createFromReference(sourceWorkspace, sourceItemUri);
        result.set(_MoveOrCopyAction.source, source);
        
        const container = await ItemClient.getContainer(sourceWorkspace, sourceItemUri);
        if (container.length > 0) {
            const target = DmObject.createFromReference(container[0].workspace, container[0].uri);
            result.set(_MoveOrCopyAction.target, target);
        }

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
            alert('Failure: \r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
        }
    }
}

class ItemDeleteAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.Delete");
        this.requiresConfirmation = true;
        this.actionVerb = "Delete Item";
        this.skipSaving = true;
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
        this.actionVerb = "Move Up";
        this.skipSaving = true;
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
        this.actionVerb = "Move Down";
        this.skipSaving = true;
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

class ItemXmiExportNavigate extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.ExportXmi.Navigate");
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        Navigator.navigateToAction(
            "Item.ExportXmi",
            "dm:///_internal/forms/internal#DatenMeister.Export.Xmi",
            {
                workspace: element.workspace,
                itemUri: element.uri
            }
        );
    }
}

class ItemXmiExport extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.ExportXmi");
    }

    async loadObject(): Promise<DmObjectWithSync> {
        let p = new URLSearchParams(window.location.search);

        if (!p.has("itemUri") || !p.has("workspace")) {
            alert('There is no workspace and extentUri given');
            throw 'There is no workspace and extentUri given';
        } else {
            const workspace = p.get('workspace');
            const itemUri = p.get('itemUri');

            // Export the Xmi and stores it into the element
            const exportedXmi = await ItemClient.exportXmi(workspace, itemUri);
            const result = await MofSync.createTemporaryDmObject(_DatenMeister._CommonTypes._Default.__XmiExportContainer_Uri);
            result.set(_DatenMeister._CommonTypes._Default._XmiExportContainer.xmi, exportedXmi.xmi);
            return result;
        }
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        alert('Nothing to do');
    }
}

class ItemXmiImportNavigate extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.ImportXmi.Navigate");
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        Navigator.navigateToAction(
            "Item.ImportXmi",
            "dm:///_internal/forms/internal#DatenMeister.Import.Item.Xmi",
            {
                workspace: element.workspace,
                itemUri: element.uri,
                metaClass: _DatenMeister._CommonTypes._Default.__XmiImportContainer_Uri
            });
    }
}

class ItemXmiImport extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.ImportXmi");
        this.actionVerb = "Perform Import";
    }

    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        alert('Now, we do the import');
        let p = new URLSearchParams(window.location.search);

        if (!p.has("itemUri") || !p.has("workspace")) {
            alert('There is no workspace and extentUri given');
            throw 'There is no workspace and extentUri given';
        } else {
            const workspace = p.get('workspace');
            const itemUri = p.get('itemUri');

            // Export the Xmi and stores it into the element
            const importedXmi = await ItemClient.importXmi(
                workspace,
                itemUri,
                element.get(_DatenMeister._CommonTypes._Default._XmiImportContainer.property, ObjectType.String),
                element.get(_DatenMeister._CommonTypes._Default._XmiImportContainer.addToCollection, ObjectType.Boolean),
                element.get(_DatenMeister._CommonTypes._Default._XmiImportContainer.xmi, ObjectType.String));

            if (importedXmi.success) {
                Navigator.navigateToItemByUrl(workspace, itemUri);
            } else {
                alert('Something failed');
            }
        }
    }
}

class ItemCreateTemporarySetMetaclass extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Item.Create.Temporary.SetMetaclass");
        this.actionVerb = "Define Metaclass";
    }

    async loadForm(): Promise<DmObject> | undefined {
        return await FormClient.getForm("dm:///_internal/forms/internal#Item.Create.Temporary.SetMetaclass");
    }
    
    async execute(form: IFormNavigation, element: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const foundElement = element.get("metaClass", ObjectType.Default) as DmObject;
        
        const temporaryElement = await ClientElements.createTemporaryElement(foundElement.uri);        
        Navigator.navigateToItemByUrl(temporaryElement.workspace, temporaryElement.uri, {editMode: true});
    }
}