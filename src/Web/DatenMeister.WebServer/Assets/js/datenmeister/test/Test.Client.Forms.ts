import * as FormFactory from '../forms/FormFactory.js'
import * as IForm from '../forms/Interfaces.js'
import * as FormConfiguration from '../forms/IFormConfiguration.js'
import * as ModuleFormLoader from '../forms/DefaultLoader.js'
import * as Mof from '../Mof.js'
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as ClientExtent from "../client/Extents.js";
import * as ClientForms from "../client/Forms.js";
import * as ClientWorkspace from "../client/Workspace.js";
import _ViewMode = _DatenMeister._Forms._ViewMode;


import '../../node_modules/chai/register-assert.js';
declare var assert: Chai.AssertStatic;

class X implements IForm.IObjectForm {
    pageNavigation: IForm.IPageNavigation;
    element: Mof.DmObject;
    extentUri: string;
    formElement: Mof.DmObject;
    itemUrl: string;
    workspace: string;
    type: string = "X";

    createFormByObject(parent: JQuery<HTMLElement>, configuration: FormConfiguration.IFormConfiguration): Promise<void> {
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
    async refreshForm(): Promise<void> {
    }

    storeFormValuesIntoDom(reuseExistingElement?: boolean): Promise<Mof.DmObject> {
        this.element = new Mof.DmObject();
        return Promise.resolve(this.element);
    }

    elements: Array<Mof.DmObject>;

    createFormByCollection(parent: JQuery<HTMLElement>, configuration: FormConfiguration.IFormConfiguration, refresh?: boolean): Promise<void> {
        return Promise.resolve(undefined);
    }
    
    setInfoText(message: string): void {
        throw new Error("Method not implemented.")
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
            FormFactory.registerCollectionForm("collectionForm", () => new Y());
            FormFactory.registerObjectForm("objectForm", () => new X());

            assert.isTrue(FormFactory.getObjectFormFactory("no") === undefined);
            assert.isTrue(FormFactory.getCollectionFormFactory("no") === undefined);
            assert.isTrue(FormFactory.getObjectFormFactory("objectForm") !== undefined);
            assert.isTrue(FormFactory.getCollectionFormFactory("collectionForm") !== undefined);
            const objectFormFactory =  FormFactory.getObjectFormFactory("objectForm");
            assert.isTrue(objectFormFactory !== undefined);
            assert.isTrue((objectFormFactory!() as X).type === "X");
            
            const collectionFormFactory =  FormFactory.getCollectionFormFactory("collectionForm");
            assert.isTrue(collectionFormFactory !== undefined);
            assert.isTrue((collectionFormFactory!() as Y).type === "Y");
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