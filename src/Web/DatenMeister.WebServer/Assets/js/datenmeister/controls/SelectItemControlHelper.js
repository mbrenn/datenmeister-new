import * as EL from "../client/Elements.js";
export class SelectItemControlHelperForWorkspaceAndExtent {
    control;
    htmlWorkspaceDiv;
    htmlExtentDiv;
    htmlWorkspaceSelect;
    htmlExtentSelect;
    /**
     * Workspace id queued for pre-selection. Consumed on the next workspace
     * load and reset to `undefined`. `undefined` means "no pre-selection
     * pending".
     */
    preSelectWorkspaceById;
    /**
     * Extent URI queued for pre-selection. Consumed on the next extent load
     * and reset to `undefined`. `undefined` means "no pre-selection pending".
     */
    preSelectExtentByUri;
    /** Last list of workspaces fetched from the server, used by {@link getSelectedWorkspace}. */
    loadedWorkspaces = new Array();
    /** Last list of extents fetched for the active workspace, used by {@link getSelectedExtent}. */
    loadedExtents = new Array();
    constructor(control) {
        this.control = control;
    }
    initialize(table) {
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
    }
    async onExtentChangedByUser() {
        await this.control.onExtentSelected(this.getUserSelectedWorkspaceId(), this.getUserSelectedExtentUri());
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
            if (!items.hasOwnProperty(n))
                continue;
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
    textOfSelectedItem;
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
        }
        else {
            const items = await EL.getAllExtents(workspaceId);
            this.htmlExtentSelect.empty();
            const none = $("<option value=''>--- None ---</option>");
            tthis.htmlExtentSelect.append(none);
            tthis.loadedExtents = items;
            for (const n in items) {
                if (!items.hasOwnProperty(n))
                    continue;
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
    getUserSelectedWorkspaceId() {
        return this.htmlWorkspaceSelect.val()?.toString() ?? "";
    }
    /**
     * Returns the extent URI currently selected in the dropdown, or the empty
     * string if no extent is selected. Reflects the DOM state only; see
     * {@link getUserSelectedWorkspaceId} for the same caveat.
     */
    getUserSelectedExtentUri() {
        const extent = this.htmlExtentSelect.val()?.toString() ?? "";
        return extent === "" ? undefined : extent;
    }
    async setExtentByUri(workspaceId, extentUri) {
        this.preSelectWorkspaceById = workspaceId;
        this.preSelectExtentByUri = extentUri;
        await this.loadWorkspaces();
        await this.loadExtents();
    }
    async setWorkspaceById(workspaceId) {
        this.preSelectWorkspaceById = workspaceId;
        await this.loadWorkspaces();
    }
}
//# sourceMappingURL=SelectItemControlHelper.js.map