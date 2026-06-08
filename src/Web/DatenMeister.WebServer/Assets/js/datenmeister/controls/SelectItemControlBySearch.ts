import {ISelectItemControl} from "./Interfaces.js";
import {UserEvent} from "../../burnsystems/Events.js";
import {ItemWithNameAndId} from "../ApiModels.js";
import * as EL from "../client/Elements.js";


export class ControlSettings {
}

export class SelectItemControlBySearch implements ISelectItemControl {
    itemClicked: UserEvent<ItemWithNameAndId>;
    itemSelected: UserEvent<ItemWithNameAndId>;
    private containerDiv: JQuery<HTMLElement>;

    private inputBoxDiv: JQuery<HTMLElement>;
    private resultsDiv: JQuery<HTMLElement>;
    private htmlWorkspaceSelect: JQuery<HTMLElement>;

    /** Last list of workspaces fetched from the server, used by {@link getSelectedWorkspace}. */
    private loadedWorkspaces: Array<ItemWithNameAndId> = new Array<ItemWithNameAndId>();
    
    /**
     * Workspace id queued for pre-selection. Consumed on the next workspace
     * load and reset to `undefined`. `undefined` means "no pre-selection
     * pending".
     */
    private preSelectWorkspaceById: string | undefined;


    init(parent: JQuery<HTMLElement>, settings?: ControlSettings): JQuery {

        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        const div = this.initDom(settings, parent);

        const _ = this.loadWorkspaces();
        return div;
    }
    async initAsync(parent: JQuery<HTMLElement>, settings?: ControlSettings): Promise<JQuery> {

        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        const div = this.initDom(settings, parent);
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
    private initDom(settings: ControlSettings, container: JQuery<HTMLElement>) {

        const tthis = this;
        // Creates the template
        const div = $(
            "<div class='dm-sic-search'>" +
            "<div class='dm-sic-search-workspace'></div>" +
            "</div>" +
            "<div class='dm-sic-search-input'>" +
            "<input type='text' />" +
            "</div>" +
            "<div class='dm-sic-search-results'></div>" + 
            "</div>");
        
        this.inputBoxDiv = $(".dm-sic-search-input input", div);
        this.resultsDiv = $(".dm-sic-search-results", div);

        this.htmlWorkspaceSelect = $("<select></select>");
        this.htmlWorkspaceSelect.on("change", () => tthis.onWorkspaceChangedByUser());
        $("dm-sic-search-workspace", div).append(this.htmlWorkspaceSelect);
        
        this.inputBoxDiv.on("input", () => tthis.onInputChanged());

        container.append(div);
        this.containerDiv = div;
        return div;
    }

    /**
     * Returns the workspace id currently selected in the dropdown, or the
     * empty string if no workspace is selected. This reflects the *DOM* state
     * — pending pre-selections that have not been applied yet are not visible
     * here.
     */
    getUserSelectedWorkspaceId(): string {
        return this.htmlWorkspaceSelect.val()?.toString() ?? "";
    }

    onWorkspaceChangedByUser(): any {
        throw new Error("Method not implemented.");
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
    }

    showControl()
    {
        this.containerDiv.show();
    }

    hideControl()
    {
        this.containerDiv.hide();
    }
    
    getSelectedItem(): ItemWithNameAndId {
        return undefined;
    }

    setExtentByUri(workspaceId: string, extentUri: string): Promise<void> {
        return Promise.resolve(undefined);
    }

    setItemByUri(workspaceId: string, itemUri: string): Promise<void> {
        return Promise.resolve(undefined);
    }

    setWorkspaceById(workspaceId: string): Promise<void> {
        return Promise.resolve(undefined);
    }

    lastLoadIndex = 0;
    private async onInputChanged() {
        
        const loadIndex = this.lastLoadIndex;
        this.lastLoadIndex++;
        
        const text = this.inputBoxDiv.val();
        if(text === "" || text === null || text === undefined)
        {
            this.resultsDiv.hide();
        }
        else
        {
            this.resultsDiv.show();
        }
    }
}