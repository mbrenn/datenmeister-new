import {UserEvent} from "../../burnsystems/Events.js";
import {EntentType, ItemWithNameAndId} from "../ApiModels.js";
import * as EL from "../client/Elements.js";
import * as ItemsClient from "../client/Items.js";
import {convertItemWithNameAndIdToDom} from "../DomHelper.js";
import {
    IWorkspaceAndExtentSelectionCallbacks,
    SelectItemControlHelperForWorkspaceAndExtent
} from "./SelectItemControlHelper.js"


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

export class SelectItemControlByBrowsingControl implements IWorkspaceAndExtentSelectionCallbacks{


    /**
     * Becomes `true` after {@link initDom} has run; used to decide whether
     * pre-selection setters should immediately trigger a reload or just queue
     * the value for the first load.
     */
    private isDomInitialized: boolean = false;
    
    /** Effective settings; defaults from {@link ControlSettings} when none were passed. */
    private settings: ControlSettings;

    /**
     * `<ul>` rendering the navigable children of the currently selected item.
     * Each `<li>` is clickable and drills one level deeper into the tree.
     */
    private htmlItemsList: JQuery<HTMLElement>;

    /** Container displaying the name (and link DOM) of the selected element. */
    private htmlSelectedElements: JQuery<HTMLElement>;

    /** `<ul class='breadcrumb'>` rendering the path to the selected item. */
    private htmlBreadcrumbList: JQuery<HTMLElement>;
    
    /** Currently selected item, or `undefined` if nothing is selected. */
    private selectedItem?: ItemWithNameAndId;
    
    itemClicked: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();

    /** Root `<table>` of the rendered control; `undefined` after {@link removeControl}. */
    private containerDiv: JQuery;
    
    private workspaceAndExtent: SelectItemControlHelperForWorkspaceAndExtent 
        = new SelectItemControlHelperForWorkspaceAndExtent(this);

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
        const result = this.initDom(settings ?? new ControlSettings(), parent);
        const _ = this.workspaceAndExtent.loadWorkspaces();
        return result;
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
        const result = this.initDom(settings ?? new ControlSettings(), parent);
        await this.workspaceAndExtent.loadWorkspaces();
        return result;
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
        
        // Creates the elements
        this.htmlSelectedElements = $("<div></div>");
        this.htmlItemsList = $("<ul></ul>");

        // Creates the template
        const div = $(
            "<table class='dm-sic-bb'>" +
            "<tr><th>Workspace: </th><td class='dm-sic-workspace'></td></tr>" +
            "<tr><th>Extent: </th><td class='dm-sic-extent'></td></tr>" +
            "<tr><th>Selected Item: </th><td><div class='dm-sic-bb-selected'></div></td></tr>" +
            "<tr><th>Children: </th>" +
            "<td><div class='dm-breadcrumb'><nav aria-label='breadcrumb'><ul class='breadcrumb'></ul></nav></div>" +
            "<div class='dm-sic-bb-items'></div>" +
            "</td></tr>" +
            "</table>");
        
        this.workspaceAndExtent.initialize(div);

        // Adds the elements
        $(".dm-sic-bb-items", div).append(this.htmlItemsList);
        $(".dm-sic-bb-selected", div).append(this.htmlSelectedElements);

        this.htmlBreadcrumbList = $(".breadcrumb", div);

        container.append(div);
        this.containerDiv = div;
        this.isDomInitialized = true;
        this.renderEmptySelectedItem();
        
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
    
    async onWorkspaceSelected(workspaceId: string): Promise<void> {
        this.selectedItem = undefined;
        this.renderEmptySelectedItem();
    }
    
    async onExtentSelected(workspaceId: string, extentUri: string): Promise<void> {
        this.selectedItem =
            {
                workspace: workspaceId,
                extentUri: extentUri,
                ententType: EntentType.Extent,
                uri: extentUri
            };
        
        await this.loadItems();
    }

    /**
     * Gets the selected item
     */
    getSelectedItem() {
        return this.selectedItem;
    }

    /**
     * This call may also be given before the workspaces has been loaded
     * The promise is resolved when the GUI is updated
     *
     * @param workspaceId ID of the workspace which is preselected.
     */
    async setWorkspaceById(workspaceId: string) {
        await this.workspaceAndExtent.setWorkspaceById(workspaceId);
        
        this.selectedItem = undefined;
        this.renderEmptySelectedItem();
    }

