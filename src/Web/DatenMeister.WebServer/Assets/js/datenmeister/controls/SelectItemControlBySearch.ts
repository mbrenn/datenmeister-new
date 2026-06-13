import {ISelectItemControl} from "./Interfaces.js";
import {UserEvent} from "../../burnsystems/Events.js";
import {ItemWithNameAndId} from "../ApiModels.js";
import * as Mof from "../Mof.js"
import * as EL from "../client/Elements.js";
import * as QueryEngine from "../modules/QueryEngine.js";
import * as ClientElements from "../client/Elements.js";
import * as DomHelper from "../DomHelper.js";
import {DmObject} from "../Mof.js";


export class ControlSettings {
}

export class SelectItemControlBySearch implements ISelectItemControl {
    itemClicked: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>();
    itemSelected: UserEvent<ItemWithNameAndId> = new UserEvent<ItemWithNameAndId>;
    private containerDiv: JQuery<HTMLElement>;

    private inputBoxDiv: JQuery<HTMLElement>;
    private resultsDiv: JQuery<HTMLElement>;
    private htmlWorkspaceSelect: JQuery<HTMLElement>;
    
    private selectedItem: DmObject;

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
            "<table>" +
            "<tr><td><span>Workspace:</span></td><td><div class='dm-sic-search-workspace'></div></td></tr>" +
            "<tr><td><span>Searchtext:</span></td>" +
            "<td><div class='dm-sic-search-input'><input type='text' /></div></td></tr>" +
            "<tr><td colspan='2'><div class='dm-sic-search-results'></div></td></tr>" + 
            "</table></div>");
        
        this.inputBoxDiv = $(".dm-sic-search-input input", div);
        this.resultsDiv = $(".dm-sic-search-results", div);
        const workspaceDiv = $(".dm-sic-search-workspace", div);        
        
        this.htmlWorkspaceSelect = $("<select></select>");
        this.htmlWorkspaceSelect.on("change", () => tthis.onWorkspaceChangedByUser());
        $("dm-sic-search-workspace", div).append(this.htmlWorkspaceSelect);
        workspaceDiv.append(this.htmlWorkspaceSelect);
        
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
        this.inputBoxDiv.trigger('focus');        
        this.inputBoxDiv.val('');
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
    lastSelectedDiv: JQuery<HTMLElement>;
    private async onInputChanged() {

        const tthis = this;
        this.lastLoadIndex++;
        const loadIndex = this.lastLoadIndex;
        
        const selectWorkspace = this.getUserSelectedWorkspaceId();
        const text = this.inputBoxDiv.val().toString();
        if(text === "" || text === null || text === undefined)
        {
            this.resultsDiv.empty();
            this.resultsDiv.hide();
        } 
        else if (selectWorkspace === undefined || selectWorkspace === "")
        {
            this.resultsDiv.empty();
            this.resultsDiv.append("<div>Please select a workspace</div>");
            this.resultsDiv.show();
        }
        else
        {
            // Now load the items with the given freetext name
            const queryBuilder = new QueryEngine.QueryBuilder();
            QueryEngine.getElementsOfWorkspace(queryBuilder, selectWorkspace);
            QueryEngine.flatten(queryBuilder);
            QueryEngine.filterByFreetext(queryBuilder, text, "name");
            QueryEngine.limit(queryBuilder, 100);
            
            const result = await ClientElements.queryObject(
                queryBuilder.queryStatement
            );
            
           if(this.lastLoadIndex == loadIndex) {
               this.resultsDiv.empty();
               for (const n in result.result) {
                   if (!result.result.hasOwnProperty(n)) continue;

                   const item = result.result[n];
                   const itemDiv = $("<div>" + item.get("name", Mof.ObjectType.String) + "</div>");
                   const _ = DomHelper.injectNameByObject(itemDiv, item,
                       {
                           inhibitItemLink: true
                       });


                   ((innerItem) =>
                       itemDiv.on('click', () => {
                           if(this.lastSelectedDiv !== undefined) {
                               tthis.lastSelectedDiv.removeClass('selected');
                           }
                           
                           itemDiv.addClass('selected');
                           tthis.lastSelectedDiv = itemDiv;

                           tthis.selectedItem = innerItem;
                           tthis.itemClicked.invoke(innerItem);
                           alert(item.id);
                       }))(item);

                   this.resultsDiv.append(itemDiv);
               }
           }
            
            this.resultsDiv.show();
        }
    }
}