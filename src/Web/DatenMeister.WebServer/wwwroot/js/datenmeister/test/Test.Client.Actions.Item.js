var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Workspace", "../client/Extents", "../client/Items", "../client/Actions.Items", "../Mof"], function (require, exports, ClientWorkspace, ClientExtent, ClientItems, ClientActionsItem, Mof_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', () => {
            describe('Actions', () => {
                describe('Item', () => {
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
                    it('MoveUp/Down in Collection', () => __awaiter(this, void 0, void 0, function* () {
                        yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                        const result = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        const subChild1 = yield ClientItems.createItemAsChild("Test", "dm:///unittest#" + result.itemId, { property: "packagedElement", asList: true });
                        yield ClientItems.setProperty("Test", subChild1.itemId, "name", "Child 1");
                        const subChild2 = yield ClientItems.createItemAsChild("Test", "dm:///unittest#" + result.itemId, { property: "packagedElement", asList: true });
                        yield ClientItems.setProperty("Test", subChild2.itemId, "name", "Child 2");
                        const subChild3 = yield ClientItems.createItemAsChild("Test", "dm:///unittest#" + result.itemId, { property: "packagedElement", asList: true });
                        yield ClientItems.setProperty("Test", subChild3.itemId, "name", "Child 3");
                        const children = yield ClientItems.getProperty("Test", result.itemId, "packagedElement");
                        chai.assert.isTrue(Array.isArray(children) === true, "Array has to be true");
                        chai.assert.isTrue(children.length === 3, "Length of array has to be 3");
                        const child2Name = children[1].get("name", Mof_1.ObjectType.String);
                        chai.assert.isTrue(child2Name === "Child 2", "Name is not found");
                        yield ClientActionsItem.moveItemInCollectionUp("Test", result.itemId, "packagedElement", subChild2.itemId);
                        const newChildren = yield ClientItems.getProperty("Test", result.itemId, "packagedElement");
                        const child1Name = newChildren[1].get("name", Mof_1.ObjectType.String);
                        chai.assert.isTrue(child1Name === "Child 1", "Item has not been moved");
                        yield ClientActionsItem.moveItemInCollectionDown("Test", result.itemId, "packagedElement", subChild2.itemId);
                        yield ClientActionsItem.moveItemInCollectionDown("Test", result.itemId, "packagedElement", subChild2.itemId);
                        const newChildren2 = yield ClientItems.getProperty("Test", result.itemId, "packagedElement");
                        const child1Name2 = newChildren2[2].get("name", Mof_1.ObjectType.String);
                        chai.assert.isTrue(child1Name2 === "Child 2", "Item has not been moved back");
                    }));
                    it('MoveUp/Down in Extent', () => __awaiter(this, void 0, void 0, function* () {
                        yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                        const subChild1 = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        yield ClientItems.setProperty("Test", subChild1.itemId, "name", "Child 1");
                        const subChild2 = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        yield ClientItems.setProperty("Test", subChild2.itemId, "name", "Child 2");
                        const subChild3 = yield ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                        yield ClientItems.setProperty("Test", subChild3.itemId, "name", "Child 3");
                        const children = yield ClientItems.getRootElements("Test", "dm:///unittest");
                        chai.assert.isTrue(Array.isArray(children) === true, "Array has to be true");
                        chai.assert.isTrue(children.length === 3, "Length of array has to be 3");
                        const child2Name = children[1].get("name", Mof_1.ObjectType.String);
                        chai.assert.isTrue(child2Name === "Child 2", "Name is not found");
                        yield ClientActionsItem.moveItemInExtentUp("Test", "dm:///unittest", subChild2.itemId);
                        const newChildren = yield ClientItems.getRootElements("Test", "dm:///unittest");
                        const child1Name = newChildren[1].get("name", Mof_1.ObjectType.String);
                        chai.assert.isTrue(child1Name === "Child 1", "Item has not been moved");
                        yield ClientActionsItem.moveItemInExtentDown("Test", "dm:///unittest", subChild2.itemId);
                        yield ClientActionsItem.moveItemInExtentDown("Test", "dm:///unittest", subChild2.itemId);
                        const newChildren2 = yield ClientItems.getRootElements("Test", "dm:///unittest");
                        const child1Name2 = newChildren2[2].get("name", Mof_1.ObjectType.String);
                        chai.assert.isTrue(child1Name2 === "Child 2", "Item has not been moved back");
                    }));
                    after(function () {
                        return __awaiter(this, void 0, void 0, function* () {
                            yield ClientExtent.deleteExtent({
                                workspace: "Test",
                                extentUri: "dm:///unittest",
                                skipIfNotExisting: true
                            });
                            yield ClientWorkspace.deleteWorkspace("Test");
                        });
                    });
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Actions.Item.js.map