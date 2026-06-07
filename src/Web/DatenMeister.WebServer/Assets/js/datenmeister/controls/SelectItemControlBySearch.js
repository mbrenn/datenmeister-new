export class ControlSettings {
}
export class SelectItemControlBySearch {
    itemClicked;
    itemSelected;
    containerDiv;
    init(parent, settings) {
        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        return this.initDom(settings, parent);
    }
    async initAsync(parent, settings) {
        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        return this.initDom(settings, parent);
    }
    /**
     * This method just creates the DOM and connects the events of the elements to the
     * invocation methods
     * @param settings Settings to be used
     * @param container JQuery-Container Element hosting the content
     * @private
     */
    initDom(settings, container) {
        // Creates the template
        const div = $("<div class='dm-sic-search'>WE ARE SEARCHING</div>");
        container.append(div);
        this.containerDiv = div;
        return div;
    }
    showControl() {
        this.containerDiv.show();
    }
    hideControl() {
        this.containerDiv.hide();
    }
    getSelectedItem() {
        return undefined;
    }
    setExtentByUri(workspaceId, extentUri) {
        return Promise.resolve(undefined);
    }
    setItemByUri(workspaceId, itemUri) {
        return Promise.resolve(undefined);
    }
    setWorkspaceById(workspaceId) {
        return Promise.resolve(undefined);
    }
}
//# sourceMappingURL=SelectItemControlBySearch.js.map