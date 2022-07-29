var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Elements", "../client/Items", "../ApiModels", "../../burnsystems/Events", "../DomHelper"], function (require, exports, EL, ItemsClient, ApiModels_1, Events_1, DomHelper_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.SelectItemControl = exports.Settings = void 0;
    class Settings {
        constructor() {
            this.showBreadcrumb = true;
            this.showWorkspaceInBreadcrumb = false;
            this.showExtentInBreadcrumb = false;
            this.showCancelButton = true;
            this.hideAtStartup = false;
            this.setButtonText = "Set";
            this.headline = undefined;
        }
    }
    exports.Settings = Settings;
    class SelectItemControl {
        constructor() {
            this.loadedWorkspaces = new Array();
            this.loadedExtents = new Array();
            /*
             * The events that can be subscribed
             */
            // This method will be called whenever the user has selected an item via the 'Set' button 
            this.itemSelected = new Events_1.UserEvent();
            // This method will be called whenever the user has selected an item in the dropdown
            this.itemClicked = new Events_1.UserEvent();
            // This method will be called when the workspace DOM elements have been updated
            this.domWorkspaceUpdated = new Events_1.UserEvent();
            // This method will be called when the extents DOM elements have been updated
            this.domExtentUpdated = new Events_1.UserEvent();
            // This method will be called when the domItems are updated
            this.domItemsUpdated = new Events_1.UserEvent();
            this.isDomInitializationDone = false;
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
            $(".dm-sic-workspace", div).append(this.htmlWorkspaceSelect);
            $(".dm-sic-extent", div).append(this.htmlExtentSelect);
            $(".dm-sic-items", div).append(this.htmlItemsList);
            $(".dm-sic-selected", div).append(this.htmlSelectedElements);
            if (this.settings.headline !== undefined) {
                $(".dm-selectitemcontrol-headline", div).text(settings.headline);
            }
            this.htmlBreadcrumbList = $(".breadcrumb", div);
            const setButton = $(".dm-sic-button", div);
            setButton.text(this.settings.setButtonText);
            setButton.on('click', () => {
                tthis.itemSelected.invoke(tthis.selectedItem);
            });
            const cancelButton = $(".dm-sic-cancelbtn", div);
            cancelButton.on('click', () => {
                this.collapse();
            });
            if (settings === null || settings === void 0 ? void 0 : settings.hideAtStartup) {
                div.hide();
            }
            parent.append(div);
            this._containerDiv = div;
            this.isDomInitializationDone = true;
            return div;
        }
        showControl() {
            if (this._containerDiv !== undefined) {
                this._containerDiv.show();
            }
        }
        // This method will be called when the user changed the workspace
        onWorkspaceChangedByUser() {
            return __awaiter(this, void 0, void 0, function* () {
                // Find the selected workspace
                this.selectedWorkspace = undefined;
                const currentWorkspace = this.getUserSelectedWorkspace();
                if (currentWorkspace !== "" && currentWorkspace !== undefined) {
                    for (let n = 0; n < this.loadedWorkspaces.length; n++) {
                        const item = this.loadedWorkspaces[n];
                        if (item.id === currentWorkspace) {
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
                if (currentExtent !== "" && currentExtent != undefined) {
                    for (let n = 0; n < this.loadedExtents.length; n++) {
                        const item = this.loadedExtents[n];
                        if (item.extentUri === currentExtent) {
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
        setItemByUri(workspaceId, itemUri) {
            return __awaiter(this, void 0, void 0, function* () {
                if (!this.isDomInitializationDone) {
                    throw "DOM is not initialized. Call initDom first";
                }
                const item = yield EL.loadNameByUri(workspaceId, itemUri);
                this.selectedItem = item;
                yield this.setWorkspaceById(workspaceId);
                yield this.setExtentByUri(item.extentUri);
                yield this.loadItems(item.uri);
                this.htmlSelectedElements.empty();
                this.htmlSelectedElements.append(yield (0, DomHelper_1.convertItemWithNameAndIdToDom)(item));
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
                if (workspaceId === "") {
                    const select = $("<option value=''>--- Select Workspace ---</option>");
                    this.htmlExtentSelect.append(select);
                }
                else {
                    const items = yield EL.getAllExtents(workspaceId);
                    this.htmlExtentSelect.empty();
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
                    return true;
                }
                else {
                    const funcElements = (items) => {
                        for (const n in items) {
                            if (!items.hasOwnProperty(n))
                                continue;
                            const item = items[n];
                            const option = $("<li class='dm-sic-item'></li>");
                            option.append((0, DomHelper_1.convertItemWithNameAndIdToDom)(item, { inhibitItemLink: true }));
                            // Creates the clickability of the list of items
                            ((innerItem) => option.on('click', () => {
                                tthis.selectedItem = innerItem;
                                tthis.loadItems(innerItem.uri);
                                tthis.itemClicked.invoke(innerItem);
                                tthis.htmlSelectedElements.empty();
                                tthis.htmlSelectedElements.append((0, DomHelper_1.convertItemWithNameAndIdToDom)(item));
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
                        const childElements = yield EL.getAllChildItems(workspaceId, selectedItem);
                        funcElements(childElements);
                    }
                }
                tthis.refreshBreadcrumb();
                this.domItemsUpdated.invoke();
            });
        }
        // Refreshes the elements on the bread crumb 
        refreshBreadcrumb() {
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
                const currentWorkspace = this.getUserSelectedWorkspace();
                const currentExtent = this.getUserSelectedExtent();
                let containerItems;
                if (this.selectedItem !== undefined && this.selectedItem.uri !== undefined) {
                    containerItems = yield ItemsClient.getContainer(currentWorkspace, this.selectedItem.uri, true);
                }
                this.htmlBreadcrumbList.empty();
                if (this.settings.showBreadcrumb) {
                    // Starts by showing the button to select to select the Workspaces
                    if (this.settings.showWorkspaceInBreadcrumb) {
                        this.addBreadcrumbItem("Workspaces", () => tthis.loadWorkspaces());
                        // Now show the current workspace
                        if (currentWorkspace !== "" && currentWorkspace !== undefined) {
                            this.addBreadcrumbItem(currentWorkspace, () => {
                                tthis.loadExtents();
                            });
                        }
                    }
                    // Shows the extent itself in the breadcrumb, if configured
                    if (this.settings.showExtentInBreadcrumb) {
                        if (currentExtent !== "" && currentExtent !== undefined) {
                            this.addBreadcrumbItem(currentExtent, () => {
                                tthis.loadItems();
                            });
                        }
                    }
                    if (containerItems !== undefined) {
                        // Otherwise, just go to the parents
                        for (let n = 0; n < containerItems.length; n++) {
                            const item = containerItems[containerItems.length - 1 - n];
                            if (item.ententType !== ApiModels_1.EntentType.Item) {
                                // The shown item is not of type "Item", this means it is an extent or a workspace
                                // The Extent or Workspace is covered by the source above
                                continue;
                            }
                            ((innerItem) => {
                                this.addBreadcrumbItem(item.name, () => {
                                    this.selectedItem = innerItem;
                                    tthis.loadItems(item.uri);
                                });
                            })(item);
                        }
                    }
                }
            });
        }
        // Evaluates the preselection of the workspaces
        evaluatePreSelectedWorkspace() {
            return __awaiter(this, void 0, void 0, function* () {
                if (this.preSelectWorkspaceById === undefined || !this.isDomInitializationDone) {
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
                if (this.preSelectExtentByUri === undefined || !this.isDomInitializationDone) {
                    return;
                }
                this.htmlExtentSelect.val(this.preSelectExtentByUri);
                this.preSelectExtentByUri = undefined;
                yield this.loadItems();
            });
        }
        // Evaluates the preselection of the workspaces
        evaluatePreselectedItem() {
            return __awaiter(this, void 0, void 0, function* () {
                if (this.preSelectItemUri === undefined || !this.isDomInitializationDone) {
                    return;
                }
                yield this.loadItems(this.preSelectItemUri);
                this.preSelectItemUri = undefined;
            });
        }
        addBreadcrumbItem(text, onClick) {
            const tthis = this;
            const breadcrumbItem = $("<li class='breadcrumb-item active'></li>");
            breadcrumbItem.text(text);
            // Remove all breadcrumb items till that one
            breadcrumbItem.on('click', () => __awaiter(this, void 0, void 0, function* () {
                onClick();
                yield tthis.refreshBreadcrumb();
            }));
            this.htmlBreadcrumbList.append(breadcrumbItem);
        }
    }
    exports.SelectItemControl = SelectItemControl;
});
//# sourceMappingURL=SelectItemControl.js.map