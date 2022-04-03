var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Client.Extents", "../Client.Workspace", "../Client.Items"], function (require, exports, ClientExtent, ClientWorkspace, ClientItems) {
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