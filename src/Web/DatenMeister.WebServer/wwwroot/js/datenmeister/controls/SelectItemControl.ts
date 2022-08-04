import * as EL from '../client/Elements';
import * as ItemsClient from '../client/Items';
import {EntentType, ItemWithNameAndId} from "../ApiModels";
import {UserEvent} from "../../burnsystems/Events";
import {convertItemWithNameAndIdToDom} from "../DomHelper";
import {convertToItemWithNameAndId} from "../Mof";

export class Settings {
    showBreadcrumb = true;
    showWorkspaceInBreadcrumb = false;
    showExtentInBreadcrumb = false;
    showCancelButton = true;
    hideAtStartup = false;
    setButtonText = "Set";
    headline:string|undefined = undefined;
}

export class SelectItemControl {

    // Defines the dropdown element in which the user can select the extent
    private htmlExtentSelect: JQuery<HTMLElement>;

    // Defines the dropdown element in which the user can select the active works
    private htmlWorkspaceSelect: JQuery<HTMLElement>;

    // Defines the element in which the user can select the corresponding list item. 
    // This element is a clickable list
    private htmlItemsList: JQuery<HTMLElement>;

    // Shows the name of the selected elemnt
    private htmlSelectedElements: JQuery<HTMLElement>;

    // Shows the bread crumb list
    private htmlBreadcrumbList: JQuery<HTMLElement>;

    // Defines the settings of the Select Item
    private settings: Settings;

    private loadedWorkspaces: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    private loadedExtents: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    private selectedItem?: ItemWithNameAndId;
    private _containerDiv: JQuery;

    // Defines the id of the workspace, when the workspace shall be pre-selected
    // This value is set to undefined, when no selection shall be given
    private preSelectWorkspaceById: string | undefined;

    // Defines the id of the extent, when the extent shall be pre-selected
    // This value is set to undefined, when no selection shall be given
    private preSelectExtentByUri: string | undefined;

    // Defines the id of the extent, when the extent shall be pre-selected
    // This value is set to undefined, when no selection shall be given
    private preSelectItemUri: string | undefined;

    /* 
     * Thsee events that can be subscribed
     */

    // This method will be called whenever the user has selected an item via the 'Set' button 
    itemSelected: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();

    /**
     * This method will be called whenever the user has selected an item in the dropdown
     */
    itemClicked: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();

    private isDomInitializationDone: boolean = false;

    // Initializes the Select Item Control and adds it to the given parent
    init(parent: JQuery<HTMLElement>, settings?: Settings): JQuery {

        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        const div = this.initDom(settings, parent);

        // Loads the workspaces
        const _ = this.loadWorkspaces();
        return div;
    }

