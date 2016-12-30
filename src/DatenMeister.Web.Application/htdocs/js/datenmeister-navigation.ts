
import DMI = require("./datenmeister-interfaces");
import DMViewport = require("./datenmeister-viewport");

export class FormForItemConfiguration {
    columns: Array<DMI.ClientResponse.IFieldData>;

    onOkForm: (data: any) => void;
    onCancelForm: () => void;

    constructor() {
        this.columns = new Array<DMI.ClientResponse.IFieldData>();
    }

    addColumn(column: DMI.ClientResponse.IFieldData): void {
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
    navigateToView(navigationView: DMViewport.IView): void;

    /**
     * Sets the status within the current navigation view that can be navigated
     * @param statusDom Statusinformation that can be set
     */
    setStatus(statusDom: JQuery): void;
    
}