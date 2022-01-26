import * as EL from './Client.Elements';
import {ItemWithNameAndId} from "./ApiModels";

export class Settings {
    showBreadcrumb = true;
    showWorkspaceInBreadcrumb = false;
    showExtentInBreadcrumb = false;
}

export class SelectItemControl {
    private htmlWorkspaceSelect: JQuery<HTMLElement>;
    private htmlItemsList: JQuery<HTMLElement>;
    private htmlExtentSelect: JQuery<HTMLElement>;
    private htmlSelectedElements: JQuery<HTMLElement>;
    private htmlBreadcrumbList: JQuery<HTMLElement>;
    private settings: Settings;

    private visitedItems: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    private loadedWorkspaces: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    private loadedExtents: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    private deferLoadedWorkspaces: JQueryDeferred<void> = $.Deferred<void>();
    private deferLoadedExtent: JQueryDeferred<void> = $.Deferred<void>();
    private selectedWorkspace?: ItemWithNameAndId;
    private selectedExtent?: ItemWithNameAndId;
    private selectedItem?: ItemWithNameAndId;

    // Defines the id of the workspace, when the workspace shall be pre-selected
    // This value is set to undefined, when no selection shall be given
    private preSelectWorkspaceById: string | undefined;

    // Defines the id of the extent, when the extent shall be pre-selected
    // This value is set to undefined, when no selection shall be given
    private preSelectExtentByUri: string | undefined;

    onItemSelected: (selectedItem: ItemWithNameAndId) => void;

    init(parent: JQuery<HTMLElement>, settings?: Settings): JQuery {
        this.settings = settings ?? new Settings();

        const tthis = this;
        this.htmlWorkspaceSelect = $("<select></select>");
        this.htmlExtentSelect = $("<select></select>");
        this.htmlSelectedElements = $("<div></div>");
        this.htmlItemsList = $("<ul></ul>");

        this.htmlWorkspaceSelect.on('change', () => {
            // Find the selected workspace
            tthis.selectedWorkspace = undefined;
            const currentWorkspace = tthis.getUserSelectedWorkspace();
            if (currentWorkspace != "" && currentWorkspace != undefined) {
                for (let n = 0; n < tthis.loadedWorkspaces.length; n++) {
                    const item = tthis.loadedWorkspaces[n];
                    if (item.id == currentWorkspace) {
                        tthis.selectedWorkspace = item;
                        break;
                    }
                }
            }

            tthis.loadExtents();
        });

        this.htmlExtentSelect.on('change', () => {
            // Find the selected extent
            tthis.selectedExtent = undefined;
            const currentExtent = tthis.getUserSelectedExtent();
            if (currentExtent != "" && currentExtent != undefined) {
                for (let n = 0; n < tthis.loadedExtents.length; n++) {
                    const item = tthis.loadedExtents[n];
                    if (item.extentUri == currentExtent) {
                        tthis.selectedExtent = item;
                        break;
                    }
                }
            }

            tthis.loadItems();
        });

        const div = $(
            "<table class='dm-selectitemcontrol'>" +
            "<tr><td>Workspace: </td><td class='workspace'></td></tr>" +
            "<tr><td>Extent: </td><td class='extent'></td></tr>" +
            "<tr><td>Items: </td>" +
            "<td><div class='dm-breadcrumb'><nav aria-label='breadcrump'><ul class='breadcrumb'></ul></nav></div>" +
            "<div class='items'></div>" +
            "</td></tr>" +
            "<tr><td>Selected Item: </td><td class='dm-selectitemcontrol-selected'></td></tr>" +
            "<tr><td></td><td class='selected'><button class='btn btn-primary dm-selectitemcontrol-button' type='button'>Set</button></td></tr>" +
            "</table>");

        $(".workspace", div).append(this.htmlWorkspaceSelect);
        $(".extent", div).append(this.htmlExtentSelect);
        $(".items", div).append(this.htmlItemsList);
        $(".dm-selectitemcontrol-selected", div).append(this.htmlSelectedElements);
        this.htmlBreadcrumbList = $(".breadcrumb", div);
        const setButton = $(".dm-selectitemcontrol-button", div);
        setButton.on('click', () => {
            const eventButton = tthis.onItemSelected;
            if (eventButton != undefined) {
                eventButton(tthis.selectedItem);
            }
        });

        parent.append(div);

        this.loadWorkspaces();

        return div;
    }

    loadWorkspaces(): JQueryDeferred<any> {
        const tthis = this;
        const r = jQuery.Deferred<any, never, never>();

        EL.getAllWorkspaces().done((items) => {
            tthis.htmlWorkspaceSelect.empty();
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
            tthis.deferLoadedWorkspaces.resolve();
            tthis.evaluatePreSelectedWorkspace();
        });

        this.loadExtents().done(() => {
            r.resolve(true);
        });

        return r;
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

    loadExtents(): JQueryDeferred<any> {
        const r = jQuery.Deferred<any, never, never>();

        const tthis = this;
        const workspaceId = this.getUserSelectedWorkspace();
        tthis.htmlSelectedElements.text(workspaceId);

        if (workspaceId == "") {
            this.htmlExtentSelect.empty();

            const select = $("<option value=''>--- Select Workspace ---</option>");
            this.htmlExtentSelect.append(select);
        } else {
            EL.getAllExtents(workspaceId).done(items => {

                tthis.htmlExtentSelect.empty();
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

                tthis.deferLoadedExtent.resolve();
            });
        }

        tthis.refreshBreadcrumb();
        this.loadItems().done(() => {
            r.resolve(true);
        });

        return r;
    }

    loadItems(selectedItem?: string): JQueryDeferred<any> {
        const r = jQuery.Deferred<any, never, never>();

        const workspaceId = this.getUserSelectedWorkspace();
        const extentUri = this.getUserSelectedExtent();
        const tthis = this;

        if (workspaceId == "" || extentUri == "") {
            this.htmlItemsList.empty();
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

            r.resolve(true);
        } else {
            const funcElements = (items: ItemWithNameAndId[]) => {
                this.htmlItemsList.empty();

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

                r.resolve(true);
            };

            if (selectedItem === undefined || selectedItem === null) {
                tthis.htmlSelectedElements.text(extentUri);
                EL.getAllRootItems(workspaceId, extentUri).done(funcElements);
            } else {
                EL.getAllChildItems(workspaceId, extentUri, selectedItem).done(funcElements);
            }
        }

        tthis.refreshBreadcrumb();

        return r;
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
    private evaluatePreSelectedWorkspace() {
        const tthis = this;
        this.deferLoadedWorkspaces.done(() => {

            if (tthis.preSelectWorkspaceById === undefined) {
                return;
            }

            tthis.htmlWorkspaceSelect.val(this.preSelectWorkspaceById);
            tthis.preSelectWorkspaceById = undefined;
            tthis.loadExtents();
        });
    }

    // Evaluates the preselection of the workspaces
    private evaluatePreSelectedExtent() {
        const tthis = this;
        this.deferLoadedExtent.done(() => {

            if (tthis.preSelectExtentByUri === undefined) {
                return;
            }

            tthis.htmlExtentSelect.val(this.preSelectExtentByUri);
            tthis.preSelectExtentByUri = undefined;
            tthis.loadItems();
        });
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