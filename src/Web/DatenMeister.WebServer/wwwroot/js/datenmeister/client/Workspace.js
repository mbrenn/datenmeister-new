import * as Settings from "../Settings.js";
import * as ApiConnection from "../ApiConnection.js";
export function createWorkspace(id, annotation, param) {
    return new Promise(resolve => {
        let url = Settings.baseUrl +
            "api/workspace/create";
        const data = {
            id: id,
            annotation: annotation
        };
        if (param?.skipIfExisting !== undefined) {
            data.skipIfExisting = param.skipIfExisting;
        }
        ApiConnection.put(url, data).then((result) => {
            resolve(result);
        });
    });
}
export function deleteWorkspace(id, param) {
    return new Promise(resolve => {
        let url = Settings.baseUrl +
            "api/workspace/delete";
        const data = {
            id: id
        };
        if (param?.skipIfExisting !== undefined) {
            data.skipIfExisting = param.skipIfExisting;
        }
        ApiConnection.deleteRequest(url, data).then((result) => {
            resolve(result);
        });
    });
}
//# sourceMappingURL=Workspace.js.map