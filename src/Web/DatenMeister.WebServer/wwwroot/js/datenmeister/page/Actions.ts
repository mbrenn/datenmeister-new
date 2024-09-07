import * as SIC from '../controls/SelectItemControl.js'
import * as Mof from '../Mof.js'
import * as _DatenMeister from '../models/DatenMeister.class.js'
import * as FormActions from '../FormActions.js'
import { loadDefaultModules } from "../modules/DefaultLoader.js"

loadDefaultModules();

export async function pageOpenSelectItemControl()
{
    const selectItemControl =$("#selectitemcontrol"); 
    selectItemControl.empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x => 
        alert ( 
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    await control.initAsync(selectItemControl);
}
export async function pageOpenSelectItemControlFullBreadcrumb()
{
    const selectItemControl =$("#selectitemcontrol");
    selectItemControl.empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x =>
        alert (
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    
    const settings = new SIC.Settings();
    settings.showExtentInBreadcrumb = true;
    settings.showWorkspaceInBreadcrumb = true;
    
    await control.initAsync(selectItemControl, settings);
}

export async function pageOpenSelectItemControlWithWorkspace()
{
    const selectItemControl =$("#selectitemcontrol");
    selectItemControl.empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x =>
        alert (
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    
    await control.setWorkspaceById("Types");
    await control.initAsync(selectItemControl);
}

export async function pageOpenSelectItemControlWithExtent()
{
    const selectItemControl =$("#selectitemcontrol");
    selectItemControl.empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x =>
        alert (
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    
    await control.setExtentByUri("Types", "dm:///_internal/types/internal");
    await control.initAsync(selectItemControl);
}

export async function pageOpenSelectItemControlWithItem()
{
    const selectItemControl = $("#selectitemcontrol");
    selectItemControl.empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x =>
        alert (
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    
    await control.setItemByUri("Types", "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
    await control.initAsync(selectItemControl);
}

export async function openNavigateToExtentPropertiesViaClientNavigation() {
    const navigate = new Mof.DmObject();
    navigate.setMetaClassByUri(_DatenMeister._DatenMeister._Actions._ClientActions.__NavigateToExtentClientAction_Uri);
    navigate.set(_DatenMeister._DatenMeister._Actions._ClientActions._NavigateToExtentClientAction.workspaceId, "Data");
    navigate.set(_DatenMeister._DatenMeister._Actions._ClientActions._NavigateToExtentClientAction.extentUri, "dm:///__internal/temp");

    FormActions.executeClientAction(navigate);
}