    /**
     * This call may also be given before the extents has been loaded
     * The promise is resolved when the GUI is updated
     */
    async setExtentByUri(workspaceId: string, extentUri: string) {
        this.selectedItem =
            {
                workspace: workspaceId,
                extentUri: extentUri,
                ententType: EntentType.Extent,
                uri: extentUri
            };
        
        await this.workspaceAndExtent.setExtentByUri(workspaceId, extentUri);
        await this.loadItems();
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

        await this.workspaceAndExtent.setExtentByUri(workspaceId, item.extentUri);
        this.preSelectItemUri = item.uri;

        if (this.isDomInitialized) {
            await this.loadItems();
        }
    }

    /**
     * Returns the workspace id currently selected in the dropdown, or the
     * empty string if no workspace is selected. This reflects the *DOM* state
     * — pending pre-selections that have not been applied yet are not visible
     * here.
     */
    getUserSelectedWorkspaceId(): string | undefined {
        return this.workspaceAndExtent.getUserSelectedWorkspaceId();
    }

    /**
     * Returns the extent URI currently selected in the dropdown, or the empty
     * string if no extent is selected. Reflects the DOM state only; see
     * {@link getUserSelectedWorkspaceId} for the same caveat.
     */
    getUserSelectedExtentUri(): string | undefined {
        return this.workspaceAndExtent.getUserSelectedExtentUri();
    }

    /**
     * Refreshes the children list and the "Selected Item" display so that
     * they describe the currently selected item.
     * 
     * Each rendered `<li>` becomes clickable; a click updates the selection,
     * fires {@link itemClicked} and recursively reloads the list.
     */
    async loadItems() {
        if (this.isDomInitialized) {
            const tthis = this;
            let selectedItem = this.selectedItem;

            // Check, whether the user has selected or preselected an item
            if (this.preSelectItemUri !== undefined) {
                const selectedWorkspace = this.getUserSelectedWorkspaceId();
                if (this.preSelectItemUri === "" || selectedWorkspace === undefined) {
                    // Empty string is used to indicate that the user would like to select the 
                    // complete extent
                    selectedItem = undefined;
                } else {
                    // User has selected a specific item
                    selectedItem =
                        {
                            uri: this.preSelectItemUri,
                            workspace: selectedWorkspace
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
                || selectedItem === undefined) {
                this.renderEmptySelectedItem();

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
                        option.append(convertItemWithNameAndIdToDom(item, {
                            inhibitItemLink: true,
                            inhibitEditItemLink: true
                        }));

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
    }

    
    /**
     * Renders an empty selected item in the UI by appending default elements
     * to the appropriate DOM containers.
     *
     * This method first checks if the DOM is initialized. If true, it adds a
     * placeholder list item to the htmlItemsList and resets the htmlSelectedElements
     * container with a default "None" element.
     *
     * @return {void} This method does not return a value.
     */
    private renderEmptySelectedItem(): void {
        if(this.isDomInitialized === true) {
            const select = $("<li>--- Select Extent ---</li>");
            this.htmlItemsList.empty();
            this.htmlItemsList.append(select);

            this.htmlSelectedElements.empty();
            this.htmlSelectedElements.append($("<em>None</em>"));
        }
    }

    /**
     * Rebuilds the breadcrumb to reflect the current selection.
     */
    async refreshBreadcrumb() {
        const tthis = this;
        const currentWorkspace = this.getUserSelectedWorkspaceId();
        const currentExtent = this.getUserSelectedExtentUri();

        let containerItems: ItemWithNameAndId[] = [];
        if (this.selectedItem !== undefined && this.selectedItem.uri !== undefined
            && currentWorkspace !== undefined) {
            containerItems = await ItemsClient.getContainer(currentWorkspace, this.selectedItem.uri, true);
        }

        this.htmlBreadcrumbList.empty();

        if (this.settings.showBreadcrumb) {

            if(currentWorkspace === undefined) {
                this.addBreadcrumbItem("No Workspace selected", async () => {});
                return;
            }
            
            // Starts by showing the button to select the Workspaces
            if (this.settings.showWorkspaceInBreadcrumb) {
                this.addBreadcrumbItem("Workspaces", async () => {
                    await this.workspaceAndExtent.setWorkspaceById(undefined);
                    this.selectedItem = undefined;
                });

                // Now show the current workspace
                if (currentWorkspace !== "" && currentWorkspace !== undefined) {
                    this.addBreadcrumbItem(
                        currentWorkspace,
                        async () => {
                            await this.workspaceAndExtent.setWorkspaceById(currentWorkspace);
                            this.selectedItem = undefined;
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
                            await this.workspaceAndExtent.setExtentByUri(this.getUserSelectedExtentUri(), currentExtent);
                            this.selectedItem =
                                {
                                    workspace: currentWorkspace,
                                    extentUri: currentExtent,
                                    ententType: EntentType.Extent,
                                    uri: currentExtent
                                };
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
                            item.name ?? "Unknown Name",
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