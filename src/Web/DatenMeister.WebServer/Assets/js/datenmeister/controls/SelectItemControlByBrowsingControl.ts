import {UserEvent} from "../../burnsystems/Events.js";
import {EntentType, ItemWithNameAndId} from "../ApiModels.js";
import {ISelectItemControl} from "./Interfaces.js";
import * as EL from "../client/Elements.js";
import * as ItemsClient from "../client/Items.js";
import {convertItemWithNameAndIdToDom} from "../DomHelper.js";

/**
 * Configurable behavior and appearance for a {@link SelectItemControlByBrowsingControl} instance.
 *
 * All properties have default values that match the historical behavior of the
 * control. Callers may construct a `Settings` instance, override individual
 * flags and pass it to {@link SelectItemControlByBrowsingControl.init} or
 * {@link SelectItemControlByBrowsingControl.initAsync}.
 */
export class ControlSettings {
    
    /**
     * When `true`, the breadcrumb row is rendered above the items list and is
     * kept in sync with the currently navigated item. When `false`, no
     * breadcrumb is rendered at all (the workspace/extent rows are unaffected).
     */
    showBreadcrumb = true;

    /**
     * When `true` (and {@link showBreadcrumb} is also `true`), the breadcrumb
     * starts with a "Workspaces" entry plus the current workspace, so that the
     * user can navigate back to the workspace selection by clicking those
     * crumbs. Default `false` for backwards compatibility.
     */
    showWorkspaceInBreadcrumb = false;

    /**
     * When `true` (and {@link showBreadcrumb} is also `true`), the breadcrumb
     * additionally contains an entry for the currently selected extent.
     * Default `false`.
     */
    showExtentInBreadcrumb = false;
}

export class SelectItemControlByBrowsingControl implements ISelectItemControl {

    /**
     * Becomes `true` after {@link initDom} has run; used to decide whether
     * pre-selection setters should immediately trigger a reload or just queue
     * the value for the first load.
     */
    private isDomInitializationDone: boolean = false;
    
    /** Effective settings; defaults from {@link ControlSettings} when none were passed. */
    private settings: ControlSettings;

    /** Dropdown element listing the available extents of the active workspace. */
    private htmlExtentSelect: JQuery<HTMLElement>;

    /** Dropdown element listing the available workspaces. */
    private htmlWorkspaceSelect: JQuery<HTMLElement>;

    /**
     * `<ul>` rendering the navigable children of the currently selected item.
     * Each `<li>` is clickable and drills one level deeper into the tree.
     */
    private htmlItemsList: JQuery<HTMLElement>;

    /** Container displaying the name (and link DOM) of the selected element. */
    private htmlSelectedElements: JQuery<HTMLElement>;

    /** `<ul class='breadcrumb'>` rendering the path to the selected item. */
    private htmlBreadcrumbList: JQuery<HTMLElement>;

    /** Last list of workspaces fetched from the server, used by {@link getSelectedWorkspace}. */
    private loadedWorkspaces: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    /** Last list of extents fetched for the active workspace, used by {@link getSelectedExtent}. */
    private loadedExtents: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    /** Currently selected item, or `undefined` if nothing is selected. */
    private selectedItem?: ItemWithNameAndId;
    
    itemClicked: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();

    /** Root `<table>` of the rendered control; `undefined` after {@link removeControl}. */
    private containerDiv: JQuery;

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

    /**
     * Item URI queued for pre-selection. Consumed on the next item load and
     * reset to `undefined`. An empty string is a sentinel meaning "select the
     * whole extent"; `undefined` means "no pre-selection pending".
     */
    private preSelectItemUri: string | undefined;

