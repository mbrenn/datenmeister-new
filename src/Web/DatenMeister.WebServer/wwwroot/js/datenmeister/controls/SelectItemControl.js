var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Elements"], function (require, exports, EL) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.SelectItemControl = exports.Settings = void 0;
    class Settings {
        constructor() {
            this.showBreadcrumb = true;
            this.showWorkspaceInBreadcrumb = false;
            this.showExtentInBreadcrumb = false;
            this.showCancelButton = true;
        }
    }
    exports.Settings = Settings;
    class SelectItemControl {
        constructor() {
            this.visitedItems = new Array();
            this.loadedWorkspaces = new Array();
            this.loadedExtents = new Array();
        }
        // Initializes the Select Item Control and adds it to the given parent
        init(parent, settings) {
            this.settings = settings !== null && settings !== void 0 ? settings : new Settings();
            const tthis = this;
            this.htmlWorkspaceSelect = $("<select></select>");
            this.htmlExtentSelect = $("<select></select>");
            this.htmlSelectedElements = $("<div></div>");
            this.htmlItemsList = $("<ul></ul>");
            // Defines the handler whenever the user changes something
            this.htmlWorkspaceSelect.on('change', () => tthis.onWorkspaceChangedByUser());
            this.htmlExtentSelect.on('change', () => tthis.onExtentChangedByUser());
            const div = $("<table class='dm-selectitemcontrol'>" +
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
        onWorkspaceChangedByUser() {
            return __awaiter(this, void 0, void 0, function* () {
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
                yield this.loadExtents();
            });
        }
        // This method will be called when the user changed the extent select item
        onExtentChangedByUser() {
            return __awaiter(this, void 0, void 0, function* () {
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
                yield this.loadItems();
            });
        }
        collapse() {
            var _a;
            (_a = this._containerDiv) === null || _a === void 0 ? void 0 : _a.remove();
            this._containerDiv = undefined;
        }
        loadWorkspaces() {
            const tthis = this;
            return new Promise((resolve) => __awaiter(this, void 0, void 0, function* () {
                tthis.htmlWorkspaceSelect.empty();
                const items = yield EL.getAllWorkspaces();
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
                tthis.evaluatePreSelectedWorkspace();
                yield this.loadExtents();
                resolve(true);
            }));
        }
        // This call may also be given before the workspaces has been loaded
        setWorkspaceById(workspaceId) {
            this.preSelectWorkspaceById = workspaceId;
            this.evaluatePreSelectedWorkspace();
        }
        // This call may also be given before the extents has been loaded
        setExtentByUri(extentUri) {
            this.preSelectExtentByUri = extentUri;
            this.evaluatePreSelectedExtent();
        }
        // Sets the workspace by given the id
        getUserSelectedWorkspace() {
            var _a, _b;
            return (_b = (_a = this.htmlWorkspaceSelect.val()) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
        }
        // Sets the extent by given the uri
        getUserSelectedExtent() {
            var _a, _b;
            return (_b = (_a = this.htmlExtentSelect.val()) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
        }
        loadExtents() {
            const tthis = this;
            return new Promise((resolve) => __awaiter(this, void 0, void 0, function* () {
                const workspaceId = this.getUserSelectedWorkspace();
                tthis.htmlSelectedElements.text(workspaceId);
                this.htmlExtentSelect.empty();
                if (workspaceId == "") {
                    const select = $("<option value=''>--- Select Workspace ---</option>");
                    this.htmlExtentSelect.append(select);
                }
                else {
                    EL.getAllExtents(workspaceId).then(items => {
                        tthis.visitedItems.length = 0;
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
                        tthis.evaluatePreSelectedExtent();
                    });
                }
                tthis.refreshBreadcrumb();
                yield this.loadItems();
                resolve(true);
            }));
        }
        loadItems(selectedItem) {
            const tthis = this;
            return new Promise((resolve) => __awaiter(this, void 0, void 0, function* () {
                const workspaceId = this.getUserSelectedWorkspace();
                const extentUri = this.getUserSelectedExtent();
                this.htmlItemsList.empty();
                if (workspaceId == "" || extentUri == "") {
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
                    resolve(true);
                }
                else {
                    const funcElements = (items) => {
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
                        resolve(true);
                    };
                    if (selectedItem === undefined || selectedItem === null) {
                        tthis.htmlSelectedElements.text(extentUri);
                        EL.getAllRootItems(workspaceId, extentUri).then(funcElements);
                    }
                    else {
                        EL.getAllChildItems(workspaceId, extentUri, selectedItem).then(funcElements);
                    }
                }
                tthis.refreshBreadcrumb();
            }));
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
        // Evaluates the preselection of the workspaces
        evaluatePreSelectedWorkspace() {
            return __awaiter(this, void 0, void 0, function* () {
                if (this.preSelectWorkspaceById === undefined) {
                    return;
                }
                this.htmlWorkspaceSelect.val(this.preSelectWorkspaceById);
                this.preSelectWorkspaceById = undefined;
                yield this.loadExtents();
            });
        }
        // Evaluates the preselection of the workspaces
        evaluatePreSelectedExtent() {
            return __awaiter(this, void 0, void 0, function* () {
                if (this.preSelectExtentByUri === undefined) {
                    return;
                }
                this.htmlExtentSelect.val(this.preSelectExtentByUri);
                this.preSelectExtentByUri = undefined;
                yield this.loadItems();
            });
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
//# sourceMappingURL=SelectItemControl.js.map