import * as ApiConnection from "../ApiConnection.js";
import * as Actions from "../client/Actions.js";
import { moveItemInExtentDown, moveItemInExtentUp } from "../client/Actions.Items.js";
import * as ClientExtents from "../client/Extents.js";
import * as ClientForms from "../client/Forms.js";
import * as ClientItems from "../client/Items.js";
import * as FormActions from "../FormActions.js";
import { SubmitMethod } from "../forms/RowForm.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import * as Mof from "../Mof.js";
import * as MofArray from "../MofArray.js";
import * as MofSync from "../MofSync.js";
import * as Navigator from "../Navigator.js";
import * as Settings from "../Settings.js";
var _StoreExtentAction = _DatenMeister._Actions._StoreExtentAction;
var _ObjectForm = _DatenMeister._Forms._ObjectForm;
var _RowForm = _DatenMeister._Forms._RowForm;
var _ActionFieldData = _DatenMeister._Forms._ActionFieldData;
import { createBreadcrumbForItem } from "../controls/ElementBreadcrumb.js";
export function loadModules() {
    FormActions.addModule(new ExtentPropertiesUpdateAction());
    FormActions.addModule(new ExtentCreateItemAction());
    FormActions.addModule(new ExtentClearAction());
    FormActions.addModule(new ExtentDeleteAction());
    FormActions.addModule(new ExtentNavigateToAction());
    FormActions.addModule(new ExtentPropertiesAction());
    FormActions.addModule(new ExtentCreateItemInPropertyAction());
    FormActions.addModule(new ExtentsListViewItemAction());
    FormActions.addModule(new ExtentsListDeleteItemAction());
    FormActions.addModule(new ExtentsListMoveUpItemAction());
    FormActions.addModule(new ExtentsListMoveDownItemAction());
    FormActions.addModule(new ExtentsStoreAction());
    FormActions.addModule(new ExtentXmiExportNavigate());
    FormActions.addModule(new ExtentXmiExport());
    FormActions.addModule(new ExtentXmiImportNavigate());
    FormActions.addModule(new ExtentXmiImport());
}
class ExtentPropertiesUpdateAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.Properties.Update");
    }
    async loadObject() {
        let p = new URLSearchParams(window.location.search);
        const workspace = p.get('workspace');
        const extentUri = p.get('extent');
        return await ClientExtents.getProperties(workspace, extentUri);
    }
    async execute(form, element, parameter, submitMethod) {
        let p = new URLSearchParams(window.location.search);
        if (!p.has("extent") || !p.has("workspace")) {
            alert('There is no extent given');
        }
        else {
            const workspace = p.get('workspace');
            const extentUri = p.get('extent');
            await ClientExtents.setProperties(workspace, extentUri, element);
            Navigator.navigateToExtentItems(workspace, extentUri);
        }
    }
}
class ExtentCreateItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.CreateItem");
        this.actionVerb = "Create Item";
    }
    async loadForm(metaClass) {
        const form = await ClientForms.getObjectFormForMetaClass(metaClass);
        const tabs = form.get(_ObjectForm.tab, Mof.ObjectType.Array);
        const firstTab = tabs[0];
        const fields = firstTab.get(_RowForm.field, Mof.ObjectType.Array);
        const parameter = new Mof.DmObject();
        parameter.set('name', 'CreateItemAndAnotherOne');
        // Adds the additional button 
        const actionButton = new Mof.DmObject(_DatenMeister._Forms.__ActionFieldData_Uri);
        actionButton.set(_ActionFieldData.title, "Create Item and another one");
        actionButton.set(_ActionFieldData.parameter, parameter);
        actionButton.set(_ActionFieldData.actionName, this.actionName);
        fields.push(actionButton);
        return form;
    }
    async execute(form, element, parameter, submitMethod) {
        if (parameter?.get('name', Mof.ObjectType.String) === 'CreateItemAndAnotherOne') {
            submitMethod = SubmitMethod.UserDefined1;
        }
        let p = new URLSearchParams(window.location.search);
        if (!p.has("extent") || !p.has("workspace")) {
            alert('There is no extent given');
        }
        else {
            const workspace = p.get('workspace');
            const extentUri = p.get('extent');
            await this.extentCreateItem(workspace, extentUri, element, undefined, submitMethod);
        }
    }
    async extentCreateItem(workspace, extentUri, element, metaClass, submitMethod) {
        if (metaClass === undefined) {
            metaClass = element.metaClass?.uri;
        }
        const newItem = await ClientItems.createItemInExtent(workspace, extentUri, {
            metaClass: metaClass === undefined ? "" : metaClass,
            properties: element
        });
        if (submitMethod === SubmitMethod.Save) {
            // If user has clicked on the save button (without closing), the form shall just be updated
            Navigator.navigateToItemByUrl(workspace, newItem.itemId);
        }
        if (submitMethod === SubmitMethod.UserDefined1) {
            // Recreate a new item, because user clicked on the userdefined item
            Navigator.navigateToCreateNewItemInExtent(workspace, extentUri, metaClass);
        }
        else {
            // Else, move to the overall items overview
            document.location.href =
                Navigator.getLinkForNavigateToExtentItems(workspace, extentUri);
        }
    }
}
class ExtentCreateItemInPropertyAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.CreateItemInProperty");
        this.actionVerb = "Create Item";
    }
    async preparePage(element, form) {
        let p = new URLSearchParams(window.location.search);
        const workspace = p.get('workspace');
        const itemUrl = p.get('itemUrl');
        await createBreadcrumbForItem($(".dm-breadcrumb-page"), workspace, itemUrl);
    }
    async loadForm(metaClass) {
        const form = await ClientForms.getObjectFormForMetaClass(metaClass);
        const tabs = form.get(_ObjectForm.tab, Mof.ObjectType.Array);
        const firstTab = tabs[0];
        const fields = firstTab.get(_RowForm.field, Mof.ObjectType.Array);
        const parameter = new Mof.DmObject();
        parameter.set('name', 'CreateItemAndAnotherOne');
        // Adds the additional button 
        const actionButton = new Mof.DmObject(_DatenMeister._Forms.__ActionFieldData_Uri);
        actionButton.set(_ActionFieldData.title, "Create Item and another one");
        actionButton.set(_ActionFieldData.parameter, parameter);
        actionButton.set(_ActionFieldData.actionName, this.actionName);
        fields.push(actionButton);
        return form;
    }
    async execute(form, element, parameter, submitMethod) {
        let p = new URLSearchParams(window.location.search);
        if (!p.has("itemUrl") || !p.has("workspace") || !p.has("property")) {
            alert('There is no itemUrl given');
        }
        else {
            if (parameter?.get('name', Mof.ObjectType.String) === 'CreateItemAndAnotherOne') {
                submitMethod = SubmitMethod.UserDefined1;
            }
            const workspace = p.get('workspace');
            const itemUrl = p.get('itemUrl');
            const property = p.get('property');
            const metaclass = p.get('metaclass');
            const metaclassWorkspace = p.get('metaclassworkspace');
            await ClientItems.createItemAsChild(workspace, itemUrl, {
                metaClass: (metaclass === undefined || metaclass === null) ? "" : metaclass,
                property: property,
                asList: true,
                properties: element
            });
            if (submitMethod === SubmitMethod.UserDefined1) {
                // Recreate a new item, because user clicked on the userdefined item
                Navigator.navigateToCreateItemInProperty(workspace, itemUrl, metaclass, metaclassWorkspace, property);
            }
            else {
                // If user has clicked on the save button (without closing), the form shall just be updated            
                Navigator.navigateToItemByUrl(workspace, itemUrl);
            }
        }
    }
}
class ExtentClearAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.Clear");
        this.actionVerb = "Clear Item";
        this.requiresConfirmation = true;
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        let extentUri = element.get('uri');
        let workspaceId = element.get('workspaceId');
        await ClientExtents.clearExtent({
            workspace: workspaceId,
            extentUri: extentUri
        });
        Navigator.navigateToExtentProperties(workspaceId, extentUri);
    }
}
class ExtentDeleteAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.Delete");
        this.actionVerb = "Delete Extent";
        this.requiresConfirmation = true;
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        let extentUri = element.get('uri');
        let workspaceId = element.get('workspaceId');
        const deleteParameter = {
            workspace: workspaceId,
            extentUri: extentUri
        };
        await ApiConnection.deleteRequest(Settings.baseUrl + "api/extent/delete", deleteParameter);
        Navigator.navigateToWorkspace(workspaceId);
    }
}
class ExtentNavigateToAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.NavigateTo");
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        let extentUri = element.get('uri');
        let workspaceId = element.get('workspaceId');
        Navigator.navigateToExtentItems(workspaceId, extentUri);
    }
}
class ExtentPropertiesAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.Properties");
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
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
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        Navigator.navigateToItemByUrl(form.workspace, element.uri);
    }
}
class ExtentsListDeleteItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ExtentsList.DeleteItem");
        this.actionVerb = "Delete Item";
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        const data = await ApiConnection.deleteRequest(Settings.baseUrl + "api/items/delete/"
            + encodeURIComponent(form.workspace) + "/" +
            encodeURIComponent(element.uri), {});
        const success = data.success;
        if (success) {
            document.location.reload();
        }
        else {
            alert('Deletion was not successful.');
        }
    }
}
class ExtentsListMoveUpItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ExtentsList.MoveUpItem");
        this.actionVerb = "Move Up";
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        await moveItemInExtentUp(form.workspace, element.extentUri, element.uri);
        // Now reorder the collection and move the selected item up...
        const asCollectionForm = form;
        if (asCollectionForm.elements !== undefined) {
            MofArray.moveItemInArrayUpByUri(asCollectionForm.elements, form.workspace, element.uri);
            await asCollectionForm.refreshForm();
        }
        else {
            document.location.reload();
        }
    }
}
class ExtentsListMoveDownItemAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("ExtentsList.MoveDownItem");
        this.actionVerb = "Move Down";
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        await moveItemInExtentDown(form.workspace, element.extentUri, element.uri);
        // Now reorder the collection and move the selected item up...
        const asCollectionForm = form;
        if (asCollectionForm.elements !== undefined) {
            MofArray.moveItemInArrayDownByUri(asCollectionForm.elements, form.workspace, element.uri);
            await asCollectionForm.refreshForm();
        }
        else {
            document.location.reload();
        }
    }
}
class ExtentsStoreAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.Store");
        this.actionVerb = "Store Extent";
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        const action = new Mof.DmObject(_DatenMeister._Actions.__StoreExtentAction_Uri);
        action.set(_StoreExtentAction.workspaceId, element.get(_DatenMeister._Management._Extent.workspaceId));
        action.set(_StoreExtentAction.extentUri, element.get(_DatenMeister._Management._Extent.uri));
        const actionParams = {
            parameter: action
        };
        await Actions.executeActionDirectly("Execute", actionParams);
        alert('Extent has been stored.');
    }
}
class ExtentXmiExportNavigate extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.ExportXmi.Navigate");
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        Navigator.navigateToAction("Extent.ExportXmi", "dm:///_internal/forms/internal#DatenMeister.Export.Xmi", {
            workspace: element.get(_DatenMeister._Management._Extent.workspaceId),
            extentUri: element.get(_DatenMeister._Management._Extent.uri)
        });
    }
}
class ExtentXmiExport extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.ExportXmi");
    }
    async loadObject() {
        let p = new URLSearchParams(window.location.search);
        if (!p.has("extentUri") || !p.has("workspace")) {
            alert('There is no workspace and extentUri given');
            throw 'There is no workspace and extentUri given';
        }
        else {
            const workspace = p.get('workspace');
            const extentUri = p.get('extentUri');
            // Export the Xmi and stores it into the element
            const exportedXmi = await ClientExtents.exportXmi(workspace, extentUri);
            const result = await MofSync.createTemporaryDmObject(_DatenMeister._CommonTypes._Default.__XmiExportContainer_Uri);
            result.set(_DatenMeister._CommonTypes._Default._XmiExportContainer.xmi, exportedXmi.xmi);
            return result;
        }
    }
    async execute(form, element, parameter, submitMethod) {
        alert('Nothing to do');
    }
}
class ExtentXmiImportNavigate extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.ImportXmi.Navigate");
        this.skipSaving = true;
    }
    async execute(form, element, parameter, submitMethod) {
        Navigator.navigateToAction("Extent.ImportXmi", "dm:///_internal/forms/internal#DatenMeister.Import.Xmi", {
            workspace: element.get(_DatenMeister._Management._Extent.workspaceId),
            extentUri: element.get(_DatenMeister._Management._Extent.uri)
        });
    }
}
class ExtentXmiImport extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("Extent.ImportXmi");
        this.actionVerb = "Perform Import";
    }
    async execute(form, element, parameter, submitMethod) {
        alert('Now, we do the import');
        let p = new URLSearchParams(window.location.search);
        if (!p.has("extentUri") || !p.has("workspace")) {
            alert('There is no workspace and extentUri given');
            throw 'There is no workspace and extentUri given';
        }
        else {
            const workspace = p.get('workspace');
            const extentUri = p.get('extentUri');
            // Export the Xmi and stores it into the element
            const importedXmi = await ClientExtents.importXmi(workspace, extentUri, element.get(_DatenMeister._CommonTypes._Default._XmiExportContainer.xmi, Mof.ObjectType.String));
            if (importedXmi.success) {
                Navigator.navigateToExtentItems(workspace, extentUri);
            }
            else {
                alert('Something failed');
            }
        }
    }
}
//# sourceMappingURL=ExtentModules.js.map