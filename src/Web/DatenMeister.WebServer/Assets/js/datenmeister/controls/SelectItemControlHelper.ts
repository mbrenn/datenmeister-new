import * as EL from "../client/Elements.js";
import {ItemWithNameAndId} from "../ApiModels.js";

export interface IWorkspaceAndExtentSelectionCallbacks {
    onWorkspaceSelected (workspaceId: string): Promise<void>;
    onExtentSelected (workspaceId: string, extentUri: string): Promise<void>;
}

export class SelectItemControlHelperForWorkspaceAndExtent {
    private isDomInitialized: boolean = false;
    
    private control: IWorkspaceAndExtentSelectionCallbacks;

    private htmlWorkspaceDiv: JQuery<HTMLElement>;
    private htmlExtentDiv: JQuery<HTMLElement>;
    private htmlWorkspaceSelect: JQuery<HTMLElement>;
    private htmlExtentSelect: JQuery<HTMLElement>;

    /**
     * Workspace id queued for pre-selection. Consumed on the next workspace
     * load and reset to `undefined`. `undefined` means "no pre-selection
     * pending".
     */
    private preSelectWorkspaceById: string | undefined;

    /**
     * Extent URI queued for pre-selection. Consumed on the next extent load
     * and reset to `undefined`. `undefined` means "no pre-selection pending".
     */
    private preSelectExtentByUri: string | undefined;

    private loadedWorkspaces: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    
    private loadedExtents: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();

    constructor(control: IWorkspaceAndExtentSelectionCallbacks) {
        this.control = control;
    }

    initialize(table: JQuery<HTMLElement>) {
        const tthis = this;
        this.htmlWorkspaceDiv = $(".dm-sic-workspace", table);
        this.htmlExtentDiv = $(".dm-sic-extent", table);
        
        // Creates the dropdown for the workspaces
        this.htmlWorkspaceSelect = $("<select></select>");
        this.htmlWorkspaceSelect.on("change", async () => await tthis.onWorkspaceChangedByUser());
        this.htmlWorkspaceDiv.append(this.htmlWorkspaceSelect);
        
        // Creates the dropdown for the extents
        this.htmlExtentSelect = $("<select></select>");
        this.htmlExtentSelect.on("change", () => tthis.onExtentChangedByUser());
        this.htmlExtentDiv.append(this.htmlExtentSelect);
        
        this.isDomInitialized = true;
    }

    async onExtentChangedByUser() {
        await this.control.onExtentSelected(
            this.getUserSelectedWorkspaceId(), this.getUserSelectedExtentUri());
    }

    async onWorkspaceChangedByUser() {
        await this.loadExtents();
        await this.control.onWorkspaceSelected(this.getUserSelectedWorkspaceId());
    }

    /**
     * Loads the workspaces and adds them into the control element containing the parameter
     * @private
     */
    async loadWorkspaces() {
        
        if(this.isDomInitialized !== false) {

            this.htmlWorkspaceSelect.empty();

            let currentlySelectedWorkspace = this.getUserSelectedWorkspaceId();
            if (this.preSelectWorkspaceById !== undefined) {
                // If there is a pre-selection, the pre-selection is valid
                currentlySelectedWorkspace = this.preSelectWorkspaceById;
                this.preSelectWorkspaceById = undefined;
            }

            const items = await EL.getAllWorkspaces();

            this.loadedWorkspaces = items;
            const none = $("<option value=''>--- None ---</option>");
            this.htmlWorkspaceSelect.append(none);

            let found = false;
            for (const n in items) {
                if (!items.hasOwnProperty(n)) continue;

                const item = items[n];
                const option = $("<option></option>");
                option.val(item.id);
                option.text(item.name);
                
                if(item.id === currentlySelectedWorkspace) {
                    found = true;
                }

                this.htmlWorkspaceSelect.append(option);
            }

            // Restores the currently selected workspace
            if (currentlySelectedWorkspace !== undefined && found) {
                this.htmlWorkspaceSelect.val(currentlySelectedWorkspace);
                await this.control.onWorkspaceSelected(currentlySelectedWorkspace);
            }

            await this.loadExtents();
        }
    }
    
    textOfSelectedItem: string;

    /**
     * Loads the extents for the currently selected workspace and (re)populates
     * the extent dropdown. Honors a pending {@link preSelectExtentByUri} value,
     * consuming it in the process. After the dropdown has been refreshed,
     * {@link loadItems} is called so that the child list and breadcrumb stay
     * in sync.
     *
     * @returns A promise that resolves to `true` when the GUI is updated.
     */
    async loadExtents() {

        if(this.isDomInitialized !== false) {

            const tthis = this;

            const workspaceId = this.getUserSelectedWorkspaceId();
            this.textOfSelectedItem = workspaceId;

            let extentUri = this.getUserSelectedExtentUri();
            if (this.preSelectExtentByUri !== undefined) {
                extentUri = this.preSelectExtentByUri;
                this.preSelectExtentByUri = undefined;
            }

            this.htmlExtentSelect.empty();

            if (workspaceId === "") {
                const select = $("<option value=''>--- Select Workspace ---</option>");
                this.htmlExtentSelect.append(select);
            } else {
                const items = await EL.getAllExtents(workspaceId);
                this.htmlExtentSelect.empty();

                const none = $("<option value=''>--- None ---</option>");
                tthis.htmlExtentSelect.append(none);

                tthis.loadedExtents = items;

                let found = false;

                for (const n in items) {
                    if (!items.hasOwnProperty(n)) continue;

                    const item = items[n];
                    const option = $("<option></option>");
                    option.val(item.extentUri);
                    option.text(item.name);
                    if (item.extentUri === extentUri) {
                        found = true;
                    }

                    tthis.htmlExtentSelect.append(option);
                }

                // Restores the selected item
                if (extentUri !== undefined && found) {
                    this.htmlExtentSelect.val(extentUri);
                    this.textOfSelectedItem = extentUri;
                    await this.control.onExtentSelected(workspaceId, extentUri);
                } else {
                    this.htmlExtentSelect.val("");
                }
            }

            return true;
        }
    }

    /**
     * Returns the workspace id currently selected in the dropdown, or the
     * empty string if no workspace is selected. This reflects the *DOM* state
     * — pending pre-selections that have not been applied yet are not visible
     * here.
     */
    getUserSelectedWorkspaceId(): string | undefined {
        if(this.isDomInitialized === false) return undefined;
        return this.htmlWorkspaceSelect.val()?.toString() ?? undefined;
    }

    /**
     * Returns the extent URI currently selected in the dropdown, or the empty
     * string if no extent is selected. Reflects the DOM state only; see
     * {@link getUserSelectedWorkspaceId} for the same issue.
     */
    getUserSelectedExtentUri(): string | undefined {
        if(this.isDomInitialized === false) return undefined;
        
        const extent = this.htmlExtentSelect.val()?.toString() ?? "";
        return extent === "" ? undefined : extent;
    }

    async setExtentByUri(workspaceId: string, extentUri: string): Promise<void> {
        this.preSelectWorkspaceById = workspaceId;
        this.preSelectExtentByUri = extentUri;
        await this.loadWorkspaces();
        await this.loadExtents();
    }

    async setWorkspaceById(workspaceId: string): Promise<void> {
        this.preSelectWorkspaceById = workspaceId;
        this.preSelectExtentByUri = undefined;
        await this.loadWorkspaces();
    }
}