    // Initializes the Select Item Control and adds it to the given parent
    // The promise is resolved as soon as the complete GUI is loaded 
    async initAsync(parent: JQuery<HTMLElement>, settings?: Settings): Promise<JQuery> {

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
    private initDom(settings: Settings, container: JQuery<HTMLElement>) {
        this.settings = settings ?? new Settings();

        const tthis = this;

        // Creates the elements
        this.htmlWorkspaceSelect = $("<select></select>");
        this.htmlExtentSelect = $("<select></select>");
        this.htmlSelectedElements = $("<div></div>");
        this.htmlItemsList = $("<ul></ul>");

        // Defines the handler whenever the user changes something
        this.htmlWorkspaceSelect.on('change', () => tthis.onWorkspaceChangedByUser());
        this.htmlExtentSelect.on('change', () => tthis.onExtentChangedByUser());

        // Creates the template
        const div = $(
            "<table class='dm-selectitemcontrol'>" +
            "<tr><th colspan='2' class='dm-selectitemcontrol-headline'>Select item:</th></tr>" +
            "<tr><td>Workspace: </td><td class='dm-sic-workspace'></td></tr>" +
            "<tr><td>Extent: </td><td class='dm-sic-extent'></td></tr>" +
            "<tr><td>Selected Item: </td><td class='dm-sic-selected'></td></tr>" +
            "<tr><td>Items: </td>" +
            "<td><div class='dm-breadcrumb'><nav aria-label='breadcrump'><ul class='breadcrumb'></ul></nav></div>" +
            "<div class='dm-sic-items'></div>" +
            "</td></tr>" +
            "<tr><td></td><td class='selected'>" +
            (this.settings.showCancelButton ? "<button class='btn btn-secondary dm-sic-cancelbtn' type='button'>Cancel</button>" : "") +
            "<button class='btn btn-primary dm-sic-button' type='button'>Set</button></td></tr>" +
            "</table>");

        const setButton = $(".dm-sic-button", div);
        const cancelButton = $(".dm-sic-cancelbtn", div);

        // Adds the elements
        $(".dm-sic-workspace", div).append(this.htmlWorkspaceSelect);
        $(".dm-sic-extent", div).append(this.htmlExtentSelect);
        $(".dm-sic-items", div).append(this.htmlItemsList);
        $(".dm-sic-selected", div).append(this.htmlSelectedElements);

        // Checks whether we need a headline
        if (this.settings.headline !== undefined) {
            $(".dm-selectitemcontrol-headline", div).text(settings.headline);
        }

        this.htmlBreadcrumbList = $(".breadcrumb", div);

        // throws the event, when the user clicks on the set button
        setButton.text(this.settings.setButtonText);
        setButton.on('click', () => {
            tthis.itemSelected.invoke(tthis.selectedItem);
        });

        cancelButton.on('click', () => {
            this.removeControl();
        });

        if (settings?.hideAtStartup) {
            div.hide();
        }

        container.append(div);
        this._containerDiv = div;
        this.isDomInitializationDone = true;
        return div;
    }

    /**
     * Shows the control (by revoking the hide status)
     */
    showControl() {
        if (this._containerDiv !== undefined) {
            this._containerDiv.show();
        }
    }

    /**
     * Removes the control. This means that 'init(Async)' must be called again
     */
    removeControl() {
        this._containerDiv?.remove();
        this._containerDiv = undefined;
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

    // This call may also be given before the extents has been loaded
    // The promise is resolved when the GUI is updated
    async setExtentByUri(workspaceId: string, extentUri: string) {
        this.preSelectWorkspaceById = workspaceId;
        this.preSelectExtentByUri = extentUri;

        if (this.isDomInitializationDone) {
            await this.loadWorkspaces();
        }
    }
    
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

    // Sets the workspace by given the id
    getUserSelectedWorkspaceId(): string {
        return this.htmlWorkspaceSelect.val()?.toString() ?? "";
    }

    // Sets the extent by given the uri

    getUserSelectedExtentUri(): string {
        return this.htmlExtentSelect.val()?.toString() ?? "";
    }

    async loadItems() {
        const tthis = this;
        let selectedItem = this.selectedItem;

        // Checks, whether the user has selected or preselected an item
        if (this.preSelectItemUri !== undefined) {
            selectedItem =
                {
                    uri: this.preSelectItemUri,
                    workspace: this.getUserSelectedWorkspaceId()
                };
        }

        const workspaceId = this.getUserSelectedWorkspaceId();
        const extentUri = this.getUserSelectedExtentUri();

        this.htmlItemsList.empty();
        if (workspaceId === "" || workspaceId === undefined
            || extentUri === "" || extentUri === undefined
            || selectedItem === undefined) {
            const select = $("<li>--- Select Extent ---</li>");
            this.htmlItemsList.append(select);

            return true;
        } else {
            const item = await ItemsClient.getItemWithNameAndId(selectedItem.workspace, selectedItem.uri);
            
            if ( item !== undefined) {
                this.htmlSelectedElements.empty();
                this.htmlSelectedElements.append(convertItemWithNameAndIdToDom(item));
            }
            
            const funcElements = (items: ItemWithNameAndId[]) => {

                for (const n in items) {
                    if (!items.hasOwnProperty(n)) continue;

                    const item = items[n];
                    const option = $("<li class='dm-sic-item'></li>");
                    option.append(convertItemWithNameAndIdToDom(item, {inhibitItemLink: true}));

                    // Creates the clickability of the list of items
                    ((innerItem) =>
                        option.on('click', async () => {
                            tthis.selectedItem = innerItem;
                            await tthis.loadItems();
                            tthis.itemClicked.invoke(innerItem);

                            tthis.htmlSelectedElements.empty();
                            tthis.htmlSelectedElements.append(convertItemWithNameAndIdToDom(item));

                            await tthis.refreshBreadcrumb();
                        }))(item);

                    tthis.htmlItemsList.append(option);
                }

                return true;
            };

            if (selectedItem.ententType === EntentType.Extent || selectedItem === undefined || selectedItem === null) {
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

    // Refreshes the elements on the bread crumb 
    async refreshBreadcrumb() {
        const tthis = this;
        const currentWorkspace = this.getUserSelectedWorkspaceId();
        const currentExtent = this.getUserSelectedExtentUri();

        let containerItems;
        if (this.selectedItem !== undefined && this.selectedItem.uri !== undefined) {
            containerItems = await ItemsClient.getContainer(currentWorkspace, this.selectedItem.uri, true);
        }

        this.htmlBreadcrumbList.empty();

        if (this.settings.showBreadcrumb) {

            // Starts by showing the button to select to select the Workspaces
            if (this.settings.showWorkspaceInBreadcrumb) {
                this.addBreadcrumbItem("Workspaces", async () => {
                    this.preSelectWorkspaceById = "";
                    this.preSelectExtentByUri = "";
                    this.preSelectItemUri = "";
                    await tthis.loadWorkspaces();
                });

                // Now show the current workspace
                if (currentWorkspace !== "" && currentWorkspace !== undefined) {
                    this.addBreadcrumbItem(
                        currentWorkspace,
                        async () => {
                            this.preSelectExtentByUri = "";
                            this.preSelectItemUri = "";
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
                            this.preSelectItemUri = "";
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
     * Adds a breadcrump item. This is just a helper method for the
     * breadcrumb creation
     * @param text Text to be added
     * @param onClick The event that is meant to be connected to the item
     * @private
     */
    addBreadcrumbItem(text: string, onClick: () => void): void {
        const tthis = this;
        const breadcrumbItem = $("<li class='breadcrumb-item active'></li>");
        breadcrumbItem.text(text);

        // Remove all breadcrumb items till that one
        breadcrumbItem.on('click', async () => {
            onClick();
            await tthis.refreshBreadcrumb();
        });

        this.htmlBreadcrumbList.append(breadcrumbItem);
    }
}