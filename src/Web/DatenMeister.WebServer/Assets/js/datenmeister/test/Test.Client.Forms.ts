import * as FormFactory from '../forms/FormFactory.js'
import * as FieldFactory from '../forms/FieldFactory.js'
import * as IForm from '../forms/Interfaces.js'
import {FormType} from '../forms/Interfaces.js'
import * as FormConfiguration from '../forms/IFormConfiguration.js'
import * as ModuleFormLoader from '../forms/DefaultLoader.js'
import * as Mof from '../Mof.js'
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as ClientExtent from "../client/Extents.js";
import * as ClientForms from "../client/Forms.js";
import * as ClientWorkspace from "../client/Workspace.js";
import {BaseField, IFieldRenderContext, IFormField} from "../fields/Interfaces.js";

import '../../node_modules/chai/register-assert.js';
import _ViewMode = _DatenMeister._Forms._ViewMode;

declare var assert: Chai.AssertStatic;

class X implements IForm.IObjectForm {
    pageNavigation: IForm.IPageNavigation;
    element: Mof.DmObject;
    extentUri: string;
    formElement: Mof.DmObject;
    itemUrl: string;
    workspace: string;
    type: string = "X";

    getFormElement(): Mof.DmObject {
        return this.formElement;
    }


    createFormByObject(parent: JQuery<HTMLElement>): Promise<void> {
        return Promise.resolve(undefined);
    }

    async refreshForm(): Promise<void> {
    }

    storeFormValuesIntoDom(reuseExistingElement?: boolean): Promise<Mof.DmObject> {
        this.element = new Mof.DmObject();
        return Promise.resolve(this.element);
    }    
}

class Y implements IForm.ICollectionForm {
    callbackLoadItems: (query: Mof.DmObject) => Promise<Array<Mof.DmObject>>;
    pageNavigation: IForm.IPageNavigation;
    element: Mof.DmObject;
    extentUri: string;
    formElement: Mof.DmObject;
    itemUrl: string;
    workspace: string;

    type: string = "Y";
    
    getFormElement(): Mof.DmObject {
        return this.formElement;
    }

    async refreshForm(): Promise<void> {
    }

    storeFormValuesIntoDom(reuseExistingElement?: boolean): Promise<Mof.DmObject> {
        this.element = new Mof.DmObject();
        return Promise.resolve(this.element);
    }

    elements: Array<Mof.DmObject>;

    createFormByCollection(parent: JQuery<HTMLElement>): Promise<void> {
        return Promise.resolve(undefined);
    }
    
    setInfoText(message: string): void {
        throw new Error("Method not implemented.")
    }

}

class CustomTestField extends BaseField implements IFormField {
    createdCount = 0;
    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {
        this.createdCount++;
        return $("<div>CustomField</div>");
    }
    async evaluateDom(dmElement: Mof.DmObject): Promise<void> {
    }
}

export function includeTests() {
    describe('Forms', () => {

        before(async function () {
            await ClientWorkspace.createWorkspace(
                "Test",
                "Annotation",
                {skipIfExisting: true});
        });
        
        it('Test Register Database', () => {
            FormFactory.registerCollectionForm("collectionForm", (c) => new Y());
            FormFactory.registerObjectForm("objectForm", (c) => new X());

            assert.isTrue(FormFactory.getObjectFormFactory("no") === undefined);
            assert.isTrue(FormFactory.getCollectionFormFactory("no") === undefined);
            assert.isTrue(FormFactory.getObjectFormFactory("objectForm") !== undefined);
            assert.isTrue(FormFactory.getCollectionFormFactory("collectionForm") !== undefined);
            const objectFormFactory = FormFactory.getObjectFormFactory("objectForm");
            assert.isTrue(objectFormFactory !== undefined);
            assert.isTrue((objectFormFactory!(
                {
                    isReadOnly: false,
                    formType: FormType.Object
                }) as X).type === "X");

            const collectionFormFactory = FormFactory.getCollectionFormFactory("collectionForm");
            assert.isTrue(collectionFormFactory !== undefined);
            assert.isTrue((collectionFormFactory!(
                {
                    isReadOnly: false,
                    formType: FormType.Collection
                }) as Y).type === "Y");
        });

        it('Test FieldFactory Registration and Context Creation', () => {
            const customUri = "dm:///_custom/TestField";
            FieldFactory.registerField(customUri, ctx => new CustomTestField(ctx));

            const fieldDef = new Mof.DmObject();
            fieldDef.setMetaClassByUri(customUri, "Types");
            fieldDef.set("name", "customProp");

            const context: IFieldRenderContext = {
                field: fieldDef,
                isReadOnly: true,
                itemUrl: "dm:///test#1",
                configuration: {
                    isReadOnly: true,
                    formType: FormType.Object
                }
            };

            const created1 = FieldFactory.createField(customUri, context);
            assert.isTrue(created1 instanceof CustomTestField);
            assert.isTrue(created1.isReadOnly === true);
            assert.isTrue(created1.itemUrl === "dm:///test#1");

            const created2 = FieldFactory.createField(context);
            assert.isTrue(created2 instanceof CustomTestField);
            assert.isTrue(created2.isReadOnly === true);
        });

        it('Test Default Database', () => {
            ModuleFormLoader.loadDefaultForms();
            assert.isTrue(
                FormFactory.getCollectionFormFactory(_DatenMeister._Forms._FormTypes.__TableForm_Uri)
                !== undefined);
        });
        
        it('Test GetDefaultViewMode', async () => {

            let result = await ClientExtent.createXmi(
                {
                    filePath: "./unittests.xmi",
                    workspace: "Test",
                    extentUri: "dm:///newexisting",
                    skipIfExisting: true
                });

            const defaultViewMode = await ClientForms.getDefaultViewMode("Test", "dm:///newexisting");
            assert.isTrue(defaultViewMode !== undefined, "Default ViewMode is not defined");
            assert.isTrue(defaultViewMode.viewMode.get(_ViewMode._name_, Mof.ObjectType.String) === "Default", "ViewMode is not Default ViewMode");
        });


        it('Test GetDefaultViewModes', async () => {

            const viewModes = await ClientForms.getViewModes();
            assert.isTrue(viewModes !== undefined, "ViewMode is not defined");
            assert.isTrue(viewModes.viewModes.length > 0, "Length of ViewModes > 0");
            
            let found = false;
            for (const n in viewModes.viewModes)
            {
                const viewMode = viewModes.viewModes[n];
                if ( viewMode.get(_ViewMode._name_, Mof.ObjectType.String) === "Default")
                {
                    found = true;
                }
            }
            
            assert.isTrue(found, "Default ViewMode has not been found");
        });
        
        after(async function () {

            await ClientExtent.deleteExtent({
                workspace: "Test",
                extentUri: "dm:///newexisting",
                skipIfNotExisting: true
            });

            await ClientWorkspace.deleteWorkspace("Test");
        });
    });
}
