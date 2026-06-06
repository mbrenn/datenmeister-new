import {ItemWithNameAndId} from "../ApiModels.js";
import {UserEvent} from "../../burnsystems/Events.js";
import * as ApiModels from "../ApiModels.js";
import * as GlobalSettings from "../Settings.js";
import {ISelectItemControl} from "./Interfaces.js";
import * as ByBrowseControl from "./SelectItemControlByBrowsingControl.js"
import {ControlSettings} from "./SelectItemControlByBrowsingControl.js";

export class ContainerSettings{
    /**
     * Optional headline rendered in the control's title row. When `undefined`,
     * the default `"Select item:"` text from the template is used.
     */
    headline:string|undefined = undefined;

    /**
     * When `true`, the control is created in a hidden state (`display: none`)
     * and must be made visible by calling {@link SelectItemControlByBrowsingControl.showControl}.
     */
    hideAtStartup = false;
    
    browseSettings: ByBrowseControl.ControlSettings;
    
    constructor() {
        this.browseSettings = new ByBrowseControl.ControlSettings();
    }
}


/**
 * Web control that lets the user pick a single item from the MOF object tree.
 *
 * The control renders four logical rows: a workspace dropdown, an extent
 * dropdown, the currently selected item (with breadcrumb) and a list of
 * navigable children. The user can drill into the hierarchy by clicking list
 * entries, and confirms the choice with the "Set" button.
 *
 * Typical use:
 * ```ts
 * const control = new SelectItemControl();
 * control.itemSelected.addListener(item => { ... });
 * await control.initAsync(parent, settings);
 * ```
 *
 * Pre-selection is supported via {@link setWorkspaceById},
 * {@link setExtentByUri} and {@link setItemByUri}; those calls are safe even
 * before {@link init}/{@link initAsync} have completed.
 *
 * The class implements {@link ISelectItemControl} so that it can be used
 * interchangeably with other selection sub-controls (browse, freetext, …).
 */
export class SelectItemControl implements ISelectItemControl {
    
    /** Root `<table>` of the rendered control; `undefined` after {@link removeControl}. */
    private containerDiv: JQuery;

    /**
     * Stores the settings of the container. 
     * @private
     */
    private settings: ContainerSettings;

    /**
     * Stores the 'byBrowseControl' instance for browsing functionality.
     */
    byBrowseControl: ByBrowseControl.SelectItemControlByBrowsingControl

    /*
     * Public events that callers can subscribe to.
     */

    /**
     * Fired when the user confirms the selection by clicking the "Set" button.
     * The payload is the currently selected item or `undefined` if nothing was
     * selected — listeners must handle the latter case.
     */
    itemSelected: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();

    /**
     * Fired whenever the user clicks an item in the children list. The
     * selection is updated, but no confirmation has been issued yet. Use this
     * for live previews; use {@link itemSelected} to react to a final choice.
     */
    itemClicked: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();
    
    
    
    public init(containerDiv: JQuery, settings?: ContainerSettings) {
        return this.initDom(settings, containerDiv);
    }

    /**
     * Async variant of {@link init}. Renders the control into `parent` and
     * resolves the returned promise once the workspace list (and any
     * pre-selected extent/item) has been loaded and the GUI reflects it.
     *
     * @param parent The container element that should host the control.
     * @param settings Optional behavior overrides; see {@link ControlSettings}.
     * @returns A promise resolving to the root `<table>` of the rendered control.
     */
    async initAsync(parent: JQuery<HTMLElement>, settings?: ContainerSettings): Promise<JQuery> {

        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        const div = this.initDom(settings, parent);
        return div;
    }

    /**
     * This method just creates the DOM and connects the events of the elements to the
     * invocation methods
     * @param settings Settings to be used
     * @param container JQuery-Container Element hosting the content
     * @private
     */
    private initDom(settings: ContainerSettings, container: JQuery<HTMLElement>) {
        this.settings = settings ?? new ContainerSettings();

        this.containerDiv = container;

        // Creates the template
        const div = $(
            "<table class='dm-selectitemcontrol'>" +
            "<tr><th colspan='2' class='dm-selectitemcontrol-headline'>Select item:</th></tr>" +
            "<tr><th colspan='2' class='dm-selectitemcontrol-bybrowse'></th></tr>" +
            "</table>");

        this.containerDiv.append(div);

        // Checks whether we need a headline
        if (this.settings.headline !== undefined) {
            $(".dm-selectitemcontrol-headline", div).text(settings.headline);
        }

        // Adds the by browsing
        const byBrowseDiv = $(".dm-selectitemcontrol-bybrowse", div);
        this.byBrowseControl = new ByBrowseControl.SelectItemControlByBrowsingControl();
        this.byBrowseControl.init(byBrowseDiv, this.settings.browseSettings);

        const tthis = this;
        this.byBrowseControl.itemSelected.addListener(
            (x) => tthis.itemSelected.invoke(x));
        this.byBrowseControl.itemClicked.addListener(
            (x) => tthis.itemClicked.invoke(x));
        this.byBrowseControl.selectionCancelled.addListener(
            () => tthis.removeControl());

        // Checks whether we need to hide the control at startup
        if (settings?.hideAtStartup) {
            this.hideControl();
        }

        return div;
    }
    
