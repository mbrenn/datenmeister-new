import * as Mof from "./Mof.js";
import * as Settings from "./Settings.js";

let count = 0;
let globalClientActions: Mof.DmObject[][] = [];

export const urlParameterName = "executeClientActions"

/**
 * Opens a new popup and stores the clientactions into the global variable, so it can 
 * be retrieved by the popup window. 
 * @param clientActions The clientactions to be executed in the popup window.
 */
export function createPopupWindowWithClientActions (clientActions: Mof.DmObject[]) {
    count++;
    globalClientActions[count] = clientActions;    
    
    window.open(`${Settings.baseUrl}ActionPure?${urlParameterName}=${count}`);
    
    return count;
}

window["DMgetClientActions"] = (index: number) => {
    if (globalClientActions === null || globalClientActions === undefined) {
        throw "The variable globalClientActions is not set";
    }

    return globalClientActions[index];
}

/**
 * Gets the client actions from the opener window
 * @param index This is the index to be used to retrieve the items as returned by 
 * createPopupWindowWithClientActions
 */
export function getClientActionsFromOpener(index: number)
{
    return window.opener["DMgetClientActions"](index);
}