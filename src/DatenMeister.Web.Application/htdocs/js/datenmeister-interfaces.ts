/* Stores all the models that can be returned via one of the */
import * as DMRibbon from "./datenmeister-ribbon";
import * as DMCI from "./datenmeister-clientinterface";

export namespace Views {
    // This interface should be implemented by all views that can be added via 'setView' to a layout
    export interface IView {
        viewport: IViewPort;
        load(): JQuery;
        getViewState(): Api.IViewState;
        
        /// Called, when the user clicks on refresh. The view has to reload the complete dynamic data
        refresh(): void;

        /**
         * Called, if the dom is added to the window
         */
        onViewShown(): void;
    }

    export interface IViewPort {
        setView(view: IView): void;
        navigateBack(): void;
    }
}

export namespace Navigation {
    
    export class FormForItemConfiguration {
        columns: Array<DMCI.In.IFieldData>;

        onOkForm: (data: any) => void;
        onCancelForm: () => void;

        constructor() {
            this.columns = new Array<DMCI.In.IFieldData>();
        }

        addColumn(column: DMCI.In.IFieldData): void {
            this.columns[this.columns.length] = column;
        }
    }

    export class DialogConfiguration extends FormForItemConfiguration {
        /*ws: string;
        ext: string;*/
    }

    export interface IItemViewSettings {
        isReadonly?: boolean;
    }

    export class ItemViewSettings implements IItemViewSettings {
        isReadonly: boolean;
    }
}

export namespace Api {
    /**
     * Defines the interface that will get called, when a table reads additional information from the 
     */
    export interface IItemsProvider {
        performQuery(query: DMCI.Out.IItemTableQuery): JQueryDeferred<DMCI.In.IItemsContent>;
    }

    export interface ILayout {
        throwViewPortChanged(data: ILayoutChangedEvent): void;
        getRibbon(): DMRibbon.Ribbon;
        mainViewPort: Views.IViewPort;
    }

    export enum PageType {
        Workspaces,
        Extents,
        Items,
        ItemDetail,
        Dialog
    }

    export interface IViewState {
        type: PageType;
        workspace?: string;
        extent?: string;
        item?: string;
        isReadonly?: boolean;
        viewname?: string;
    }

    export class ViewState {
        type: PageType;
        workspace: string;
        extent: string;
        item: string;
        isReadonly: boolean;
        viewname: string;
    }

    export interface ILayoutChangedEvent {
        viewState: IViewState;
    }

    /*
     * Stores the event being used when an update of ribbon is required
     */
    export interface IRibbonUpdateEvent {
        layout?: ILayout;
        viewState: IViewState;
    }
}

export namespace Plugin
{

    export class PluginParameter {
        version: string;
        layout: Api.ILayout;
    }

    /**
     * This interface has to be returned by all plugins
     */
    export interface IPluginResult {
        /**
         * Called after a ribbon shall be updated due to a change of the viewport
         * This method will be called by the ApplicationWindow
         */
        onRibbonUpdate?: (ev: Api.IRibbonUpdateEvent) => void;

        /**
         * Returns a function that will be called, if the viewport changes
         * @param ev Parameter containing the information about the changed view set
         * @returns {} The function
         */
        onViewPortChanged?: (ev: Api.ILayoutChangedEvent) => void;
    }
}