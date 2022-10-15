import * as SIC from '../controls/SelectItemControl'

export async function pageOpenSelectItemControl()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x => 
        alert ( 
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    await control.initAsync($("#selectitemcontrol"));
}


export async function pageOpenSelectItemControlFullBreadcrumb()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x =>
        alert (
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    
    const settings = new SIC.Settings();
    settings.showExtentInBreadcrumb = true;
    settings.showWorkspaceInBreadcrumb = true;
    
    await control.initAsync($("#selectitemcontrol"), settings);
}

export async function pageOpenSelectItemControlWithWorkspace()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x =>
        alert (
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    
    await control.setWorkspaceById("Types");
    await control.initAsync($("#selectitemcontrol"));
}

export async function pageOpenSelectItemControlWithExtent()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x =>
        alert (
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    
    await control.setExtentByUri("Types", "dm:///_internal/types/internal");
    await control.initAsync($("#selectitemcontrol"));
}

export async function pageOpenSelectItemControlWithItem()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    control.itemSelected.addListener(x =>
        alert (
            "Uri:" + x.uri+ ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
    
    await control.setItemByUri("Types", "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
    await control.initAsync($("#selectitemcontrol"));
}