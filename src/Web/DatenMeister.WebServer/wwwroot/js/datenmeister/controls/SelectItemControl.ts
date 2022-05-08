import * as EL from '../client/Elements';
import {ItemWithNameAndId} from "../ApiModels";
import {UserEvent} from "../../burnsystems/Events";
import {convertItemWithNameAndIdToDom} from "../DomHelper";

export class Settings {
    showBreadcrumb = true;
    showWorkspaceInBreadcrumb = false;
    showExtentInBreadcrumb = false;
    showCancelButton = true;
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

    private visitedItems: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    private loadedWorkspaces: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    private loadedExtents: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    private selectedWorkspace?: ItemWithNameAndId;
    private selectedExtent?: ItemWithNameAndId;
    private selectedItem?: ItemWithNameAndId;
    private _containerDiv: JQuery;

    // Defines the id of the workspace, when the workspace shall be pre-selected
    // This value is set to undefined, when no selection shall be given
    private preSelectWorkspaceById: string | undefined;

    // Defines the id of the extent, when the extent shall be pre-selected
    // This value is set to undefined, when no selection shall be given
    private preSelectExtentByUri: string | undefined;

    /* 
     * The events that can be subscribed
     */
    
    // This method will be called whenever the user has selected an item via the 'Set' button 
    itemSelected: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();
    
    // This method will be called whenever the user has selected an item in the dropdown
    itemClicked: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();
    
    // This method will be called when the workspace DOM elements have been updated
    domWorkspaceUpdated: UserEvent<void> = new UserEvent<void>();
    
    // This method will be called when the extents DOM elements have been updated
    domExtentUpdated: UserEvent<void> = new UserEvent<void>();
    
    // This method will be called when the domItems are updated
    domItemsUpdated: UserEvent<void> = new UserEvent<void>();
    
    private isDomInitializationDone: boolean = false;    

    // Initializes the Select Item Control and adds it to the given parent
    init(parent: JQuery<HTMLElement>, settings?: Settings): JQuery {
        const div = this.initDom(settings, parent);
        this.loadWorkspaces();
        return div;
    }

    // Initializes the Select Item Control and adds it to the given parent
    // The promise is resolved as soon as the complete GUI is loaded 
    async initAsync(parent: JQuery<HTMLElement>, settings?: Settings): Promise<JQuery> {
        const div = this.initDom(settings, parent);
        await this.loadWorkspaces();
        return div;
    }

