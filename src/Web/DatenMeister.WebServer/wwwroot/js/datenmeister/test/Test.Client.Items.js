var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Extents", "../client/Workspace", "../client/Items", "../Mof", "../ApiModels"], function (require, exports, ClientExtent, ClientWorkspace, ClientItems, Mof_1, ApiModels_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Items', function () {
                before(function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true });
                        yield ClientExtent.createXmi({
                            extentUri: "dm:///unittest",
                            filePath: "./unittest.xmi",
                            workspace: "Test",
                            skipIfExisting: true
                        });
                    });
                });
                it('Get Non-existing item', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        const result = yield ClientItems.getObjectByUri("Test", "Does_Not_Exist");
                        chai.assert.isUndefined(result);
                    });
                });
                it('Create and Delete Item', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        chai.assert.isTrue(result.success, 'Item was not created');
                        chai.assert.isTrue(result.itemId !== undefined && result.itemId !== null, "Item id is null");
                        const item = yield ClientItems.getObjectByUri("Test", result.itemId);
                        chai.assert.isTrue(item !== undefined, "Item is not existing");
                        chai.assert.isTrue(item.extentUri === "dm:///unittest", "Extent Uri is not correctly set");
                        chai.assert.isTrue(item.workspace === "Test", "Workspace is not correctly set");
                        const resultDelete = yield ClientItems.deleteItem("Test", result.itemId);
                        chai.assert.isTrue(result.success, "Item was not successful deleted");
                        const nonFoundItem = yield ClientItems.getObjectByUri("Test", result.itemId);
                        chai.assert.isTrue(item !== undefined, "Item was found when it should not be found");
                    });
                });
                it('Create and Delete All', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        chai.assert.isTrue(result.success, 'Item was not created');
                        const item = yield ClientItems.getObjectByUri("Test", result.itemId);
                        chai.assert.isTrue(item !== undefined, "Item is not existing");
                        const result2 = yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                        chai.assert.isTrue(result2.success, "Deletion of all root Elements did not work");
                        const nonFoundItem = yield ClientItems.getObjectByUri("Test", result.itemId);
                        chai.assert.isTrue(nonFoundItem === undefined, "Item was found when it should not be found");
                    });
                });
                it('Get and Set Properties', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        chai.assert.isTrue(result.success, 'Item was not created');
                        yield ClientItems.setProperty('Test', '#' + result.itemId, 'name', 'Brenn');
                        const property = yield ClientItems.getProperty('Test', result.itemId, 'name');
                        chai.assert.isTrue(property === 'Brenn');
                        const result2 = yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                        chai.assert.isTrue(result2.success, "Deletion of all root Elements did not work");
                    });
                });
                it('Get Root Elements', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        const result2 = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        chai.assert.isTrue(result.success, 'Item was not created');
                        chai.assert.isTrue(result2.success, 'Item was not created');
                        const allItems = yield ClientItems.getRootElements('Test', 'dm:///unittest');
                        chai.assert.equal(allItems.length, 2, "There are less or more items in the root elements");
                        for (let n in allItems) {
                            const item = allItems[n];
                            chai.assert.isTrue(item.uri === "dm:///unittest#" + result.itemId
                                || item.uri === "dm:///unittest#" + result2.itemId);
                        }
                        const result3 = yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                        chai.assert.isTrue(result3.success, "Deletion of all root Elements did not work");
                    });
                });
                it('Get Container', () => __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                    const subChild = yield ClientItems.createItemAsChild("Test", "dm:///unittest#" + result.itemId, { property: "packagedElement" });
                    const subSubChild = yield ClientItems.createItemAsChild("Test", "dm:///unittest#" + subChild.itemId, { property: "packagedElement" });
                    const extentContainer = yield ClientItems.getContainer("Test", "dm:///unittest#" + result.itemId);
                    // Should only be extent and the workspace. First item as the extent
                    chai.assert.isTrue(extentContainer.length === 2, "Test 1: Length should be 2");
                    chai.assert.isTrue(extentContainer[0].ententType === ApiModels_1.EntentType.Extent, "Test 2: First item should be Extent");
                    chai.assert.isTrue(extentContainer[1].ententType === ApiModels_1.EntentType.Workspace, "Test 3: First item should be Workspace");
                    const extentContainer2 = yield ClientItems.getContainer("Test", "dm:///unittest#" + result.itemId, true);
                    // Should only be extent and the workspace. First item as the extent
                    chai.assert.isTrue(extentContainer2.length === 3, "Test 4: Length should be 3");
                    chai.assert.isTrue(extentContainer2[0].ententType === ApiModels_1.EntentType.Item, "Test 5: First item should be Item");
                    chai.assert.isTrue(extentContainer2[0].uri === "dm:///unittest#" + result.itemId);
                    chai.assert.isTrue(extentContainer2[1].ententType === ApiModels_1.EntentType.Extent, "Test 6: First item should be Extent");
                    chai.assert.isTrue(extentContainer2[2].ententType === ApiModels_1.EntentType.Workspace, "Test 7: First item should be Workspace");
                    const extentContainer3 = yield ClientItems.getContainer("Test", "dm:///unittest#" + subSubChild.itemId, true);
                    // Should only be extent and the workspace. First item as the extent
                    chai.assert.isTrue(extentContainer3.length === 5, "Test 8: Length should be 3");
                    chai.assert.isTrue(extentContainer3[0].ententType === ApiModels_1.EntentType.Item, "Test 9: First item should be Item");
                    chai.assert.isTrue(extentContainer3[0].uri === "dm:///unittest#" + subSubChild.itemId, "Test 9a");
                    chai.assert.isTrue(extentContainer3[1].ententType === ApiModels_1.EntentType.Item, "Test 10: First item should be Item");
                    chai.assert.isTrue(extentContainer3[1].uri === "dm:///unittest#" + subChild.itemId, "Test 10a");
                    chai.assert.isTrue(extentContainer3[2].ententType === ApiModels_1.EntentType.Item, "Test 11: First item should be Item");
                    chai.assert.isTrue(extentContainer3[2].uri === "dm:///unittest#" + result.itemId, "Test 11a");
                    chai.assert.isTrue(extentContainer3[3].ententType === ApiModels_1.EntentType.Extent, "Test 12: First item should be Extent");
                    chai.assert.isTrue(extentContainer3[4].ententType === ApiModels_1.EntentType.Workspace, "Test 13: First item should be Workspace");
                    const extentContainer4 = yield ClientItems.getContainer("Test", "dm:///unittest#" + subSubChild.itemId);
                    // Should only be extent and the workspace. First item as the extent
                    chai.assert.isTrue(extentContainer4.length === 4, "Test 14: Length should be 4");
                    chai.assert.isTrue(extentContainer4[0].ententType === ApiModels_1.EntentType.Item, "Test 17 First item should be Item");
                    chai.assert.isTrue(extentContainer4[0].uri === "dm:///unittest#" + subChild.itemId, "Test 18");
                    chai.assert.isTrue(extentContainer4[1].ententType === ApiModels_1.EntentType.Item, "Test 19: First item should be Item");
                    chai.assert.isTrue(extentContainer4[1].uri === "dm:///unittest#" + result.itemId, "Test 20");
                    chai.assert.isTrue(extentContainer4[2].ententType === ApiModels_1.EntentType.Extent, "Test 21: First item should be Extent");
                    chai.assert.isTrue(extentContainer4[3].ententType === ApiModels_1.EntentType.Workspace, "Test 22: First item should be Workspace");
                }));
                it('Set and get multiple properties', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        const element = new Mof_1.DmObject();
                        element.set('name', 'Brenn');
                        element.set('age', 40);
                        const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", { properties: element });
                        let property = yield ClientItems.getProperty('Test', result.itemId, 'name');
                        chai.assert.equal(property, 'Brenn', 'Name is not set');
                        property = yield ClientItems.getProperty('Test', result.itemId, 'age');
                        chai.assert.equal(property.toString(), '40', 'Age is not set');
                        const element2 = new Mof_1.DmObject();
                        element2.set('prename', 'Martin');
                        element2.set('zip', 12345);
                        yield ClientItems.setProperties('Test', result.itemId, element2);
                        property = yield ClientItems.getProperty('Test', result.itemId, 'prename');
                        chai.assert.equal(property, 'Martin', 'Martin is not set');
                        property = yield ClientItems.getProperty('Test', result.itemId, 'zip');
                        chai.assert.equal(property.toString(), '12345', 'Zip is not set');
                        const properties = yield ClientItems.getObjectByUri('Test', result.itemId);
                        chai.assert.isTrue(properties !== undefined && properties !== null, 'properties are not set');
                        chai.assert.equal(properties.get('name'), 'Brenn', 'name is not correctly set');
                        chai.assert.equal(properties.get('prename'), 'Martin', 'prename is not correctly set');
                        chai.assert.equal(properties.get('age').toString(), '40', 'age is not correctly set');
                        chai.assert.equal(properties.get('zip').toString(), '12345', 'zip is not correctly set');
                        const result3 = yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                        chai.assert.isTrue(result3.success, "Deletion of all root Elements did not work");
                    });
                });
                it('Set Reference Property', () => __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                    const result2 = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                    yield ClientItems.setProperty("Test", result.itemId, "name", "item1");
                    yield ClientItems.setProperty("Test", result2.itemId, "name", "item2");
                    let reference = yield ClientItems.getProperty("Test", result.itemId, "reference");
                    chai.assert.isTrue(reference === undefined || reference === null, "Not set item should be undefined");
                    const resultSetProperty = yield ClientItems.setPropertyReference("Test", result.itemId, {
                        property: "reference",
                        workspaceId: "Test",
                        referenceUri: result2.itemId
                    });
                    chai.assert.isTrue(resultSetProperty.success === true, "Setting should be a success story");
                    reference = yield ClientItems.getProperty("Test", result.itemId, "reference");
                    chai.assert.isTrue(reference !== undefined, "Item is still undefined");
                    chai.assert.isTrue(reference.get("name") === "item2", "Name is not correctly set");
                }));
                it('Import Xmi', () => __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                    yield ClientItems.importXmi("Test", "dm:///unittest#" + result.itemId, "child", false, "<item p1:type=\"dm:///_internal/types/internal#IssueMeister.Issue\" state=\"Closed\" name=\"Yes\" xmlns:p1=\"http://www.omg.org/spec/XMI/20131001\" />");
                    const child = yield ClientItems.getProperty("Test", "dm:///unittest#" + result.itemId, "child");
                    const asDmObject = child[0];
                    chai.assert.isTrue(asDmObject !== undefined);
                    chai.assert.isTrue(asDmObject.get("state", Mof_1.ObjectType.String) === "Closed");
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
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Items.js.map