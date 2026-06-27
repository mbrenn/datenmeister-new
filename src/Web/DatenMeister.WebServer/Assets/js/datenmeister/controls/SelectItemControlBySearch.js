import { UserEvent } from "../../burnsystems/Events.js";
import * as Mof from "../Mof.js";
import * as QueryEngine from "../modules/QueryEngine.js";
import * as ClientElements from "../client/Elements.js";
import * as DomHelper from "../DomHelper.js";
import { SelectItemControlHelperForWorkspaceAndExtent } from "./SelectItemControlHelper.js";
export class ControlSettings {
    maxItemsPerSearch = 10;
}
export class SelectItemControlBySearch {
    itemClicked = new UserEvent();
    containerDiv;
    inputBoxDiv;
    resultsDiv;
    selectedItem;
    workspaceAndExtent = new SelectItemControlHelperForWorkspaceAndExtent(this);
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
            "<tr><th><span>Workspace:</span></th><td><div class='dm-sic-workspace'></div></td></tr>" +
            "<tr><th><span>Extent:</span></th><td><div class='dm-sic-extent'></div></td></tr>" +
            "<tr><th><span>Search text:</span></th>" +
            "<td><div class='dm-sic-search-input'><input type='text' /></div></td></tr>" +
            "<tr><td colspan='2'><div class='dm-sic-search-results'></div></td></tr>" +
            "</table></div>");
        this.workspaceAndExtent.initialize(div);
        this.inputBoxDiv = $(".dm-sic-search-input input", div);
        this.resultsDiv = $(".dm-sic-search-results", div);
        this.inputBoxDiv.on("input", () => tthis.onInputChanged());
        container.append(div);
        this.containerDiv = div;
        return div;
    }
    getUserSelectedWorkspaceId() {
        return this.workspaceAndExtent.getUserSelectedWorkspaceId();
    }
    getUserSelectedExtentUri() {
        return this.workspaceAndExtent.getUserSelectedExtentUri();
    }
    async onWorkspaceSelected(workspaceId) {
        this.inputBoxDiv.trigger('focus');
        this.inputBoxDiv.val('');
        await this.workspaceAndExtent.setExtentByUri(this.getUserSelectedWorkspaceId(), undefined);
    }
    async onExtentSelected(workspaceId, extentUri) {
        this.inputBoxDiv.trigger('focus');
        this.inputBoxDiv.val('');
    }
    async onWorkspaceChangedByUser() {
        await this.loadExtents();
    }
    onExtentChangedByUser() {
    }
    /**
     * Loads the workspaces and adds them into the control element containing the parameter
     * @private
     */
    async loadWorkspaces() {
        await this.workspaceAndExtent.loadWorkspaces();
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
        await this.workspaceAndExtent.loadExtents();
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
            const extentName = this.getUserSelectedExtentUri();
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
    async setExtentByUri(workspaceId, extentUri) {
        await this.workspaceAndExtent.setExtentByUri(workspaceId, extentUri);
    }
    async setWorkspaceById(workspaceId) {
        await this.workspaceAndExtent.setWorkspaceById(workspaceId);
    }
}
//# sourceMappingURL=SelectItemControlBySearch.js.map