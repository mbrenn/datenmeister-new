import * as ApiModels from "../ApiModels.js";
import {ItemWithNameAndId} from "../ApiModels.js";
import {UserEvent} from "../../burnsystems/Events.js";
import * as GlobalSettings from "../Settings.js";
import {ISelectItemControl} from "./Interfaces.js";
import * as ByBrowseControl from "./SelectItemControlByBrowsingControl.js"
import * as BySearch from "./SelectItemControlBySearch.js"
import {ControlSettings} from "./SelectItemControlByBrowsingControl.js"

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
    
    browseSettings: ByBrowseControl.ControlSettings = new ByBrowseControl.ControlSettings();

    searchSettings: BySearch.ControlSettings = new BySearch.ControlSettings();

    /**
     * When `true`, a "Cancel" button is rendered next to the "Set" button.
     * Clicking it removes the control from the DOM (see
     * {@link SelectItemControlByBrowsingControl.removeControl}). Has no effect when
     * {@link hideButtonRow} is `true`.
     */
    showCancelButton = true;

    /**
     * When `true`, the entire button row (Set/Cancel) is omitted from the DOM.
     * Use this when the host container wants to provide its own confirmation
     * affordances and read the selection via
     * {@link SelectItemControlByBrowsingControl.getSelectedItem}.
     */
    hideButtonRow = false;

    /**
     * Label of the primary confirmation button. Defaults to `"Set"` and can be
     * overridden to fit the surrounding UI (e.g. `"Choose"`, `"Apply"`).
     */
    setButtonText = "Set";
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
    byBrowseControl : ByBrowseControl.SelectItemControlByBrowsingControl 
        = new ByBrowseControl.SelectItemControlByBrowsingControl();

    /**
     * Stores the 'bySearch' instance for browsing functionality.
     */
    bySearchControl : BySearch.SelectItemControlBySearch
        = new BySearch.SelectItemControlBySearch();

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
    
    selectionCancelled: UserEvent<void> = new UserEvent<void>();
    
    currentTab: "byBrowse" | "bySearch" = "byBrowse";
    private byBrowserControlDiv: JQuery<HTMLElement>;
    private bySearchControlDiv: JQuery<HTMLElement>;

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
        return this.initDom(settings, parent);
    }

    /**
     * This method just creates the DOM and connects the events of the elements to the
     * invocation methods
     * @param settings Settings to be used
     * @param container JQuery-Container Element hosting the content
     * @private
     */
    private async initDom(settings: ContainerSettings, container: JQuery<HTMLElement>) {
        this.settings = settings ?? new ContainerSettings();

        this.containerDiv = container;

        // Creates the template
        const div = $(
            "<div class='dm-selectitemcontrol'>" +
            "<div class='dm-selectitemcontrol-headline'>Select item:</div>" +
            "<div class='dm-selectitemcontrol-tabs'>" +
            "<div class='dm-selectitemcontrol-tab-bybrowse'><button>Browse</button></div>" +
            "<div class='dm-selectitemcontrol-tab-search'><button>Search</button></div>" +
            "</div>" +
            "<div class='dm-selectitemcontrol-bybrowse'></div>"  +
            "<div class='dm-selectitemcontrol-search'></div>"  +
            (this.settings.hideButtonRow !== true
                ?
                "<div class='dm-selectitemcontrol-buttons'>" +
                (this.settings.showCancelButton ? "<button class='btn btn-secondary dm-sic-cancelbtn' type='button'>Cancel</button>" : "") +
                "<button class='btn btn-primary dm-sic-button' type='button'>Set</button></div>"
                :
                "") +
            "</div>");

        this.containerDiv.append(div);

        // Sets the button logic
        const setButton = $(".dm-sic-button", div);
        const cancelButton = $(".dm-sic-cancelbtn", div);
        const byBrowseButton = $(".dm-selectitemcontrol-tab-bybrowse button", div);
        const bySearchButton = $(".dm-selectitemcontrol-tab-search button", div);
        this.byBrowserControlDiv = $(".dm-selectitemcontrol-bybrowse", div);
        this.bySearchControlDiv = $(".dm-selectitemcontrol-search", div);
        byBrowseButton.on('click', () => {this.updateTabStatus("byBrowse");});
        bySearchButton.on('click', () => {this.updateTabStatus("bySearch");});

        // throws the event, when the user clicks on the set button
        setButton.text(this.settings.setButtonText);
        setButton.on("click", () => {
            tthis.itemSelected.invoke(this.getSelectedItem());
        });

        cancelButton.on("click", () => {
            this.selectionCancelled.invoke();
        });
        
        this.selectionCancelled.addListener(
            () => tthis.removeControl());                

        // Checks whether we need a headline
        if (this.settings.headline !== undefined) {
            $(".dm-selectitemcontrol-headline", div).text(settings.headline);
        }

        // Adds the by browsing
        const byBrowseDiv = $(".dm-selectitemcontrol-bybrowse", div);
        this.byBrowseControl.init(byBrowseDiv, this.settings.browseSettings);        

        const tthis = this;
        this.byBrowseControl.itemClicked.addListener(
            (x) => tthis.itemClicked.invoke(x));
        this.bySearchControl.itemClicked.addListener(
            (x) => tthis.itemClicked.invoke(x));
                
        // Adds the searching
        const bySearchDiv = $(".dm-selectitemcontrol-search", div);
        this.bySearchControl.init(bySearchDiv, this.settings.searchSettings);
        
        await this.updateTabStatus();

        // Checks whether we need to hide the control at startup
        if (settings?.hideAtStartup) {
            this.hideControl();
        }

        return div;
    }
    
    async updateTabStatus(nextTab? : "byBrowse" | "bySearch") {
        const currentWorkspace= this.getCurrentlySelectedWorkspace();
        if(nextTab !== undefined) {
            this.currentTab = nextTab;
        }
        
        switch (this.currentTab) {
            case "byBrowse":
                this.byBrowserControlDiv.show();
                if(currentWorkspace !== undefined && currentWorkspace !== "" && currentWorkspace !== null) {
                    await this.byBrowseControl.setWorkspaceById(currentWorkspace);
                }
                
                this.bySearchControlDiv.hide();
                break;
            case "bySearch":
                this.byBrowserControlDiv.hide();
                this.bySearchControlDiv.show();
                if(currentWorkspace !== undefined && currentWorkspace !== "" && currentWorkspace !== null) {
                    await this.bySearchControl.setWorkspaceById(currentWorkspace);
                }
                break;
        }
    }
    
    async setWorkspaceById(workspaceId: string) {
        switch(this.currentTab) {
            case "byBrowse":
                await this.byBrowseControl.setWorkspaceById(workspaceId);
                break;
            case "bySearch":
                await this.bySearchControl.setWorkspaceById(workspaceId);
                break;
        }        
    }
    
    async setExtentByUri(workspaceId: string, extentUri: string): Promise<void> {
        switch(this.currentTab) {
            case "byBrowse":
                await this.byBrowseControl.setExtentByUri(workspaceId, extentUri);
                break;
            case "bySearch":
                await this.bySearchControl.setExtentByUri(workspaceId, extentUri);
                break;
        }
    }
    
    async setItemByUri(workspaceId: string, itemUri: string): Promise<void> {
        await this.byBrowseControl.setItemByUri(workspaceId, itemUri)    
    }
    getSelectedItem(): ItemWithNameAndId {
        switch(this.currentTab)
        {
            case "byBrowse":
                return this.byBrowseControl.getSelectedItem();
            case "bySearch":
                return this.bySearchControl.getSelectedItem();
        }
    }
    
    getCurrentlySelectedWorkspace()
    {
        switch(this.currentTab) {
            case "byBrowse":
                return this.byBrowseControl.getUserSelectedWorkspaceId();
            case "bySearch":
                return this.bySearchControl.getUserSelectedWorkspaceId();
        }
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

        await selectItem.initAsync(changeContainerCell, settings);
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