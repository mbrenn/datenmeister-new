var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Extents", "../client/Workspace", "../client/Items", "../client/Forms"], function (require, exports, ClientExtent, ClientWorkspace, ClientItems, ClientForms) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Forms', () => {
            let itemUri;
            before(function () {
                return __awaiter(this, void 0, void 0, function* () {
                    yield ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true });
                    yield ClientExtent.createXmi({
                        extentUri: "dm:///unittest",
                        filePath: "./unittest.xmi",
                        workspace: "Test",
                        skipIfExisting: true
                    });
                    yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                    const createdItem = yield ClientItems.createItemInExtent('Test', 'dm:///unittest', {});
                    itemUri = 'dm:///unittest#' + createdItem.itemId;
                    yield ClientItems.setProperty('Test', itemUri, 'name', 'NamedElement');
                    const createdChild = yield ClientItems.createItemAsChild('Test', itemUri, { property: 'packagedElement' });
                    const childItem = 'dm:///unittest#' + createdChild.itemId;
                    yield ClientItems.setProperty('Test', childItem, 'name', 'ChildElement');
                });
            });
            it('Load Default Form for Extents', () => __awaiter(this, void 0, void 0, function* () {
                const form = yield ClientForms.getDefaultFormForExtent('Test', 'dm:///unittest', '');
                chai.assert.isTrue(form !== undefined, 'Form was not found');
                const tab = form.get('tab');
                chai.assert.isTrue(tab !== undefined, 'tabs are not defined');
                chai.assert.isTrue(Array.isArray(tab), 'Tabs is not an array');
                const firstTab = tab[0];
                chai.assert.isTrue(firstTab !== undefined, 'First tab is not defined');
                const fields = firstTab.get('field');
                chai.assert.isTrue(fields !== undefined, 'fields are not defined');
                chai.assert.isTrue(Array.isArray(fields), 'fields is not an array');
                // Check, if any of the field has a name 
                let found = false;
                for (let n in fields) {
                    const field = fields[n];
                    if (field.get('name') === 'name') {
                        found = true;
                    }
                }
                chai.assert.isTrue(found, 'Field with name was not found');
            }));
            it('Load Default Form for Detail', () => __awaiter(this, void 0, void 0, function* () {
                const form = yield ClientForms.getDefaultFormForItem('Test', itemUri, '');
                chai.assert.isTrue(form !== undefined, 'Form was not found');
                const tab = form.get('tab');
                chai.assert.isTrue(tab !== undefined, 'tabs are not defined');
                chai.assert.isTrue(Array.isArray(tab), 'Tabs is not an array');
                const firstTab = tab[0];
                chai.assert.isTrue(firstTab !== undefined, 'First tab is not defined');
                const fields = firstTab.get('field');
                chai.assert.isTrue(fields !== undefined, 'fields are not defined');
                chai.assert.isTrue(Array.isArray(fields), 'fields is not an array');
                // Check, if any of the field has a name 
                let found = false;
                for (let n in fields) {
                    const field = fields[n];
                    if (field.get('name') === 'name') {
                        found = true;
                    }
                }
                chai.assert.isTrue(found, 'Field with name was not found');
                // Check, if we have a metaclass 
                found = false;
                for (let n in fields) {
                    const field = fields[n];
                    if (field.metaClass.uri === 'dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData') {
                        found = true;
                    }
                }
                chai.assert.isTrue(found, 'Fields do not contain a metaclass');
            }));
            it('Load ViewModes', () => __awaiter(this, void 0, void 0, function* () {
                const viewModes = yield ClientForms.getViewModes();
                let found = false;
                for (let n in viewModes.viewModes) {
                    const v = viewModes.viewModes[n];
                    if (v.get('id') === "Default") {
                        found = true;
                    }
                }
                chai.assert.isTrue(found, "The Default viewMode was not found");
            }));
            after(function () {
                return __awaiter(this, void 0, void 0, function* () {
                    yield ClientExtent.deleteExtent({
                        workspace: "Test",
                        extentUri: "dm:///unittest",
                        skipIfNotExisting: true
                    });
                    yield ClientExtent.deleteExtent({
                        workspace: "Test",
                        extentUri: "dm:///newexisting",
                        skipIfNotExisting: true
                    });
                    yield ClientWorkspace.deleteWorkspace("Test");
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Forms.js.map