import {ISelectItemControl} from "./Interfaces.js";
import {UserEvent} from "../../burnsystems/Events.js";
import {ItemWithNameAndId} from "../ApiModels.js";


export class ControlSettings {
}

export class SelectItemControlBySearch implements ISelectItemControl {
    itemClicked: UserEvent<ItemWithNameAndId>;
    itemSelected: UserEvent<ItemWithNameAndId>;
    private containerDiv: JQuery<HTMLElement>;
    
    init(parent: JQuery<HTMLElement>, settings?: ControlSettings): JQuery {

        // Performs the initialization of the DOM, providing all elements
        // and event handlers
        return this.initDom(settings, parent);
    }
    async initAsync(parent: JQuery<HTMLElement>, settings?: ControlSettings): Promise<JQuery> {

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
    private initDom(settings: ControlSettings, container: JQuery<HTMLElement>) {

        // Creates the template
        const div = $(
            "<div class='dm-sic-search'>WE ARE SEARCHING</div>");

        container.append(div);
        this.containerDiv = div;
        return div;
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
    
}