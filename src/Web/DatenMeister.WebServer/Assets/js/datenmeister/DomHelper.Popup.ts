import * as Mof from "./Mof.js";

let globalClientActions: Mof.DmObject[] | null = null; 
export function createPopupWindowWithClientActions (clientActions: Mof.DmObject[]) {
}

window["DMgetClientActions"] = () => {
    if (globalClientActions === null || globalClientActions === undefined) {
        throw "The variable globalClientActions is not set";
    }

    const result = globalClientActions;
    globalClientActions = null;
    return result;
}
