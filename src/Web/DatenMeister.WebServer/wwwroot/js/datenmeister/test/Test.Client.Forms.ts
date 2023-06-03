import * as FormFactory from '../forms/FormFactory.js'
import * as IForm from '../forms/Interfaces.js'
import * as FormConfiguration from '../forms/IFormConfiguration.js'
import * as ModuleFormLoader from '../forms/DefaultLoader.js'
import * as Mof from '../Mof.js'
import {_DatenMeister} from "../models/DatenMeister.class.js";

class X implements IForm.IObjectFormElement {
    element: Mof.DmObject;
    extentUri: string;
    formElement: Mof.DmObject;
    formType: IForm.FormType;
    itemUrl: string;
    workspace: string;
    type: string = "X";

    createFormByObject(parent: JQuery<HTMLElement>, configuration: FormConfiguration.IFormConfiguration): Promise<void> {
        return Promise.resolve(undefined);
    }

    refreshForm(): void {
    }

    storeFormValuesIntoDom(reuseExistingElement?: boolean): Promise<Mof.DmObject> {
        return Promise.resolve(undefined);
    }    
}

class Y implements IForm.ICollectionFormElement {
    element: Mof.DmObject;
    extentUri: string;
    formElement: Mof.DmObject;
    formType: IForm.FormType;
    itemUrl: string;
    workspace: string;

    type: string = "Y";
    refreshForm(): void {
    }

    storeFormValuesIntoDom(reuseExistingElement?: boolean): Promise<Mof.DmObject> {
        return Promise.resolve(undefined);
    }

    elements: Array<Mof.DmObject>;

    createFormByCollection(parent: JQuery<HTMLElement>, configuration: FormConfiguration.IFormConfiguration, refresh?: boolean): Promise<void> {
        return Promise.resolve(undefined);
    }
}

export function includeTests() {
    describe('Forms', () => {
        it('Test Register Database', () => {
            FormFactory.registerCollectionForm("collectionForm", () => new Y());
            FormFactory.registerObjectForm("objectForm", () => new X());

            chai.assert.isTrue(FormFactory.getObjectFormFactory("no") === undefined);
            chai.assert.isTrue(FormFactory.getCollectionFormFactory("no") === undefined);
            chai.assert.isTrue(FormFactory.getObjectFormFactory("objectForm") !== undefined);
            chai.assert.isTrue(FormFactory.getCollectionFormFactory("collectionForm") !== undefined);
            chai.assert.isTrue((FormFactory.getObjectFormFactory("objectForm")() as X).type === "X");
            chai.assert.isTrue((FormFactory.getCollectionFormFactory("collectionForm")() as Y).type === "Y");
        });

        it('Test Default Database', () => {
            ModuleFormLoader.loadDefaultForms();
            chai.assert.isTrue(
                FormFactory.getCollectionFormFactory(_DatenMeister._Forms.__TableForm_Uri)
                !== undefined);
        });
    });
}