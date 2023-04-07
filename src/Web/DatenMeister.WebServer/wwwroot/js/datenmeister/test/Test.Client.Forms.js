define(["require", "exports", "../forms/FormFactory", "../forms/DefaultLoader", "../models/DatenMeister.class"], function (require, exports, FormFactory, ModuleFormLoader, DatenMeister_class_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
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
    function includeTests() {
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
                chai.assert.isTrue(FormFactory.getCollectionFormFactory(DatenMeister_class_1._DatenMeister._Forms.__TableForm_Uri)
                    !== undefined);
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Forms.js.map