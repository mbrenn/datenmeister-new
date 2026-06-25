import * as SICInterface from './Interfaces.js'
import * as EL from "../client/Elements.js";
import {ItemWithNameAndId} from "../ApiModels.js";

export class SelectItemControlHelperForWorkspaceAndExtent {
    private control: SICInterface.ISelectItemControl;
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

    /** Last list of workspaces fetched from the server, used by {@link getSelectedWorkspace}. */
    private loadedWorkspaces: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    /** Last list of extents fetched for the active workspace, used by {@link getSelectedExtent}. */
    private loadedExtents: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();

    constructor(control: SICInterface.ISelectItemControl) {
        this.control = control;
    }

    initialize(table: JQuery<HTMLElement>) {
        this.htmlWorkspaceSelect = $("workspace-select", table);
        this.htmlExtentSelect = $("extent-select", table);
    }

    /**
     * Loads the workspaces and adds them into the control element containing the parameter
     * @private
     */
    private async loadWorkspaces() {
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

        for (const n in items) {
            if (!items.hasOwnProperty(n)) continue;

            const item = items[n];
            const option = $("<option></option>");
            option.val(item.id);
            option.text(item.name);

            this.htmlWorkspaceSelect.append(option);
        }

        // Restores the currently selected workspace
        if (currentlySelectedWorkspace !== undefined) {
            this.htmlWorkspaceSelect.val(currentlySelectedWorkspace);
        }

        await this.loadExtents();
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

            for (const n in items) {
                if (!items.hasOwnProperty(n)) continue;

                const item = items[n];
                const option = $("<option></option>");
                option.val(item.extentUri);
                option.text(item.name);

                tthis.htmlExtentSelect.append(option);
            }

            // Restores the selected item
            if (extentUri !== undefined) {
                this.htmlExtentSelect.val(extentUri);
                this.textOfSelectedItem = extentUri;
            }
        }

        return true;
    }

    /**
     * Returns the workspace id currently selected in the dropdown, or the
     * empty string if no workspace is selected. This reflects the *DOM* state
     * — pending pre-selections that have not been applied yet are not visible
     * here.
     */
    getUserSelectedWorkspaceId(): string {
        return this.htmlWorkspaceSelect.val()?.toString() ?? "";
    }

    /**
     * Returns the extent URI currently selected in the dropdown, or the empty
     * string if no extent is selected. Reflects the DOM state only; see
     * {@link getUserSelectedWorkspaceId} for the same caveat.
     */
    getUserSelectedExtentUri(): string {
        const extent = this.htmlExtentSelect.val()?.toString() ?? "";
        return extent === "" ? undefined : extent;
    }
}