import { UserEvent } from "../../burnsystems/Events.js";
import * as Mof from "../Mof.js";
import * as EL from "../client/Elements.js";
import * as QueryEngine from "../modules/QueryEngine.js";
import * as ClientElements from "../client/Elements.js";
import * as DomHelper from "../DomHelper.js";
export class ControlSettings {
    maxItemsPerSearch = 10;
}
export class SelectItemControlBySearch {
    itemClicked = new UserEvent();
    containerDiv;
    inputBoxDiv;
    resultsDiv;
    htmlWorkspaceSelect;
    htmlExtentSelect;
    selectedItem;
    /** Last list of workspaces fetched from the server, used by {@link getSelectedWorkspace}. */
    loadedWorkspaces = new Array();
    /** Last list of extents fetched for the active workspace, used by {@link getSelectedExtent}. */
    loadedExtents = new Array();
    preSelectWorkspaceById;
    preSelectExtentByUri;
    settings;
    init(parent, settings) {
        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        const div = this.initDom(settings, parent);
        const _ = this.loadWorkspaces();
        return div;
    }
    async initAsync(parent, settings) {
        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        const div = this.initDom(settings, parent);
        await this.loadWorkspaces();
        return div;
    }
    /**
     * This method just creates the DOM and connects the events of the elements to the
     * invocation methods
     * @param settings Settings to be used
     * @param container JQuery-Container Element hosting the content
     * @private
     */
    initDom(settings, container) {
        const tthis = this;
        this.settings = settings ?? new ControlSettings();
        // Creates the template
        const div = $("<div class='dm-sic-search'>" +
            "<table>" +
            "<tr><td><span>Workspace:</span></td><td><div class='dm-sic-search-workspace'></div></td></tr>" +
            "<tr><td><span>Extent:</span></td><td><div class='dm-sic-search-workspace'></div></td></tr>" +
            "<tr><td><span>Search text:</span></td>" +
            "<td><div class='dm-sic-search-input'><input type='text' /></div></td></tr>" +
            "<tr><td colspan='2'><div class='dm-sic-search-results'></div></td></tr>" +
            "</table></div>");
        this.inputBoxDiv = $(".dm-sic-search-input input", div);
        this.resultsDiv = $(".dm-sic-search-results", div);
        // Creates the dropdown for the workspaces
        this.htmlWorkspaceSelect = $("<select></select>");
        this.htmlWorkspaceSelect.on("change", async () => await tthis.onWorkspaceChangedByUser());
        $(".dm-sic-search-workspace", div).append(this.htmlWorkspaceSelect);
        // Creates the dropdown for the extents
        this.htmlExtentSelect = $("<select></select>");
        this.htmlExtentSelect.on("change", () => tthis.onExtentChangedByUser());
        $(".dm-sic-search-extent", div).append(this.htmlExtentSelect);
        this.inputBoxDiv.on("input", () => tthis.onInputChanged());
        container.append(div);
        this.containerDiv = div;
        return div;
    }
    getUserSelectedWorkspaceId() {
        return this.htmlWorkspaceSelect.val()?.toString() ?? "";
    }
    getUserSelectedExtent() {
        return this.htmlExtentSelect.val()?.toString() ?? "";
    }
    async onWorkspaceChangedByUser() {
        this.inputBoxDiv.trigger('focus');
        this.inputBoxDiv.val('');
        this.htmlExtentSelect.val("");
        this.preSelectExtentByUri = "";
        await this.loadExtents();
    }
    onExtentChangedByUser() {
        this.inputBoxDiv.trigger('focus');
        this.inputBoxDiv.val('');
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
    }
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
        let extentUri = this.getUserSelectedExtent();
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
            }
        }
        return true;
    }
    showControl() {
        this.containerDiv.show();
    }
    hideControl() {
        this.containerDiv.hide();
    }
    getSelectedItem() {
        return this.selectedItem;
    }
    setExtentByUri(workspaceId, extentUri) {
        return Promise.resolve(undefined);
    }
    setItemByUri(workspaceId, itemUri) {
        return Promise.resolve(undefined);
    }
    async setWorkspaceById(workspaceId) {
        this.preSelectWorkspaceById = workspaceId;
        await this.loadWorkspaces();
    }
    lastLoadIndex = 0;
    lastSelectedDiv;
    async onInputChanged() {
        const tthis = this;
        this.lastLoadIndex++;
        const loadIndex = this.lastLoadIndex;
        const selectWorkspace = this.getUserSelectedWorkspaceId();
        const text = this.inputBoxDiv.val().toString();
        if (text === "" || text === null || text === undefined) {
            this.resultsDiv.empty();
            this.resultsDiv.hide();
        }
        else if (selectWorkspace === undefined || selectWorkspace === "") {
            this.resultsDiv.empty();
            this.resultsDiv.append("<div>Please select a workspace</div>");
            this.resultsDiv.show();
        }
        else {
            const extentName = this.getUserSelectedExtent();
            // Now load the items with the given freetext name
            const queryBuilder = new QueryEngine.QueryBuilder();
            if (extentName === undefined || extentName === null || extentName === "") {
                QueryEngine.getElementsOfWorkspace(queryBuilder, selectWorkspace);
            }
            else {
                QueryEngine.getElementsOfExtent(queryBuilder, selectWorkspace, extentName);
            }
            QueryEngine.flatten(queryBuilder);
            QueryEngine.filterByFreetext(queryBuilder, text, "name");
            QueryEngine.limit(queryBuilder, this.settings.maxItemsPerSearch + 1);
            const result = await ClientElements.queryObject(queryBuilder.queryStatement);
            if (this.lastLoadIndex == loadIndex) {
                this.resultsDiv.empty();
                let found = 0;
                for (const n in result.result) {
                    if (!result.result.hasOwnProperty(n))
                        continue;
                    // Only 10 items shall be shown. 
                    if (found >= this.settings.maxItemsPerSearch) {
                        const itemDiv = $("<div class='more'><em>... and more ...</em></div>");
                        this.resultsDiv.append(itemDiv);
                        break;
                    }
                    found++;
                    // Create the div for the items themselves
                    const item = result.result[n];
                    const itemDiv = $("<div>" + item.get("name", Mof.ObjectType.String) + "</div>");
                    const _ = DomHelper.injectNameByObject(itemDiv, item, {
                        inhibitItemLink: true
                    });
                    ((innerItem) => itemDiv.on('click', () => {
                        if (this.lastSelectedDiv !== undefined) {
                            tthis.lastSelectedDiv.removeClass('selected');
                        }
                        itemDiv.addClass('selected');
                        tthis.lastSelectedDiv = itemDiv;
                        tthis.selectedItem = innerItem;
                        tthis.itemClicked.invoke(innerItem);
                    }))(item);
                    this.resultsDiv.append(itemDiv);
                }
            }
            this.resultsDiv.show();
        }
    }
}
//# sourceMappingURL=SelectItemControlBySearch.js.map