var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Extents", "../client/Workspace", "../client/Items", "../Mof"], function (require, exports, ClientExtent, ClientWorkspace, ClientItems, Mof_1) {
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
                        for (var n in allItems) {
                            const item = allItems[n];
                            chai.assert.isTrue(item.uri === "dm:///unittest#" + result.itemId
                                || item.uri === "dm:///unittest#" + result2.itemId);
                        }
                        const result3 = yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                        chai.assert.isTrue(result3.success, "Deletion of all root Elements did not work");
                    });
                });
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