    /**
     * Renders the control into `parent` and kicks off the asynchronous load of
     * the workspace list. Returns synchronously as soon as the DOM is in
     * place; the workspace dropdown is populated in the background.
     *
     * Use {@link initAsync} if you need to await the initial data load.
     *
     * @param parent The container element that should host the control.
     * @param settings Optional behavior overrides; see {@link ControlSettings}.
     * @returns The root `<table>` of the rendered control.
     */
    init(parent: JQuery<HTMLElement>, settings?: ControlSettings): JQuery {

        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        const div = this.initDom(settings, parent);

        // Loads the workspaces
        const _ = this.loadWorkspaces();
        return div;
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
    async initAsync(parent: JQuery<HTMLElement>, settings?: ControlSettings): Promise<JQuery> {

        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        const div = this.initDom(settings, parent);

        // Loads the workspaces
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
    private initDom(settings: ControlSettings, container: JQuery<HTMLElement>) {
        this.settings = settings ?? new ControlSettings();

        const tthis = this;

        // Creates the elements
        this.htmlWorkspaceSelect = $("<select></select>");
        this.htmlExtentSelect = $("<select></select>");
        this.htmlSelectedElements = $("<div></div>");
        this.htmlItemsList = $("<ul></ul>");

        // Defines the handler whenever the user changes something
        this.htmlWorkspaceSelect.on("change", () => tthis.onWorkspaceChangedByUser());
        this.htmlExtentSelect.on("change", () => tthis.onExtentChangedByUser());

        // Creates the template
        const div = $(
            "<table class='dm-sic-bb'>" +
            "<tr><th>Workspace: </th><td class='dm-sic-bb-workspace'></td></tr>" +
            "<tr><th>Extent: </th><td class='dm-sic-bb-extent'></td></tr>" +
            "<tr><th>Selected Item: </th><td><div class='dm-sic-bb-selected'></div></td></tr>" +
            "<tr><th>Children: </th>" +
            "<td><div class='dm-breadcrumb'><nav aria-label='breadcrump'><ul class='breadcrumb'></ul></nav></div>" +
            "<div class='dm-sic-bb-items'></div>" +
            "</td></tr>" +
            "</table>");

        // Adds the elements
        $(".dm-sic-bb-workspace", div).append(this.htmlWorkspaceSelect);
        $(".dm-sic-bb-extent", div).append(this.htmlExtentSelect);
        $(".dm-sic-bb-items", div).append(this.htmlItemsList);
        $(".dm-sic-bb-selected", div).append(this.htmlSelectedElements);

        this.htmlBreadcrumbList = $(".breadcrumb", div);

        container.append(div);
        this.containerDiv = div;
        this.isDomInitializationDone = true;
        return div;
    }

    showControl()
    {
        this.containerDiv.show();
    }

    hideControl()
    {
        this.containerDiv.hide();
    }

    /**
     * This method will be called when the user changed the selected workspace
     * @private
     */
    private async onWorkspaceChangedByUser() {
        // Find the selected workspace
        this.selectedItem = undefined;
        this.htmlExtentSelect.val("");
        this.preSelectExtentByUri = "";
        await this.loadExtents();
    }

    /**
     * This method will be called when the user changed the extent select item
     * @private
     */
    private async onExtentChangedByUser() {
        // Find the selected extent
        this.preSelectItemUri = undefined;
        this.selectedItem =
            {
                workspace: this.getUserSelectedWorkspaceId(),
                extentUri: this.getUserSelectedExtentUri(),
                ententType: EntentType.Extent,
                uri: this.getUserSelectedExtentUri()
            };

        await this.loadItems();
    }

    /**
     * Gets the selected workspace
     */
    getSelectedWorkspace() {
        const currentWorkspace = this.getUserSelectedWorkspaceId();
        if (currentWorkspace !== "" && currentWorkspace !== undefined) {
            for (let n = 0; n < this.loadedWorkspaces.length; n++) {
                const item = this.loadedWorkspaces[n];
                if (item.id === currentWorkspace) {
                    return item;
                }
            }
        }

        return undefined;
    }

    /**
     * Gets the selected extent
     */
    getSelectedExtent() {
        const currentExtent = this.getUserSelectedExtentUri();
        if (currentExtent !== "" && currentExtent != undefined) {
            for (let n = 0; n < this.loadedExtents.length; n++) {
                const item = this.loadedExtents[n];
                if (item.extentUri === currentExtent) {
                    return item;
                }
            }
        }

        return undefined;
    }

    /**
     * Gets the selected item
     */
    getSelectedItem() {
        return this.selectedItem;
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

    /**
     * This call may also be given before the workspaces has been loaded
     * The promise is resolved when the GUI is updated
     *
     * @param workspaceId ID of the workspace which is preselected.
     */
    async setWorkspaceById(workspaceId: string) {
        this.preSelectWorkspaceById = workspaceId;

        if (this.isDomInitializationDone) {
            await this.loadWorkspaces();
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
        tthis.htmlSelectedElements.text(workspaceId);

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
                tthis.htmlSelectedElements.text(extentUri);
            }
        }

        await tthis.loadItems();

        return true;
    }

    /**
     * This call may also be given before the extents has been loaded
     * The promise is resolved when the GUI is updated
     */
    async setExtentByUri(workspaceId: string, extentUri: string) {
        this.preSelectWorkspaceById = workspaceId;
        this.preSelectExtentByUri = extentUri;
        this.selectedItem =
            {
                workspace: workspaceId,
                extentUri: extentUri,
                ententType: EntentType.Extent,
                uri: extentUri
            };

        if (this.isDomInitializationDone) {
            await this.loadWorkspaces();
        }
    }

    /**
     * Pre-selects a specific item by resolving the corresponding workspace and
     * extent from the server, then queueing all three values for the next
     * load. Safe to call before the GUI is initialized.
     *
     * @param workspaceId ID of the workspace containing the item.
     * @param itemUri URI of the item to pre-select.
     * @throws A string error message when no item can be resolved for the given
     *         workspace/URI pair.
     */
    async setItemByUri(workspaceId: string, itemUri: string) {
        const item = await EL.loadNameByUri(workspaceId, itemUri);
        if (item === undefined) {
            throw "Item: " + workspaceId + ":" + itemUri + " has not been found";
        }

        this.preSelectWorkspaceById = workspaceId;
        this.preSelectExtentByUri = item.extentUri;
        this.preSelectItemUri = item.uri;

        if (this.isDomInitializationDone) {
            await this.loadWorkspaces();
        }
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

    /**
     * Refreshes the children list and the "Selected Item" display so that
     * they describe the currently selected item.
     *
     * Behavior:
     * - If a {@link preSelectItemUri} is pending, it is consumed first. An
     *   empty string is interpreted as "select the whole extent" and clears
     *   the selected item; any other value becomes the new selection.
     * - When no valid workspace, extent or item is in scope, a placeholder
     *   `--- Select Extent ---` row is rendered.
     * - When the selection is an `Extent`, the extent's root items are shown.
     *   Otherwise the children of the selected item are shown.
     * - The breadcrumb is refreshed at the end via {@link refreshBreadcrumb}.
     *
     * Each rendered `<li>` becomes clickable; a click updates the selection,
     * fires {@link itemClicked} and recursively reloads the list.
     */
    async loadItems() {
        const tthis = this;
        let selectedItem = this.selectedItem;

        // Checks, whether the user has selected or preselected an item
        if (this.preSelectItemUri !== undefined) {
            if (this.preSelectItemUri === "") {
                // Empty string is used to indicate that the user would like to select the 
                // complete extent
                selectedItem = undefined;
            } else {
                // User has selected a specific item
                selectedItem =
                    {
                        uri: this.preSelectItemUri,
                        workspace: this.getUserSelectedWorkspaceId()
                    };
            }

            this.selectedItem = selectedItem;

            // Now get rid of it
            this.preSelectItemUri = undefined;
        }

        const workspaceId = this.getUserSelectedWorkspaceId();
        const extentUri = this.getUserSelectedExtentUri();

        this.htmlItemsList.empty();
        if (workspaceId === "" || workspaceId === undefined
            || extentUri === "" || extentUri === undefined
            || selectedItem === undefined || selectedItem === null) {
            const select = $("<li>--- Select Extent ---</li>");
            this.htmlItemsList.append(select);
        } else {
            const item = await ItemsClient.getItemWithNameAndId(selectedItem.workspace, selectedItem.uri);

            if (item !== undefined) {
                this.htmlSelectedElements.empty();
                this.htmlSelectedElements.append(convertItemWithNameAndIdToDom(item));
            }

            const funcElements = (items: ItemWithNameAndId[]) => {

                for (const n in items) {
                    if (!items.hasOwnProperty(n)) continue;

                    const item = items[n];
                    const option = $("<li class='dm-sic-bb-item'></li>");
                    option.append(convertItemWithNameAndIdToDom(item, { inhibitItemLink: true, inhibitEditItemLink: true }));

                    // Creates the clickability of the list of items
                    ((innerItem) =>
                        option.on("click", async () => {
                            tthis.selectedItem = innerItem;
                            tthis.itemClicked.invoke(innerItem);
                            await tthis.loadItems();

                            tthis.htmlSelectedElements.empty();
                            tthis.htmlSelectedElements.append(convertItemWithNameAndIdToDom(item));

                            await tthis.refreshBreadcrumb();
                        }))(item);

                    tthis.htmlItemsList.append(option);
                }

                return true;
            };

            if (selectedItem.ententType === EntentType.Extent) {
                tthis.htmlSelectedElements.text(extentUri);
                const rootElements = await EL.getAllRootItems(workspaceId, extentUri);
                funcElements(rootElements);
            } else {
                const childElements = await EL.getAllChildItems(workspaceId, selectedItem.uri);
                funcElements(childElements);
            }
        }

        await tthis.refreshBreadcrumb();
    }

    /**
     * Rebuilds the breadcrumb to reflect the current selection.
     *
     * The breadcrumb is composed in three sections, each gated by a
     * {@link ControlSettings} flag:
     *  1. When {@link ControlSettings.showWorkspaceInBreadcrumb} is set, a
     *     "Workspaces" entry plus the current workspace are prepended.
     *     Clicking them resets the selection back to that level.
     *  2. When {@link ControlSettings.showExtentInBreadcrumb} is set, the active
     *     extent is appended. Clicking it resets the selection to the extent.
     *  3. Finally the container chain of the selected item (queried via
     *     `ItemsClient.getContainer`) is appended in root-to-leaf order. Only
     *     entries of `EntentType.Item` are added — extent/workspace entries
     *     are already represented by the sections above.
     *
     * If {@link ControlSettings.showBreadcrumb} is `false`, the breadcrumb list is
     * cleared and nothing else is rendered.
     */
    async refreshBreadcrumb() {
        const tthis = this;
        const currentWorkspace = this.getUserSelectedWorkspaceId();
        const currentExtent = this.getUserSelectedExtentUri();

        let containerItems: string | any[];
        if (this.selectedItem !== undefined && this.selectedItem.uri !== undefined) {
            containerItems = await ItemsClient.getContainer(currentWorkspace, this.selectedItem.uri, true);
        }

        this.htmlBreadcrumbList.empty();

        if (this.settings.showBreadcrumb) {

            // Starts by showing the button to select the Workspaces
            if (this.settings.showWorkspaceInBreadcrumb) {
                this.addBreadcrumbItem("Workspaces", async () => {
                    this.preSelectWorkspaceById = "";
                    this.preSelectExtentByUri = "";
                    this.selectedItem = undefined;
                    await tthis.loadWorkspaces();
                });

                // Now show the current workspace
                if (currentWorkspace !== "" && currentWorkspace !== undefined) {
                    this.addBreadcrumbItem(
                        currentWorkspace,
                        async () => {
                            this.preSelectExtentByUri = "";
                            this.selectedItem = undefined;
                            await tthis.loadExtents();
                        }
                    );
                }
            }

            // Shows the extent itself in the breadcrumb, if configured
            if (this.settings.showExtentInBreadcrumb) {
                if (currentExtent !== "" && currentExtent !== undefined) {
                    this.addBreadcrumbItem(
                        currentExtent,
                        async () => {
                            this.preSelectExtentByUri = currentExtent;
                            this.selectedItem =
                                {
                                    workspace: this.getUserSelectedWorkspaceId(),
                                    extentUri: this.getUserSelectedExtentUri(),
                                    ententType: EntentType.Extent,
                                    uri: this.getUserSelectedExtentUri()
                                };
                            await tthis.loadExtents();
                        }
                    );
                }
            }

            if (containerItems !== undefined) {

                // Otherwise, just go to the parents
                for (let n = 0; n < containerItems.length; n++) {
                    const item = containerItems[containerItems.length - 1 - n];
                    if (item.ententType !== EntentType.Item) {
                        // The shown item is not of type "Item", this means it is an extent or a workspace
                        // The Extent or Workspace is covered by the source above
                        continue;
                    }

                    ((innerItem) => {
                        this.addBreadcrumbItem(
                            item.name,
                            async () => {
                                this.selectedItem = innerItem;
                                await tthis.loadItems();
                            });
                    })(item);
                }
            }
        }
    }

    /**
     * Appends a single clickable entry to the breadcrumb list.
     *
     * After invoking `onClick`, the breadcrumb is refreshed so that crumbs
     * "below" the clicked one are removed and the rest of the GUI follows
     * the new selection.
     *
     * @param text Label of the breadcrumb entry.
     * @param onClick Handler invoked when the user clicks the entry.
     */
    addBreadcrumbItem(text: string, onClick: () => void): void {
        const tthis = this;
        const breadcrumbItem = $("<li class='breadcrumb-item active'></li>");
        breadcrumbItem.text(text);

        // Remove all breadcrumb items till that one
        breadcrumbItem.on("click", async () => {
            onClick();
            await tthis.refreshBreadcrumb();
        });

        this.htmlBreadcrumbList.append(breadcrumbItem);
    }
}