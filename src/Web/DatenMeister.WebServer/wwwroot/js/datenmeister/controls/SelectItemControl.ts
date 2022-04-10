import * as EL from '../client/Elements';
import {ItemWithNameAndId} from "../ApiModels";

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
    
    // This method will be called whenever the user has selected an item
    itemSelected: (selectedItem: ItemWithNameAndId) => void;
    
    // This method will be called when the workspace DOM elements have been updated
    domWorkspaceUpdated: () => void;
    
    // This method will be called when the extents DOM elements have been updated
    domExtentUpdated: () => void;
    
    // This method will be called when the domItems are updated
    domItemsUpdated: () => void;
    

    // Initializes the Select Item Control and adds it to the given parent
    init(parent: JQuery<HTMLElement>, settings?: Settings): JQuery {
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
            "<tr><td>Workspace: </td><td class='workspace'></td></tr>" +
            "<tr><td>Extent: </td><td class='extent'></td></tr>" +
            "<tr><td>Items: </td>" +
            "<td><div class='dm-breadcrumb'><nav aria-label='breadcrump'><ul class='breadcrumb'></ul></nav></div>" +
            "<div class='items'></div>" +
            "</td></tr>" +
            "<tr><td>Selected Item: </td><td class='dm-selectitemcontrol-selected'></td></tr>" +
            "<tr><td></td><td class='selected'>" +
            (this.settings.showCancelButton ? "<button class='btn btn-secondary dm-selectitemcontrol-cancelbtn' type='button'>Cancel</button>" : "") +
            "<button class='btn btn-primary dm-selectitemcontrol-button' type='button'>Set</button></td></tr>" +
            "</table>");

        $(".workspace", div).append(this.htmlWorkspaceSelect);
        $(".extent", div).append(this.htmlExtentSelect);
        $(".items", div).append(this.htmlItemsList);
        $(".dm-selectitemcontrol-selected", div).append(this.htmlSelectedElements);
        this.htmlBreadcrumbList = $(".breadcrumb", div);
        const setButton = $(".dm-selectitemcontrol-button", div);
        setButton.on('click', () => {
            const eventButton = tthis.itemSelected;
            if (eventButton != undefined) {
                eventButton(tthis.selectedItem);
            }
        });

        const cancelButton = $(".dm-selectitemcontrol-cancelbtn", div);
        cancelButton.on('click', () => {
            this.collapse();
        });

        parent.append(div);

        this.loadWorkspaces();
        this._containerDiv = div;

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

    loadWorkspaces(): Promise<any> {
        const tthis = this;
        return new Promise<any>(async resolve => {
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
            tthis.evaluatePreSelectedWorkspace();
            
            await this.loadExtents();
            
            resolve(true);
        });
    }

    // This call may also be given before the workspaces has been loaded
    setWorkspaceById(workspaceId: string) {
        this.preSelectWorkspaceById = workspaceId;
        this.evaluatePreSelectedWorkspace();
    }

    // This call may also be given before the extents has been loaded
    setExtentByUri(extentUri: string) {
        this.preSelectExtentByUri = extentUri;
        this.evaluatePreSelectedExtent();
    }

    // Sets the workspace by given the id
    getUserSelectedWorkspace(): string {
        return this.htmlWorkspaceSelect.val()?.toString() ?? "";
    }

    // Sets the extent by given the uri

    getUserSelectedExtent(): string {
        return this.htmlExtentSelect.val()?.toString() ?? "";
    }

    loadExtents(): Promise<any> {
        const tthis = this;
        return new Promise<any>(async resolve => {

            const workspaceId = this.getUserSelectedWorkspace();
            tthis.htmlSelectedElements.text(workspaceId);
            this.htmlExtentSelect.empty();

            if (workspaceId == "") {
                const select = $("<option value=''>--- Select Workspace ---</option>");
                this.htmlExtentSelect.append(select);
            } else {
                EL.getAllExtents(workspaceId).then(items => {

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
                });
            }

            tthis.refreshBreadcrumb();
            await this.loadItems();

            resolve(true);
        });
    }

    loadItems(selectedItem?: string): Promise<any> {

        const tthis = this;        
        return new Promise<any>(async resolve => {

            const workspaceId = this.getUserSelectedWorkspace();
            const extentUri = this.getUserSelectedExtent();
            this.htmlItemsList.empty();

            if (workspaceId == "" || extentUri == "") {
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

                resolve(true);
                
            } else {
                const funcElements = (items: ItemWithNameAndId[]) => {

                    for (const n in items) {
                        if (!items.hasOwnProperty(n)) continue;

                        const item = items[n];
                        const option = $("<li></li>");
                        option.text(item.name);
                        ((innerItem) =>
                            option.on('click', () => {
                                tthis.selectedItem = innerItem;
                                tthis.loadItems(innerItem.id);
                                tthis.htmlSelectedElements.text(innerItem.fullName);
                                tthis.visitedItems.push(item);
                                tthis.refreshBreadcrumb();
                            }))(item);

                        tthis.htmlItemsList.append(option);
                    }

                    resolve(true);
                };

                if (selectedItem === undefined || selectedItem === null) {
                    tthis.htmlSelectedElements.text(extentUri);
                    EL.getAllRootItems(workspaceId, extentUri).then(funcElements);
                } else {
                    EL.getAllChildItems(workspaceId, extentUri, selectedItem).then(funcElements);
                }
            }

            tthis.refreshBreadcrumb();
        });
    }

    refreshBreadcrumb() {
        const tthis = this;
        this.htmlBreadcrumbList.empty();

        if (this.settings.showBreadcrumb) {
            const currentWorkspace = this.getUserSelectedWorkspace();
            const currentExtent = this.getUserSelectedExtent();

            if (this.settings.showWorkspaceInBreadcrumb) {
                this.addBreadcrumbItem("DatenMeister", () => tthis.loadWorkspaces());
            }

            if (this.settings.showExtentInBreadcrumb) {
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

            if (currentExtent !== "" && currentExtent !== undefined) {
                this.addBreadcrumbItem(
                    currentExtent,
                    () => {
                        tthis.loadItems();
                        tthis.visitedItems.length = 0;
                    }
                );
            }

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

        if (this.preSelectWorkspaceById === undefined) {
            return;
        }

        this.htmlWorkspaceSelect.val(this.preSelectWorkspaceById);
        this.preSelectWorkspaceById = undefined;
        await this.loadExtents();
    }

    // Evaluates the preselection of the workspaces
    private async evaluatePreSelectedExtent() {
        if (this.preSelectExtentByUri === undefined) {
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