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
             * Thsee events that can be subscribed
             */
            // This method will be called whenever the user has selected an item via the 'Set' button 
            this.itemSelected = new Events_1.UserEvent();
            /**
             * This method will be called whenever the user has selected an item in the dropdown
             */
            this.itemClicked = new Events_1.UserEvent();
            this.isDomInitializationDone = false;
        }
        // Initializes the Select Item Control and adds it to the given parent
        init(parent, settings) {
            // Performs the initialization of the DOM, providing all elements
            // and event handlers
            const div = this.initDom(settings, parent);
            // Loads the workspaces
            const _ = this.loadWorkspaces();
            return div;
        }
        // Initializes the Select Item Control and adds it to the given parent
        // The promise is resolved as soon as the complete GUI is loaded 
        initAsync(parent, settings) {
            return __awaiter(this, void 0, void 0, function* () {
                // Performs the initialization of the DOM, providing all elements
                // and event handlers
                const div = this.initDom(settings, parent);
                // Loads the workspaces
                yield this.loadWorkspaces();
                return div;
            });
        }
        /**
         * This method just creates the DOM and connects the events of the elements to the
         * invocation methods
         * @param settings Settings to be used
         * @param container JQuery-Container Element hosting the content
         * @private
         */
        initDom(settings, container) {
            this.settings = settings !== null && settings !== void 0 ? settings : new Settings();
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
            const div = $("<table class='dm-selectitemcontrol'>" +
                "<tr><th colspan='2' class='dm-selectitemcontrol-headline'>Select item:</th></tr>" +
                "<tr><th>Workspace: </th><td class='dm-sic-workspace'></td></tr>" +
                "<tr><th>Extent: </th><td class='dm-sic-extent'></td></tr>" +
                "<tr><th>Selected Item: </th><td><div class='dm-sic-selected'></div></td></tr>" +
                "<tr><th>Children: </th>" +
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
            setButton.on("click", () => {
                tthis.itemSelected.invoke(tthis.selectedItem);
            });
            cancelButton.on("click", () => {
                this.removeControl();
            });
            if (settings === null || settings === void 0 ? void 0 : settings.hideAtStartup) {
                div.hide();
            }
            container.append(div);
            this.containerDiv = div;
            this.isDomInitializationDone = true;
            return div;
        }
        /**
         * Shows the control (by revoking the hide status)
         */
        showControl() {
            if (this.containerDiv !== undefined) {
                this.containerDiv.show();
            }
        }
        /**
         * Removes the control. This means that 'init(Async)' must be called again
         */
        removeControl() {
            var _a;
            (_a = this.containerDiv) === null || _a === void 0 ? void 0 : _a.remove();
            this.containerDiv = undefined;
        }
        /**
         * This method will be called when the user changed the selected workspace
         * @private
         */
        onWorkspaceChangedByUser() {
            return __awaiter(this, void 0, void 0, function* () {
                // Find the selected workspace
                this.selectedItem = undefined;
                this.htmlExtentSelect.val("");
                this.preSelectExtentByUri = "";
                yield this.loadExtents();
            });
        }
        /**
         * This method will be called when the user changed the extent select item
         * @private
         */
        onExtentChangedByUser() {
            return __awaiter(this, void 0, void 0, function* () {
                // Find the selected extent
                this.preSelectItemUri = undefined;
                this.selectedItem =
                    {
                        workspace: this.getUserSelectedWorkspaceId(),
                        extentUri: this.getUserSelectedExtentUri(),
                        ententType: ApiModels_1.EntentType.Extent,
                        uri: this.getUserSelectedExtentUri()
                    };
                yield this.loadItems();
            });
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
        loadWorkspaces() {
            return __awaiter(this, void 0, void 0, function* () {
                this.htmlWorkspaceSelect.empty();
                let currentlySelectedWorkspace = this.getUserSelectedWorkspaceId();
                if (this.preSelectWorkspaceById !== undefined) {
                    // If there is a pre-selection, the pre-selection is valid
                    currentlySelectedWorkspace = this.preSelectWorkspaceById;
                    this.preSelectWorkspaceById = undefined;
                }
                const items = yield EL.getAllWorkspaces();
                this.loadedWorkspaces = items;
                const none = $("<option value=''>--- None ---</option>");
                this.htmlWorkspaceSelect.append(none);
                for (const n in items) {
                    if (!items.hasOwnProperty(n))
                        continue;
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
                yield this.loadExtents();
            });
        }
        /**
         * This call may also be given before the workspaces has been loaded
         * The promise is resolved when the GUI is updated
         *
         * @param workspaceId ID of the workspace which is preselected.
         */
        setWorkspaceById(workspaceId) {
            return __awaiter(this, void 0, void 0, function* () {
                this.preSelectWorkspaceById = workspaceId;
                if (this.isDomInitializationDone) {
                    yield this.loadWorkspaces();
                }
            });
        }
        loadExtents() {
            return __awaiter(this, void 0, void 0, function* () {
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
                    // Restores the selected item
                    if (extentUri !== undefined) {
                        this.htmlExtentSelect.val(extentUri);
                        tthis.htmlSelectedElements.text(extentUri);
                    }
                }
                yield tthis.loadItems();
                return true;
            });
        }
        // This call may also be given before the extents has been loaded
        // The promise is resolved when the GUI is updated
        setExtentByUri(workspaceId, extentUri) {
            return __awaiter(this, void 0, void 0, function* () {
                this.preSelectWorkspaceById = workspaceId;
                this.preSelectExtentByUri = extentUri;
                this.selectedItem =
                    {
                        workspace: workspaceId,
                        extentUri: extentUri,
                        ententType: ApiModels_1.EntentType.Extent,
                        uri: extentUri
                    };
                if (this.isDomInitializationDone) {
                    yield this.loadWorkspaces();
                }
            });
        }
        setItemByUri(workspaceId, itemUri) {
            return __awaiter(this, void 0, void 0, function* () {
                const item = yield EL.loadNameByUri(workspaceId, itemUri);
                if (item === undefined) {
                    throw "Item: " + workspaceId + ":" + itemUri + " has not been found";
                }
                this.preSelectWorkspaceById = workspaceId;
                this.preSelectExtentByUri = item.extentUri;
                this.preSelectItemUri = item.uri;
                if (this.isDomInitializationDone) {
                    yield this.loadWorkspaces();
                }
            });
        }
        // Sets the workspace by given the id
        getUserSelectedWorkspaceId() {
            var _a, _b;
            return (_b = (_a = this.htmlWorkspaceSelect.val()) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
        }
        // Sets the extent by given the uri
        getUserSelectedExtentUri() {
            var _a, _b;
            return (_b = (_a = this.htmlExtentSelect.val()) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
        }
        loadItems() {
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
                let selectedItem = this.selectedItem;
                // Checks, whether the user has selected or preselected an item
                if (this.preSelectItemUri !== undefined) {
                    if (this.preSelectItemUri === "") {
                        // Empty string is used to indicate that the user would like to select the 
                        // complete extent
                        selectedItem = undefined;
                    }
                    else {
                        // User has selected a specfic item
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
                    || selectedItem === undefined) {
                    const select = $("<li>--- Select Extent ---</li>");
                    this.htmlItemsList.append(select);
                }
                else {
                    const item = yield ItemsClient.getItemWithNameAndId(selectedItem.workspace, selectedItem.uri);
                    if (item !== undefined) {
                        this.htmlSelectedElements.empty();
                        this.htmlSelectedElements.append((0, DomHelper_1.convertItemWithNameAndIdToDom)(item));
                    }
                    const funcElements = (items) => {
                        for (const n in items) {
                            if (!items.hasOwnProperty(n))
                                continue;
                            const item = items[n];
                            const option = $("<li class='dm-sic-item'></li>");
                            option.append((0, DomHelper_1.convertItemWithNameAndIdToDom)(item, { inhibitItemLink: true }));
                            // Creates the clickability of the list of items
                            ((innerItem) => option.on("click", () => __awaiter(this, void 0, void 0, function* () {
                                tthis.selectedItem = innerItem;
                                tthis.itemClicked.invoke(innerItem);
                                yield tthis.loadItems();
                                tthis.htmlSelectedElements.empty();
                                tthis.htmlSelectedElements.append((0, DomHelper_1.convertItemWithNameAndIdToDom)(item));
                                yield tthis.refreshBreadcrumb();
                            })))(item);
                            tthis.htmlItemsList.append(option);
                        }
                        return true;
                    };
                    if (selectedItem.ententType === ApiModels_1.EntentType.Extent || selectedItem === undefined || selectedItem === null) {
                        tthis.htmlSelectedElements.text(extentUri);
                        const rootElements = yield EL.getAllRootItems(workspaceId, extentUri);
                        funcElements(rootElements);
                    }
                    else {
                        const childElements = yield EL.getAllChildItems(workspaceId, selectedItem.uri);
                        funcElements(childElements);
                    }
                }
                yield tthis.refreshBreadcrumb();
            });
        }
        // Refreshes the elements on the bread crumb 
        refreshBreadcrumb() {
            return __awaiter(this, void 0, void 0, function* () {
                const tthis = this;
                const currentWorkspace = this.getUserSelectedWorkspaceId();
                const currentExtent = this.getUserSelectedExtentUri();
                let containerItems;
                if (this.selectedItem !== undefined && this.selectedItem.uri !== undefined) {
                    containerItems = yield ItemsClient.getContainer(currentWorkspace, this.selectedItem.uri, true);
                }
                this.htmlBreadcrumbList.empty();
                if (this.settings.showBreadcrumb) {
                    // Starts by showing the button to select to select the Workspaces
                    if (this.settings.showWorkspaceInBreadcrumb) {
                        this.addBreadcrumbItem("Workspaces", () => __awaiter(this, void 0, void 0, function* () {
                            this.preSelectWorkspaceById = "";
                            this.preSelectExtentByUri = "";
                            this.selectedItem = undefined;
                            yield tthis.loadWorkspaces();
                        }));
                        // Now show the current workspace
                        if (currentWorkspace !== "" && currentWorkspace !== undefined) {
                            this.addBreadcrumbItem(currentWorkspace, () => __awaiter(this, void 0, void 0, function* () {
                                this.preSelectExtentByUri = "";
                                this.selectedItem = undefined;
                                yield tthis.loadExtents();
                            }));
                        }
                    }
                    // Shows the extent itself in the breadcrumb, if configured
                    if (this.settings.showExtentInBreadcrumb) {
                        if (currentExtent !== "" && currentExtent !== undefined) {
                            this.addBreadcrumbItem(currentExtent, () => __awaiter(this, void 0, void 0, function* () {
                                this.preSelectExtentByUri = currentExtent;
                                this.selectedItem =
                                    {
                                        workspace: this.getUserSelectedWorkspaceId(),
                                        extentUri: this.getUserSelectedExtentUri(),
                                        ententType: ApiModels_1.EntentType.Extent,
                                        uri: this.getUserSelectedExtentUri()
                                    };
                                yield tthis.loadExtents();
                            }));
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
                                this.addBreadcrumbItem(item.name, () => __awaiter(this, void 0, void 0, function* () {
                                    this.selectedItem = innerItem;
                                    yield tthis.loadItems();
                                }));
                            })(item);
                        }
                    }
                }
            });
        }
        /**
         * Adds a breadcrump item. This is just a helper method for the
         * breadcrumb creation
         * @param text Text to be added
         * @param onClick The event that is meant to be connected to the item
         * @private
         */
        addBreadcrumbItem(text, onClick) {
            const tthis = this;
            const breadcrumbItem = $("<li class='breadcrumb-item active'></li>");
            breadcrumbItem.text(text);
            // Remove all breadcrumb items till that one
            breadcrumbItem.on("click", () => __awaiter(this, void 0, void 0, function* () {
                onClick();
                yield tthis.refreshBreadcrumb();
            }));
            this.htmlBreadcrumbList.append(breadcrumbItem);
        }
    }
    exports.SelectItemControl = SelectItemControl;
});
//# sourceMappingURL=SelectItemControl.js.map