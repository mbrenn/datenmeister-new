var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Extents", "../client/Workspace", "../client/Items", "../client/Forms", "../Mof", "../models/DatenMeister.class", "../forms/Interfaces"], function (require, exports, ClientExtent, ClientWorkspace, ClientItems, ClientForms, Mof_1, DatenMeister_class_1, Interfaces_1) {
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
                const form = yield ClientForms.getCollectionFormForExtent('Test', 'dm:///unittest', '');
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
            it('Load Specific Form with different Form Types', () => __awaiter(this, void 0, void 0, function* () {
                // Test that default form is a row form
                const form = yield ClientForms.getForm('dm:///_internal/forms/internal#ImportManagerFindExtent');
                chai.assert.isTrue(form.metaClass.name === "RowForm", 'Not a row Form');
                // Test that retrieval as collection form is working
                const formAsCollection = yield ClientForms.getForm('dm:///_internal/forms/internal#ImportManagerFindExtent', Interfaces_1.FormType.Collection);
                chai.assert.isTrue(formAsCollection.metaClass.name === "CollectionForm", 'Not a collection Form');
                const tabs = formAsCollection.get(DatenMeister_class_1._DatenMeister._Forms._CollectionForm.tab, Mof_1.ObjectType.Array);
                chai.assert.isTrue(tabs.length === 1, '# of tabs of CollectionForm is not 1');
                chai.assert.isTrue(tabs[0].metaClass.name === "RowForm", 'Tab of CollectionForm is not a RowForm');
                // Test that retrieval as Object Form is working
                const formAsObject = yield ClientForms.getForm('dm:///_internal/forms/internal#ImportManagerFindExtent', Interfaces_1.FormType.Object);
                chai.assert.isTrue(formAsObject.metaClass.name === "ObjectForm", 'Not an Object Form');
                const tabsObject = formAsObject.get(DatenMeister_class_1._DatenMeister._Forms._CollectionForm.tab, Mof_1.ObjectType.Array);
                chai.assert.isTrue(tabsObject.length === 1, '# of tabs of ObjectForm is not 1');
                chai.assert.isTrue(tabsObject[0].metaClass.name === "RowForm", 'Tab of ObjectForm is not a RowForm');
            }));
            it('Load Default Form for Detail', () => __awaiter(this, void 0, void 0, function* () {
                const form = yield ClientForms.getObjectFormForItem('Test', itemUri, '');
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
            it('Create Collection Form For Extent', () => __awaiter(this, void 0, void 0, function* () {
                const result = yield ClientForms.createCollectionFormForExtent("Types", "dm:///_internal/types/internal");
                const foundForm = yield ClientItems.getItemWithNameAndId(result.createdForm.workspace, result.createdForm.uri);
                chai.assert.isTrue(foundForm !== undefined, "Form was not found");
                chai.assert.isTrue(foundForm.workspace === "Management", "Form is not in management");
                chai.assert.isTrue(foundForm.metaClassName === "CollectionForm", "Form is not Collection Form");
                yield ClientItems.deleteItem(result.createdForm.workspace, result.createdForm.uri);
            }));
            it('Create Object Form For Extent', () => __awaiter(this, void 0, void 0, function* () {
                const result = yield ClientForms.createObjectFormForItem("Management", "dm:///_internal/forms/internal#ImportManagerFindExtent");
                const foundForm = yield ClientItems.getItemWithNameAndId(result.createdForm.workspace, result.createdForm.uri);
                chai.assert.isTrue(foundForm !== undefined, "Form was not found");
                chai.assert.isTrue(foundForm.workspace === "Management", "Form is not in management");
                chai.assert.isTrue(foundForm.metaClassName === "ObjectForm", "Form is not ObjectForm Form");
                yield ClientItems.deleteItem(result.createdForm.workspace, result.createdForm.uri);
            }));
            it('Load ViewModes', () => __awaiter(this, void 0, void 0, function* () {
                const viewModes = yield ClientForms.getViewModes();
                let found = false;
                for (let n in viewModes.viewModes) {
                    const v = viewModes.viewModes[n];
                    if (v.get('id') === "ViewMode.Default") {
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