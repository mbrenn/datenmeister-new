import {DmObject} from "./Mof";
import * as Settings from "./Settings";

export function getItemDetailUri(element: DmObject) {
    return Settings.baseUrl + "Item/"
        + encodeURIComponent(element.workspace) + "/"
        + encodeURIComponent(element.uri);
}