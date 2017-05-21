﻿/* Stores all the models that can be returned via one of the */
import * as DMRibbon from "./datenmeister-ribbon";
import * as DMCI from "./datenmeister-clientinterface";

export namespace Views {
    // This interface should be implemented by all views that can be added via 'setView' to a layout
    export interface IView {
        viewport: IViewPort;
        getContent(): JQuery;
        getViewState(): Api.IViewState;
    }

    export interface IViewPort {
        setView(view: IView): void;
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
        ws: string;
        ext: string;
    }

    export interface IItemViewSettings {
        isReadonly?: boolean;
    }

    export class ItemViewSettings implements IItemViewSettings {
        isReadonly: boolean;
    }

    export interface INavigation {
        navigateToWorkspaces(): void;
        navigateToExtents(workspaceId: string): void;
        navigateToItems(ws: string, extentUrl: string, viewname?: string): void;
        navigateToItem(ws: string, extentUrl: string, itemUrl: string, viewname?: string, settings?: IItemViewSettings);
        navigateToDialog(configuration: DialogConfiguration): void;
        navigateToView(navigationView: Views.IView): void;

        /**
         * Sets the status within the current navigation view that can be navigated
         * @param statusDom Statusinformation that can be set
         */
        setStatus(statusDom: JQuery): void;

        refresh();
    
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
    }

    export class ViewState {
        type: PageType;
        workspace: string;
        extent: string;
        item: string;
    }

    export interface ILayoutChangedEvent {
        navigation?: Navigation.INavigation; // Will be set by the thrower
        viewState: IViewState;
    }

    /*
     * Stores the event being used when an update of ribbon is required
     */
    export interface IRibbonUpdateEvent {
        navigation?: Navigation.INavigation; // Will be set by the thrower
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