    private initDom(settings: Settings, parent: JQuery<HTMLElement>) {
        this.settings = settings ?? new Settings();

        const tthis = this;
        this.htmlWorkspaceSelect = $("<select></select>");
        this.htmlExtentSelect = $("<select></select>");
        this.htmlSelectedElements = $("<div></div>");
        this.htmlItemsList = $("<ul></ul>");

        // Defines the handler whenever the user changes something
        this.htmlWorkspaceSelect.on('change', () => tthis.onWorkspaceChangedByUser());

        this.htmlExtentSelect.on('change', () => tthis.onExtentChangedByUser());

        const div = $(
            "<table class='dm-selectitemcontrol'>" +
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

        $(".dm-sic-workspace", div).append(this.htmlWorkspaceSelect);
        $(".dm-sic-extent", div).append(this.htmlExtentSelect);
        $(".dm-sic-items", div).append(this.htmlItemsList);
        $(".dm-sic-selected", div).append(this.htmlSelectedElements);
        this.htmlBreadcrumbList = $(".breadcrumb", div);
        const setButton = $(".dm-sic-button", div);
        setButton.on('click', () => {
            tthis.itemSelected.invoke(tthis.selectedItem);
        });

        const cancelButton = $(".dm-sic-cancelbtn", div);
        cancelButton.on('click', () => {
            this.collapse();
        });

        parent.append(div);
        this._containerDiv = div;
        this.isDomInitializationDone = true;
        return div;
    }

    // This method will be called when the user changed the workspace
    private async onWorkspaceChangedByUser()
    {
        // Find the selected workspace
        this.selectedWorkspace = undefined;
        const currentWorkspace = this.getUserSelectedWorkspace();
        if (currentWorkspace != "" && currentWorkspace != undefined) {
            for (let n = 0; n < this.loadedWorkspaces.length; n++) {
                const item = this.loadedWorkspaces[n];
                if (item.id == currentWorkspace) {
                    this.selectedWorkspace = item;
                    break;
                }
            }
        }

        await this.loadExtents();
    }
    
    // This method will be called when the user changed the extent select item
    private async onExtentChangedByUser()
    {
        // Find the selected extent
        this.selectedExtent = undefined;
        const currentExtent = this.getUserSelectedExtent();
        if (currentExtent != "" && currentExtent != undefined) {
            for (let n = 0; n < this.loadedExtents.length; n++) {
                const item = this.loadedExtents[n];
                if (item.extentUri == currentExtent) {
                    this.selectedExtent = item;
                    break;
                }
            }
        }

        await this.loadItems();
    }

    collapse() {
        this._containerDiv?.remove();
        this._containerDiv = undefined;
    }

    async loadWorkspaces() {
        const tthis = this;
        tthis.htmlWorkspaceSelect.empty();

        const items = await EL.getAllWorkspaces();

        tthis.visitedItems.length = 0;
        tthis.loadedWorkspaces = items;
        const none = $("<option value=''>--- None ---</option>");
        tthis.htmlWorkspaceSelect.append(none);

        for (const n in items) {
            if (!items.hasOwnProperty(n)) continue;

            const item = items[n];
            const option = $("<option></option>");
            option.val(item.id);
            option.text(item.name);

            tthis.htmlWorkspaceSelect.append(option);
        }

        tthis.refreshBreadcrumb();

        const p1 = tthis.evaluatePreSelectedWorkspace();
        const p2 = this.loadExtents();

        // Calls the DOM workspace updated event
        Promise.all([p1]).then(() => {
            tthis.domWorkspaceUpdated.invoke()
        });

        await Promise.all([p1, p2]);

        return true;
    }

    // This call may also be given before the workspaces has been loaded
    // The promise is resolved when the GUI is updated 
    async setWorkspaceById(workspaceId: string) {
        this.preSelectWorkspaceById = workspaceId;
        await this.evaluatePreSelectedWorkspace();
    }

    // This call may also be given before the extents has been loaded
    // The promise is resolved when the GUI is updated
    async setExtentByUri(extentUri: string) {
        this.preSelectExtentByUri = extentUri;
        await this.evaluatePreSelectedExtent();
    }

    // Sets the workspace by given the id
    getUserSelectedWorkspace(): string {
        return this.htmlWorkspaceSelect.val()?.toString() ?? "";
    }

    // Sets the extent by given the uri

    getUserSelectedExtent(): string {
        return this.htmlExtentSelect.val()?.toString() ?? "";
    }

    async loadExtents() {
        const tthis = this;

        const workspaceId = this.getUserSelectedWorkspace();
        tthis.htmlSelectedElements.text(workspaceId);
        this.htmlExtentSelect.empty();

        if (workspaceId == "") {
            const select = $("<option value=''>--- Select Workspace ---</option>");
            this.htmlExtentSelect.append(select);
        } else {
            const items = await EL.getAllExtents(workspaceId);

            tthis.visitedItems.length = 0;

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

            tthis.evaluatePreSelectedExtent();
        }

        tthis.refreshBreadcrumb();
        await this.loadItems();
        
        this.domExtentUpdated.invoke();

        return true;
    }

    async loadItems(selectedItem?: string) {

        const tthis = this;

        const workspaceId = this.getUserSelectedWorkspace();
        const extentUri = this.getUserSelectedExtent();
        this.htmlItemsList.empty();

        if (workspaceId === "" || extentUri === "") {
            const select = $("<li>--- Select Extent ---</li>");
            this.htmlItemsList.append(select);
            this.visitedItems.length = 0;

            if (extentUri !== "" && extentUri !== undefined && extentUri !== null) {
                this.visitedItems.push(
                    {
                        id: extentUri,
                        name: extentUri,
                        fullName: extentUri,
                        extentUri: extentUri
                    }
                );
            }

            return true;

        } else {
            const funcElements = (items: ItemWithNameAndId[]) => {

                for (const n in items) {
                    if (!items.hasOwnProperty(n)) continue;

                    const item = items[n];
                    const option = $("<li class='dm-sic-item'></li>");
                    option.append(convertItemWithNameAndIdToDom(item, {inhibitItemLink: true}));
                    
                    // Creates the clickability of the list of items
                    ((innerItem) =>
                        option.on('click', () => {
                            tthis.selectedItem = innerItem;
                            tthis.loadItems(innerItem.id);
                            tthis.itemClicked.invoke(innerItem);
                            
                            tthis.htmlSelectedElements.empty();
                            tthis.htmlSelectedElements.append(convertItemWithNameAndIdToDom(item));
                            
                            tthis.visitedItems.push(item);
                            tthis.refreshBreadcrumb();
                        }))(item);

                    tthis.htmlItemsList.append(option);
                }

                return true;
            };

            if (selectedItem === undefined || selectedItem === null) {
                tthis.htmlSelectedElements.text(extentUri);
                const rootElements = await EL.getAllRootItems(workspaceId, extentUri);
                funcElements(rootElements);
            } else {
                const childElements = await EL.getAllChildItems(workspaceId, extentUri, selectedItem);
                funcElements(childElements);
            }
        }

        tthis.refreshBreadcrumb();

        this.domItemsUpdated.invoke();
    }

    // Refreshes the elements on the bread crumb 
    refreshBreadcrumb() {
        const tthis = this;
        this.htmlBreadcrumbList.empty();

        if (this.settings.showBreadcrumb) {
            const currentWorkspace = this.getUserSelectedWorkspace();
            const currentExtent = this.getUserSelectedExtent();

            // Starts by showing the button to select to select the Workspaces
            if (this.settings.showWorkspaceInBreadcrumb) {
                this.addBreadcrumbItem("Workspaces", () => tthis.loadWorkspaces());
                
                // Now show the current workspace
                if (currentWorkspace !== "" && currentWorkspace !== undefined) {
                    this.addBreadcrumbItem(
                        currentWorkspace,
                        () => {
                            tthis.loadExtents();
                            tthis.visitedItems.length = 0;
                        }
                    );
                }
            }

            // Shows the extent itself in the breadcrumb, if configured
            if (this.settings.showExtentInBreadcrumb) {
                if (currentExtent !== "" && currentExtent !== undefined) {
                    this.addBreadcrumbItem(
                        currentExtent,
                        () => {
                            tthis.loadItems();
                            tthis.visitedItems.length = 0;
                        }
                    );
                }
            }

            // Otherwise, just go to the parents
            for (let n = 0; n < this.visitedItems.length; n++) {
                const item = this.visitedItems[n];

                this.addBreadcrumbItem(
                    item.name,
                    () => {
                        tthis.loadItems(item.id);
                        tthis.visitedItems = tthis.visitedItems.slice(0, n + 1);
                    });
            }
        }
    }

    // Evaluates the preselection of the workspaces
    private async evaluatePreSelectedWorkspace() {

        if (this.preSelectWorkspaceById === undefined || !this.isDomInitializationDone) {
            return;
        }

        this.htmlWorkspaceSelect.val(this.preSelectWorkspaceById);
        this.preSelectWorkspaceById = undefined;
        await this.loadExtents();
    }

    // Evaluates the preselection of the workspaces
    private async evaluatePreSelectedExtent() {
        if (this.preSelectExtentByUri === undefined || !this.isDomInitializationDone) {
            return;
        }

        this.htmlExtentSelect.val(this.preSelectExtentByUri);
        this.preSelectExtentByUri = undefined;
        await this.loadItems();
    }

    addBreadcrumbItem(text: string, onClick: () => void): void {
        const tthis = this;
        const breadcrumbItem = $("<li class='breadcrumb-item active'></li>");
        breadcrumbItem.text(text);

        // Remove all breadcrumb items till that one
        breadcrumbItem.on('click', () => {
            onClick();
            tthis.refreshBreadcrumb();
        });

        this.htmlBreadcrumbList.append(breadcrumbItem);
    }
}