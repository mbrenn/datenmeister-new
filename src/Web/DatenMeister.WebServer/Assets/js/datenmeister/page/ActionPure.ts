import * as FormActions from "../FormActions.js"
import * as ActionClient from "../client/Actions.js"
import * as Mof from "../Mof.js";
import { loadDefaultModules } from "../actions/DefaultLoader.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";

/**
 * Initializes the ActionPure page.
 *
 * The page simply invokes the given server-side action and lets the returned
 * ClientActions render the actual UI into the `pageContent` div.
 *
 * The action receives a `context` string parameter, e.g. "render", which lets
 * the same action produce different result types depending on the calling
 * situation.
 */
export async function init(
    context: string,
    workspace: string,
    actionUrl: string): Promise<void> {

    loadDefaultModules();

    window.document.title = "Action - Der DatenMeister";

    if (workspace !== "" && actionUrl !== "") {        
        await executeReferencedAction(context, workspace, actionUrl);
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
    context: string,
    workspace: string,
    itemUrl: string): Promise<void> {

    try {
        const result = await ActionClient.executeAction(workspace, itemUrl);

        if (!result.success) {
            renderError(result.reason);
            return;
        }

        await dispatchClientActions(result.resultAsDmObject);
    } catch (exception) {
        renderError(String(exception));
    }
}

/**
 * Executes an action by name via /api/action/execute_directly with a small
 * synthetic parameter that carries the request context.
 */
async function executeActionByName(actionName: string, context: string): Promise<void> {
    const parameter = new Mof.DmObject();
    parameter.set("actionName", actionName);
    parameter.set("context", context);

    try {
        const result = await ActionClient.executeActionDirectly("Execute", { parameter });
        if (!result.success) {
            renderError(result.reason);
            return;
        }

        await dispatchClientActions(result.resultAsDmObject);
    } catch (exception) {
        renderError(String(exception));
    }
}

async function dispatchClientActions(actionResult: Mof.DmObject | undefined): Promise<void> {
    if (actionResult === undefined) {
        renderMessage("Action executed. No further client actions were returned.");
        return;
    }
    
    if(actionResult.metaClass.uri !== _DatenMeister._Actions.__ActionResult_Uri)
    {
        renderError("The action result is not of type ActionResult");
        return;
    }

    const clientActions = actionResult.get(
        _DatenMeister._Actions._ActionResult.clientActions,
        Mof.ObjectType.Array);

    if (clientActions === undefined || clientActions.length === 0) {
        renderMessage("Action executed on server. No client actions were returned.");
        return;
    }

    for (const n in clientActions) {
        const clientAction = clientActions[n] as Mof.DmObject;  
        await FormActions.executeClientAction(clientAction);
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
