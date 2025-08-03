import * as FormFactory from '../forms/FormFactory.js';
import * as ModuleFormLoader from '../forms/DefaultLoader.js';
import * as Mof from '../Mof.js';
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as ClientExtent from "../client/Extents.js";
import * as ClientForms from "../client/Forms.js";
import * as ClientWorkspace from "../client/Workspace.js";
var _ViewMode = _DatenMeister._Forms._ViewMode;
class X {
    constructor() {
        this.type = "X";
    }
    createFormByObject(parent, configuration) {
        return Promise.resolve(undefined);
    }
    async refreshForm() {
    }
    storeFormValuesIntoDom(reuseExistingElement) {
        return Promise.resolve(undefined);
    }
}
class Y {
    constructor() {
        this.type = "Y";
    }
    async refreshForm() {
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
        before(async function () {
            await ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true });
        });
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
        it('Test GetDefaultViewMode', async () => {
            let result = await ClientExtent.createXmi({
                filePath: "./unittests.xmi",
                workspace: "Test",
                extentUri: "dm:///newexisting",
                skipIfExisting: true
            });
            const defaultViewMode = await ClientForms.getDefaultViewMode("Test", "dm:///newexisting");
            chai.assert.isTrue(defaultViewMode !== undefined, "Default ViewMode is not defined");
            chai.assert.isTrue(defaultViewMode.viewMode.get(_ViewMode._name_, Mof.ObjectType.String) === "Default", "ViewMode is not Default ViewMode");
        });
        it('Test GetDefaultViewModes', async () => {
            const viewModes = await ClientForms.getViewModes();
            chai.assert.isTrue(viewModes !== undefined, "ViewMode is not defined");
            chai.assert.isTrue(viewModes.viewModes.length > 0, "Length of ViewModes > 0");
            let found = false;
            for (const n in viewModes.viewModes) {
                const viewMode = viewModes.viewModes[n];
                if (viewMode.get(_ViewMode._name_, Mof.ObjectType.String) === "Default") {
                    found = true;
                }
            }
            chai.assert.isTrue(found, "Default ViewMode has not been found");
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
//# sourceMappingURL=Test.Client.Forms.js.map