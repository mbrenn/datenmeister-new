import * as FormActions from "../FormActions.js"
import * as Mof from "../Mof.js";
import { IFormNavigation, IPageForm } from "../forms/Interfaces.js";
import { SubmitMethod } from "../forms/Forms.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as Navigation from "../Navigator.js";
import {_Actions} from "../models/DatenMeister.class.js";
import * as Settings from "../Settings.js";

export function loadModules() {

    FormActions.addModule(new ChangeForm());
    FormActions.addModule(new CreateNewItem());
    FormActions.addModule(new CreateAction());
    FormActions.addModule(new NavigateToExtent());
    FormActions.addModule(new NavigateToUrl());
    FormActions.addModule(new NavigateOpenWindow());
    FormActions.addModule(new NavigateOpenActionInWindow());
}

/**
 * Moves the browser to the specific url
 */
class NavigateToUrl extends FormActions.ItemFormActionModuleBase
{
    constructor() {
        super("DatenMeister.Navigation.ToUrl",
            _Actions._ClientActions.__NavigateToUrlClientAction_Uri);
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        if(parameter === undefined) throw new Error("Parameter is undefined");
        document.location.href = Settings.baseUrl + parameter.get(_Actions._ClientActions._NavigateToUrlClientAction.url);
    }
}

/**
 * Opens the given url in a new browser window
 */
class NavigateOpenWindow extends FormActions.ItemFormActionModuleBase
{
    constructor() {
        super("DatenMeister.Navigation.OpenWindow",
            _Actions._ClientActions.__NavigateOpenWindow_Uri);
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const url = element.get(_Actions._ClientActions._NavigateOpenWindow.url, Mof.ObjectType.String);
        if (url === undefined || url === null || url === "") throw new Error("Url is not set");
        const isAbsoluteUrl = element.get(_Actions._ClientActions._NavigateOpenWindow.isAbsoluteUrl, Mof.ObjectType.Boolean) === true;

        openWindow({
                url: url,
                isAbsoluteUrl: isAbsoluteUrl,
            }
        );
    }
}

/**
 * Stores the parameter of the window opening
 */
export interface OpenWindowParameter
{
    /**
     * The url website to be opened
     */
    url: string;
    /**
     * Information, if the url is an absolute url. If it is not absolute, the baseurl will be prepended 
     */
    isAbsoluteUrl?: boolean;
}

/**
 * Opens the window according the parameter
 * @param parameter Parameter of the window opening
 */
export function openWindow(parameter: OpenWindowParameter){
    window.open(parameter.isAbsoluteUrl ? parameter.url : Settings.baseUrl + parameter.url, "_blank");
}

class NavigateOpenActionInWindow extends FormActions.ItemFormActionModuleBase
{
    constructor() {
        super("DatenMeister.Navigation.OpenActionInWindow",
            _Actions._ClientActions.__NavigateOpenActionInWindow_Uri);
        this.skipSaving = true;
    }



    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const context = element.get(_Actions._ClientActions._NavigateOpenActionInWindow.context, Mof.ObjectType.String);
        const actionUrl = element.get(_Actions._ClientActions._NavigateOpenActionInWindow.actionUrl, Mof.ObjectType.String);
        const workspace = element.get(_Actions._ClientActions._NavigateOpenActionInWindow.workspaceId, Mof.ObjectType.String);

        const Url = Settings.baseUrl + "ActionPure?actionVerb=" + encodeURIComponent(context)
            + "&workspace=" + encodeURIComponent(workspace)
            + "&actionUrl=" + encodeURIComponent(actionUrl);
        window.open(Url, "_blank");
    }
    
}

class ChangeForm extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Navigation.ChangeForm");
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        if(parameter === undefined) throw new Error("Parameter is undefined");
        const formUrl = parameter.get("formUrl", Mof.ObjectType.String);

        var asFormPage = form as IPageForm;
        asFormPage.pageNavigation.switchFormUrl(formUrl);
    }
}

class CreateNewItem extends FormActions.ItemFormActionModuleBase {

    constructor() {
        super("DatenMeister.Navigation.CreateNewItem");
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
    }
}



class CreateAction extends FormActions.ItemFormActionModuleBase {

    constructor() {
        super("DatenMeister.Navigation.CreateAction");
        this.skipSaving = true;
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        if(parameter === undefined) throw new Error("Parameter is undefined");
        const actionType = parameter.get(_DatenMeister._Actions._ParameterTypes._NavigationDefineActionParameter.actionName, Mof.ObjectType.String);
        const formUrl = parameter.get(_DatenMeister._Actions._ParameterTypes._NavigationDefineActionParameter.formUrl, Mof.ObjectType.String);
        const metaClassUrl = parameter.get(_DatenMeister._Actions._ParameterTypes._NavigationDefineActionParameter.metaClassUrl, Mof.ObjectType.String);

        Navigation.navigateToAction(actionType, formUrl, { workspace: element.workspace, itemUri: element.uri, metaClass: metaClassUrl });
    }
}

class NavigateToExtent extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super(
            "DatenMeister.Navigation.ToExtent",
            _DatenMeister._Actions._ClientActions.__NavigateToExtentClientAction_Uri);
        this.skipSaving = true;
    }
    
    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        Navigation.navigateToExtentProperties(
            element.get(_DatenMeister._Actions._ClientActions._NavigateToExtentClientAction.workspaceId),
            element.get(_DatenMeister._Actions._ClientActions._NavigateToExtentClientAction.extentUri)
        );
    }
}

