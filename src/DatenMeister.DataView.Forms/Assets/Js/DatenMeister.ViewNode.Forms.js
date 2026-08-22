import * as FormActions from '/js/datenmeister/FormActions.js';
import { DmObject } from "/js/datenmeister/Mof.js";
import * as ClientElements from "/js/datenmeister/client/Elements.js";
import * as ClientItems from "/js/datenmeister/client/Items.js";
import * as DmModel from '/js/datenmeister/models/DatenMeister.class.js';
export function init() {
    FormActions.addModule(new ViewNodeRenderAction());
}
class ViewNodeRenderAction extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super();
        this.actionName = "DatenMeister.ViewNodes.RenderItemsInPopup";
    }
    async execute(form, element, parameter, submitMethod) {
        if (element === undefined)
            throw new Error("element is null");
        // We have to create the temporary action on the server to navigate to it. 
        const temporaryItemResult = await ClientElements.createTemporaryElement(_Root.__ViewNodeRenderAction_Uri);
        await ClientItems.setPropertyReference(temporaryItemResult.workspace, temporaryItemResult.uri, {
            property: _Root._ViewNodeRenderAction.viewNode,
            referenceUri: element.uri
        });
        // Now open the popup window
        const createWindow = new DmObject(DmModel._Actions._ClientActions.__NavigateOpenActionInWindow_Uri);
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.context, "render");
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.actionUrl, temporaryItemResult.uri);
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.workspaceId, temporaryItemResult.workspace);
        await FormActions.executeClientAction(createWindow);
        return Promise.resolve(undefined);
    }
}
// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.3.0.0
export var _Root;
(function (_Root) {
    class _ViewNodeRenderAction {
    }
    _ViewNodeRenderAction.viewNode = "viewNode";
    _Root._ViewNodeRenderAction = _ViewNodeRenderAction;
    _Root.__ViewNodeRenderAction_Uri = "dm:///types.forms.dataview.datenmeister/#06f21d5d-3813-4810-a63c-16f12ef5d175";
})(_Root || (_Root = {}));
//# sourceMappingURL=DatenMeister.ViewNode.Forms.js.map