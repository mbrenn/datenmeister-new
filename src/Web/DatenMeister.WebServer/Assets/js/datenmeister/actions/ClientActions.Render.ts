import * as FormActions from "../FormActions.js";
import {IFormNavigation} from "../forms/Interfaces.js";
import * as Mof from "../Mof.js";
import {SubmitMethod} from "../forms/Forms.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";

export function loadModules() {
    FormActions.addModule(new RenderHtml());
}

const divIdOfRenderArea = '#pageContent';

class RenderHtml extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Render.Html", _DatenMeister._Actions._ClientActions.__RenderHtmlClientAction_Uri);
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const html = element.get(_DatenMeister._Actions._ClientActions._RenderHtmlClientAction.html, Mof.ObjectType.String);
        if(html !== undefined && html !== "" && html !== null)
        {
            $(divIdOfRenderArea).html(html);
            return;
        }
        
        const text = element.get(_DatenMeister._Actions._ClientActions._RenderHtmlClientAction.text, Mof.ObjectType.String);
        if(text !== undefined && text !== "" && text !== null)
        {
            $(divIdOfRenderArea).text(text);
            return;
        }
        
        $(divIdOfRenderArea).text("ClientActions.RenderHtml: Neither html nor text was set");
    }
}