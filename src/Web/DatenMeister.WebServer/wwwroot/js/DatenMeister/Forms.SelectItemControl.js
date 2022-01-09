define(["require", "exports", "./Client.Elements"], function (require, exports, EL) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {value: true});
    exports.SelectItemControl = exports.Settings = void 0;

    class Settings {
        constructor() {
            this.showBreadcrumb = true;
            this.showWorkspaceInBreadcrumb = false;
            this.showExtentInBreadcrumb = false;
        }
    }
    exports.Settings = Settings;
    class SelectItemControl {
        constructor() {
            this.visitedItems = new Array();
            this.loadedWorkspaces = new Array();
            this.loadedExtents = new Array();
        }
        init(parent, settings) {
            this.settings = settings !== null && settings !== void 0 ? settings : new Settings();
            const tthis = this;
            this.htmlWorkspaceSelect = $("<select></select>");
            this.htmlExtentSelect = $("<select></select>");
            this.htmlSelectedElements = $("<div></div>");
            this.htmlItemsList = $("<ul></ul>");
            this.htmlWorkspaceSelect.on('change', () => {
                // Find the selected workspace
                tthis.selectedWorkspace = undefined;
                const currentWorkspace = tthis.getSelectedWorkspace();
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
                const currentExtent = tthis.getSelectedExtent();
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
            const div = $("<table class='dm-selectitemcontrol'>" +
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
        loadWorkspaces() {
            const tthis = this;
            EL.getAllWorkspaces().done((items) => {
                tthis.htmlWorkspaceSelect.empty();
                tthis.visitedItems.length = 0;
                tthis.loadedWorkspaces = items;
                const none = $("<option value=''>--- None ---</option>");
                tthis.htmlWorkspaceSelect.append(none);
                for (const n in items) {
                    if (!items.hasOwnProperty(n))
                        continue;
                    const item = items[n];
                    const option = $("<option></option>");
                    option.val(item.id);
                    option.text(item.name);
                    tthis.htmlWorkspaceSelect.append(option);
                }
                tthis.refreshBreadcrumb();
            });
            this.loadExtents();
        }
        getSelectedWorkspace() {
            var _a, _b;
            return (_b = (_a = this.htmlWorkspaceSelect.val()) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
        }
        getSelectedExtent() {
            var _a, _b;
            return (_b = (_a = this.htmlExtentSelect.val()) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
        }
        loadExtents() {
            const tthis = this;
            const workspaceId = this.getSelectedWorkspace();
            tthis.htmlSelectedElements.text(workspaceId);
            if (workspaceId == "") {
                this.htmlExtentSelect.empty();
                const select = $("<option value=''>--- Select Workspace ---</option>");
                this.htmlExtentSelect.append(select);
            }
            else {
                EL.getAllExtents(workspaceId).done(items => {
                    this.htmlExtentSelect.empty();
                    this.visitedItems.length = 0;
                    const none = $("<option value=''>--- None ---</option>");
                    this.htmlExtentSelect.append(none);
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
                });
            }
            tthis.refreshBreadcrumb();
            this.loadItems();
        }
        loadItems(selectedItem) {
            const workspaceId = this.getSelectedWorkspace();
            const extentUri = this.getSelectedExtent();
            const tthis = this;
            if (workspaceId == "" || extentUri == "") {
                this.htmlItemsList.empty();
                const select = $("<li>--- Select Extent ---</li>");
                this.htmlItemsList.append(select);
                this.visitedItems.length = 0;
                if (extentUri !== "" && extentUri !== undefined && extentUri !== null) {
                    this.visitedItems.push({
                        id: extentUri,
                        name: extentUri,
                        fullName: extentUri,
                        extentUri: extentUri
                    });
                }
            }
            else {
                const funcElements = (items) => {
                    this.htmlItemsList.empty();
                    for (const n in items) {
                        if (!items.hasOwnProperty(n))
                            continue;
                        const item = items[n];
                        const option = $("<li></li>");
                        option.text(item.name);
                        ((innerItem) => option.on('click', () => {
                            tthis.selectedItem = innerItem;
                            tthis.loadItems(innerItem.id);
                            tthis.htmlSelectedElements.text(innerItem.fullName);
                            tthis.visitedItems.push(item);
                            tthis.refreshBreadcrumb();
                        }))(item);
                        tthis.htmlItemsList.append(option);
                    }
                };
                if (selectedItem === undefined || selectedItem === null) {
                    tthis.htmlSelectedElements.text(extentUri);
                    EL.getAllRootItems(workspaceId, extentUri).done(funcElements);
                }
                else {
                    EL.getAllChildItems(workspaceId, extentUri, selectedItem).done(funcElements);
                }
            }
            tthis.refreshBreadcrumb();
        }
        refreshBreadcrumb() {
            const tthis = this;
            this.htmlBreadcrumbList.empty();
            if (this.settings.showBreadcrumb) {
                const currentWorkspace = this.getSelectedWorkspace();
                const currentExtent = this.getSelectedExtent();
                if (this.settings.showWorkspaceInBreadcrumb) {
                    this.addBreadcrumbItem("DatenMeister", () => tthis.loadWorkspaces());
                }
                if (this.settings.showExtentInBreadcrumb) {
                    if (currentWorkspace !== "" && currentWorkspace !== undefined) {
                        this.addBreadcrumbItem(currentWorkspace, () => {
                            tthis.loadExtents();
                            tthis.visitedItems.length = 0;
                        });
                    }
                }
                if (currentExtent !== "" && currentExtent !== undefined) {
                    this.addBreadcrumbItem(currentExtent, () => {
                        tthis.loadItems();
                        tthis.visitedItems.length = 0;
                    });
                }
                for (let n = 0; n < this.visitedItems.length; n++) {
                    const item = this.visitedItems[n];
                    this.addBreadcrumbItem(item.name, () => {
                        tthis.loadItems(item.id);
                        tthis.visitedItems = tthis.visitedItems.slice(0, n + 1);
                    });
                }
            }
        }
        addBreadcrumbItem(text, onClick) {
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
    exports.SelectItemControl = SelectItemControl;
});
//# sourceMappingURL=Forms.SelectItemControl.js.map