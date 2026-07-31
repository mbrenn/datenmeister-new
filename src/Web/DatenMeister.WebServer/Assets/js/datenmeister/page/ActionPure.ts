import * as FormActions from "../FormActions.js"
import * as ActionClient from "../client/Actions.js"
import * as Mof from "../Mof.js";
import { loadDefaultModules } from "../actions/DefaultLoader.js";

/**
 * Initializes the ActionPure page.
 *
 * The page simply invokes the given server-side action and lets the returned
 * ClientActions render the actual UI into the `pageContent` div.
 *
 * The action receives a `actionVerb` string parameter, e.g. "render", which lets
 * the same action produce different result types depending on the calling
 * situation.
 */
export async function init(
    actionVerb: string,
    workspace: string,
    actionUrl: string): Promise<void> {

    loadDefaultModules();

    window.document.title = "Action - Der DatenMeister";

    if (workspace !== "" && actionUrl !== "") {        
        await executeReferencedAction(actionVerb, workspace, actionUrl);
    }
    else
    {   
        const pageContentDiv = $("#pageContent");
        pageContentDiv.empty();
        pageContentDiv.text('Workspace or actionUrl is missing');
    }
}

/**
 * Fetches an action by workspace + itemUri and executes it directly on the server.
 * ClientActions returned by the server are dispatched to their handlers.
 */
async function executeReferencedAction(
    actionVerb: string,
    workspace: string,
    itemUrl: string): Promise<void> {

    try {
        const result = await ActionClient.executeAction(workspace, itemUrl);

        if (!result.success) {
            renderError(result.reason);
            return;
        }

        await FormActions.executeClientActionResult(result.resultAsDmObject);
    } catch (exception) {
        renderError(String(exception));
    }
}

/**
 * Executes an action by name via /api/action/execute_directly with a small
 * synthetic parameter that carries the request actionVerb.
 */
async function executeActionByName(actionName: string, actionVerb: string): Promise<void> {
    const parameter = new Mof.DmObject();
    parameter.set("actionName", actionName);
    parameter.set("actionVerb", actionVerb);

    try {
        const result = await ActionClient.executeActionDirectly("Execute", { parameter });
        if (!result.success) {
            renderError(result.reason);
            return;
        }

        await FormActions.executeClientActionResult(result.resultAsDmObject);
    } catch (exception) {
        renderError(String(exception));
    }
}
function renderError(reason: string): void {
    const container = $("#pageContent");
    container.empty();
    container.append($("<div class='alert alert-danger'></div>").text(reason));
}

function renderMessage(message: string): void {
    $("#pageContent").text(message);
}