    async setWorkspaceById(workspaceId: string) {
        await this.byBrowseControl.setWorkspaceById(workspaceId);
    }
    async setExtentByUri(workspaceId: string, extentUri: string): Promise<void> {
        await this.byBrowseControl.setExtentByUri(workspaceId, extentUri)    
    }
    async setItemByUri(workspaceId: string, itemUri: string): Promise<void> {
        await this.byBrowseControl.setItemByUri(workspaceId, itemUri)    
    }
    getSelectedItem(): ItemWithNameAndId {
        return this.byBrowseControl.getSelectedItem();
    }
    
    getCurrentlySelectedWorkspace()
    {
        return this.byBrowseControl.getUserSelectedWorkspaceId();
    }
    
    showControl(): void {
        if (this.containerDiv !== undefined) {
            this.containerDiv.show();
        }
    }
    
    hideControl(): void {
        if (this.containerDiv !== undefined) {
            this.containerDiv.hide();
        }
    }

    /**
     * Removes the control. This means that 'init(Async)' must be called again
     */
    removeControl() {
        this.containerDiv?.remove();
        this.containerDiv = undefined;
    }
}

/**
 * Allows the selection of a package.
 * Resolves with the selected item's link details or rejects if no valid selection is made.
 *
 * @param {JQuery} changeContainerCell - The container element where the selection control will be initialized.
 * @return {Promise<ApiModels.ItemLink>} A promise that resolves with the selected item link containing its workspace and URI, or rejects if no item is selected.
 */
export function selectPackage(changeContainerCell: JQuery) : Promise<ApiModels.ItemLink> {

    return this.selectItem(changeContainerCell, { workspaceId: GlobalSettings.WorkspaceData, title: "Select Package in which the item shall be created:" });
}

/**
 * Allows the selection of a type
 * Resolves with the selected item's link details or rejects if no valid selection is made.
 *
 * @param {JQuery} changeContainerCell - The container element where the selection control will be initialized.
 * @return {Promise<ApiModels.ItemLink>} A promise that resolves with the selected item link containing its workspace and URI, or rejects if no item is selected.
 */
export function selectType(changeContainerCell: JQuery) : Promise<ApiModels.ItemLink> {

    return this.selectItem(changeContainerCell, { workspaceId: GlobalSettings.WorkspaceTypes, title: "Select type of new item:" });
}

/**
 * Selects an item using a custom selection control and returns the selected item's information.
 *
 * @param changeContainerCell A JQuery object representing the HTML element where the selection control will be initialized.
 * @param parameter The parameters to create the selection control.
 * @return A Promise resolving to an object containing the selected item's workspace and URI, or rejecting if no item is selected.
 */
export function selectItem(changeContainerCell: JQuery<HTMLElement>, parameter: ISelectItemParameter) {
    return new Promise<ApiModels.ItemLink>(async (resolve, reject) => {

        const workspaceId = parameter.workspaceId;
        const title = parameter.title;

        changeContainerCell.empty();
        const selectItem = new SelectItemControl();
        const settings = new ContainerSettings();
        settings.browseSettings.showWorkspaceInBreadcrumb = true;
        settings.browseSettings.showExtentInBreadcrumb = true;
        if(title !== undefined) settings.headline = title;
        await selectItem.setWorkspaceById(workspaceId);
        selectItem.itemSelected.addListener(
            selectedItem => {

                if (selectedItem === undefined ||
                    selectedItem.uri === undefined) {
                    alert("Nothing is selected.");
                    reject("Nothing is selected");
                    return;
                }

                resolve({
                    workspace: selectedItem.workspace,
                    uri: selectedItem.uri
                });
            });

        selectItem.init(changeContainerCell, settings);
    });
}

/**
 * Optional parameters consumed by {@link selectItem} (and its convenience
 * wrappers {@link selectPackage} / {@link selectType}) to configure the
 * one-shot selection dialog they create internally.
 */
interface ISelectItemParameter{
    /**
     * If set, the corresponding workspace is pre-selected and the user can
     * only browse within it. If omitted, the user starts at the workspace
     * picker.
     */
    workspaceId?: string,
    /**
     * Headline rendered in the title row of the selection control. When
     * omitted, the default `"Select item:"` text is used.
     */
    title?: string
}