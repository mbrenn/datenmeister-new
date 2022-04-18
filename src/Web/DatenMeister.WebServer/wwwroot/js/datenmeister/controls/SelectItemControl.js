var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Elements", "../../burnsystems/Events"], function (require, exports, EL, Events_1) {
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
            /*
             * The events that can be subscribed
             */
            // This method will be called whenever the user has selected an item
            this.itemSelected = new Events_1.UserEvent();
            // This method will be called when the workspace DOM elements have been updated
            this.domWorkspaceUpdated = new Events_1.UserEvent();
            // This method will be called when the extents DOM elements have been updated
            this.domExtentUpdated = new Events_1.UserEvent();
            // This method will be called when the domItems are updated
            this.domItemsUpdated = new Events_1.UserEvent();
        }
        // Initializes the Select Item Control and adds it to the given parent
        init(parent, settings) {
            const div = this.initDom(settings, parent);
            this.loadWorkspaces();
            return div;
        }
        // Initializes the Select Item Control and adds it to the given parent
        // The promise is resolved as soon as the complete GUI is loaded 
        initAsync(parent, settings) {
            return __awaiter(this, void 0, void 0, function* () {
                const div = this.initDom(settings, parent);
                yield this.loadWorkspaces();
                return div;
            });
        }
        initDom(settings, parent) {
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
                "<tr><td>Workspace: </td><td class='dm-sic-workspace'></td></tr>" +
                "<tr><td>Extent: </td><td class='dm-sic-extent'></td></tr>" +
                "<tr><td>Items: </td>" +
                "<td><div class='dm-breadcrumb'><nav aria-label='breadcrump'><ul class='breadcrumb'></ul></nav></div>" +
                "<div class='dm-sic-items'></div>" +
                "</td></tr>" +
                "<tr><td>Selected Item: </td><td class='sic'></td></tr>" +
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
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
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
                const p1 = tthis.evaluatePreSelectedWorkspace();
                const p2 = this.loadExtents();
                // Calls the DOM workspace updated event
                Promise.all([p1]).then(() => {
                    tthis.domWorkspaceUpdated.invoke();
                });
                yield Promise.all([p1, p2]);
                return true;
            });
        }
        // This call may also be given before the workspaces has been loaded
        // The promise is resolved when the GUI is updated 
        setWorkspaceById(workspaceId) {
            return __awaiter(this, void 0, void 0, function* () {
                this.preSelectWorkspaceById = workspaceId;
                yield this.evaluatePreSelectedWorkspace();
            });
        }
        // This call may also be given before the extents has been loaded
        // The promise is resolved when the GUI is updated
        setExtentByUri(extentUri) {
            return __awaiter(this, void 0, void 0, function* () {
                this.preSelectExtentByUri = extentUri;
                yield this.evaluatePreSelectedExtent();
            });
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
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
                const workspaceId = this.getUserSelectedWorkspace();
                tthis.htmlSelectedElements.text(workspaceId);
                this.htmlExtentSelect.empty();
                if (workspaceId == "") {
                    const select = $("<option value=''>--- Select Workspace ---</option>");
                    this.htmlExtentSelect.append(select);
                }
                else {
                    const items = yield EL.getAllExtents(workspaceId);
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
                }
                tthis.refreshBreadcrumb();
                yield this.loadItems();
                this.domExtentUpdated.invoke();
                return true;
            });
        }
        loadItems(selectedItem) {
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
                const workspaceId = this.getUserSelectedWorkspace();
                const extentUri = this.getUserSelectedExtent();
                this.htmlItemsList.empty();
                if (workspaceId === "" || extentUri === "") {
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
                    return true;
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
                                tthis.itemSelected.invoke(innerItem);
                                tthis.loadItems(innerItem.id);
                                tthis.htmlSelectedElements.text(innerItem.fullName);
                                tthis.visitedItems.push(item);
                                tthis.refreshBreadcrumb();
                            }))(item);
                            tthis.htmlItemsList.append(option);
                        }
                        return true;
                    };
                    if (selectedItem === undefined || selectedItem === null) {
                        tthis.htmlSelectedElements.text(extentUri);
                        const rootElements = yield EL.getAllRootItems(workspaceId, extentUri);
                        funcElements(rootElements);
                    }
                    else {
                        const childElements = yield EL.getAllChildItems(workspaceId, extentUri, selectedItem);
                        funcElements(childElements);
                    }
                }
                tthis.refreshBreadcrumb();
                this.domItemsUpdated.invoke();
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