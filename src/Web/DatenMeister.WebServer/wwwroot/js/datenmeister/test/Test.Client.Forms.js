import * as FormFactory from '../forms/FormFactory.js';
import * as ModuleFormLoader from '../forms/DefaultLoader.js';
import { _DatenMeister } from "../models/DatenMeister.class.js";
class X {
    constructor() {
        this.type = "X";
    }
    createFormByObject(parent, configuration) {
        return Promise.resolve(undefined);
    }
    refreshForm() {
    }
    storeFormValuesIntoDom(reuseExistingElement) {
        return Promise.resolve(undefined);
    }
}
class Y {
    constructor() {
        this.type = "Y";
    }
    refreshForm() {
    }
    storeFormValuesIntoDom(reuseExistingElement) {
        return Promise.resolve(undefined);
    }
    createFormByCollection(parent, configuration, refresh) {
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
            chai.assert.isTrue(FormFactory.getObjectFormFactory("objectForm")().type === "X");
            chai.assert.isTrue(FormFactory.getCollectionFormFactory("collectionForm")().type === "Y");
        });
        it('Test Default Database', () => {
            ModuleFormLoader.loadDefaultForms();
            chai.assert.isTrue(FormFactory.getCollectionFormFactory(_DatenMeister._Forms.__TableForm_Uri)
                !== undefined);
        });
    });
}
//# sourceMappingURL=Test.Client.Forms.js.map