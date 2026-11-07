import * as FormActions from "../FormActions.js";
import {FormType, IFormNavigation} from "../forms/Interfaces.js";
import * as Mof from "../Mof.js";
import {SubmitMethod} from "../forms/Forms.js";
import * as FormFactory from "../forms/FormFactory.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as ClientForm from "../client/Forms.js"
import * as ClientItem from "../client/Items.js"
import * as ViewModeLogic from "../forms/ViewModeLogic.js"
import * as ObjectForm from "../forms/ObjectForm.js"
import * as CollectionForm from "../forms/CollectionForm.js"

export function loadModules() {
    FormActions.addModule(new RenderHtml());
    FormActions.addModule(new RenderForm());
    FormActions.addModule(new DownloadHtml());
}

const divIdOfRenderArea = '#pageContent';

class RenderHtml extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Render.Html", _DatenMeister._Actions._ClientActions.__RenderHtmlClientAction_Uri);
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const renderCompletePage = element.get(_DatenMeister._Actions._ClientActions._RenderHtmlClientAction.renderCompletePage, Mof.ObjectType.Boolean);
        const html = element.get(_DatenMeister._Actions._ClientActions._RenderHtmlClientAction.html, Mof.ObjectType.String);
        if(html !== undefined && html !== "" && html !== null) {
            if (renderCompletePage) {
                const blob = new Blob([html], { type: "text/html" });
                window.location.href = URL.createObjectURL(blob);
            } else {
                $(divIdOfRenderArea).html(html);
            }
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

class DownloadHtml extends FormActions.ItemFormActionModuleBase {
    constructor() {
        super("DatenMeister.Download.Html", _DatenMeister._Actions._ClientActions.__DownloadHtmlClientAction_Uri);
    }

    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const html = element.get(_DatenMeister._Actions._ClientActions._DownloadHtmlClientAction.html, Mof.ObjectType.String);
        const filename = element.get(_DatenMeister._Actions._ClientActions._DownloadHtmlClientAction.filename, Mof.ObjectType.String);
        this.downloadHtml(html, filename);
    }

    /**
     * Downloads the HTML as a file
     * @param html Html of the download
     * @param fileName Filename of the download
     */
    downloadHtml(html: string, fileName: string = "download.html"): void {
        // 1. Create a Blob with UTF-8 HTML MIME type
        const blob = new Blob([html], { type: "text/html;charset=utf-8" });

        // 2. Generate a temporary object URL pointing to the Blob
        const url = URL.createObjectURL(blob);

        // 3. Create a temporary anchor element and configure the download
        const anchor = document.createElement("a");
        anchor.href = url;
        anchor.download = fileName;
        anchor.style.display = "none";

        // 4. Append to DOM, trigger download, and remove
        document.body.appendChild(anchor);
        anchor.click();
        document.body.removeChild(anchor);

        // 5. Release memory by revoking the Object URL
        setTimeout(() => URL.revokeObjectURL(url), 100);
    }
}

class RenderForm extends FormActions.ItemFormActionModuleBase
{
    constructor() {
        super("DatenMeister.Render.Form", _DatenMeister._Actions._ClientActions.__RenderFormClientAction_Uri);
    }
    
    async execute(form: IFormNavigation, element: Mof.DmObject, parameter?: Mof.DmObject, submitMethod?: SubmitMethod): Promise<void> {
        const dataWorkspace = element.get(_DatenMeister._Actions._ClientActions._RenderFormClientAction.dataWorkspaceId, Mof.ObjectType.String);
        const dataUri = element.get(_DatenMeister._Actions._ClientActions._RenderFormClientAction.dataUri, Mof.ObjectType.String);
        const formAutogenerate = element.get(_DatenMeister._Actions._ClientActions._RenderFormClientAction.formAutoGenerate, Mof.ObjectType.Boolean);
        const formType = element.get(_DatenMeister._Actions._ClientActions._RenderFormClientAction.formType, Mof.ObjectType.String);
        const detailForm = element.get(_DatenMeister._Actions._ClientActions._RenderFormClientAction.form, Mof.ObjectType.Object);

        // Ok, we have consolidated the information. 
        if (formAutogenerate && detailForm !== undefined) {
            throw new Error("formAutogenerate is set and forms are provided. This does not make sense");
        }

        if (!formAutogenerate && detailForm === undefined) {
            throw new Error("formAutogenerate is not set and no forms are provided. This does not make sense");
        }

        const pageContent = $("#pageContent");

        // Get the autogenerated form
        switch (formType) {
            case _DatenMeister._Forms._FormTypes._FormType.Decoupled:
                if (detailForm === undefined)
                    throw new Error("No support of AutoGenerated forms as decoupled");

                // We have a decoupled form and that means we need to create the form and start rendering
                const formUri = detailForm?.metaClass.uri;
                if (formUri == undefined)
                    throw new Error("Unknown metaclass");

                const decoupledForm = FormFactory.getDecoupledFormFactory(formUri);
                if (decoupledForm === undefined)
                    throw new Error("Unknown decoupled form: " + formUri);

                await decoupledForm({
                    isReadOnly: true,
                    formType: FormType.Object,
                    viewMode: ViewModeLogic.ViewModes.Default
                }).createForm(pageContent, {form: detailForm});
                break;
            case _DatenMeister._Forms._FormTypes._FormType.Object: {
                // Gets the form
                const createdForm = await ClientForm.getObjectFormForItem(
                    dataWorkspace, dataUri, ViewModeLogic.ViewModes.Default);

                if (createdForm === undefined) {
                    throw new Error("Could not load form from server");
                }

                // Gets the element
                const element = await ClientItem.getObjectByUri(dataWorkspace, dataUri);

                // Now creates the form
                const container = new ObjectForm.ObjectFormHtmlElements();
                container.itemContainer = pageContent;

                const objectForm = new ObjectForm.ObjectFormCreator(container);
                objectForm.element = element;
                objectForm.workspace = dataWorkspace;
                await objectForm.createFormByObject(
                    {
                        isReadOnly: true,
                        formType: FormType.Object,
                        viewMode: ViewModeLogic.ViewModes.Default,
                        formElement: createdForm
                    });
                break;
            }

            case _DatenMeister._Forms._FormTypes._FormType.Collection: {
                // Gets the form
                const createdForm = await ClientForm.getCollectionFormForExtent(
                    dataWorkspace, dataUri, ViewModeLogic.ViewModes.Default);

                if (createdForm === undefined) {
                    throw new Error("Could not load form from server");
                }

                // Gets the element

                // Now creates the form

                const container = new CollectionForm.CollectionFormHtmlElements();
                container.itemContainer = pageContent;

                const collectionForm = new CollectionForm.CollectionFormCreator(container);
                collectionForm.workspace = dataWorkspace;
                await collectionForm.createCollectionForRootElements(
                    dataWorkspace,
                    dataUri,
                    {
                        isReadOnly: true,
                        formType: FormType.Collection,
                        viewMode: ViewModeLogic.ViewModes.Default,
                        formElement: createdForm
                    });
                break;
            }
            default:
                throw new Error("Unknown form type: " + formType);
        }
    }
}