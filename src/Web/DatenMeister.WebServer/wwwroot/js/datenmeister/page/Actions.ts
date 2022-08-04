import * as SIC from '../controls/SelectItemControl'

export async function pageOpenSelectItemControl()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    await control.initAsync($("#selectitemcontrol"));
}


export async function pageOpenSelectItemControlFullBreadcrumb()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    const settings = new SIC.Settings();
    settings.showExtentInBreadcrumb = true;
    settings.showWorkspaceInBreadcrumb = true;
    
    await control.initAsync($("#selectitemcontrol"), settings);
}

export async function pageOpenSelectItemControlWithWorkspace()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    await control.setWorkspaceById("Types");
    await control.initAsync($("#selectitemcontrol"));
}

export async function pageOpenSelectItemControlWithExtent()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    await control.setExtentByUri("Types", "dm:///_internal/types/internal");
    await control.initAsync($("#selectitemcontrol"));
}

export async function pageOpenSelectItemControlWithItem()
{
    $("#selectitemcontrol").empty();
    const control = new SIC.SelectItemControl();
    await control.setItemByUri("Types", "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
    await control.initAsync($("#selectitemcontrol"));
}