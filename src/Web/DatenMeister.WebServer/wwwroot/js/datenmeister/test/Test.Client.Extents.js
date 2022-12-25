var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Extents", "../client/Items", "../client/Workspace", "../Mof"], function (require, exports, ClientExtent, ClientItem, ClientWorkspace, Mof_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Extents', function () {
                before(function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true });
                    });
                });
                it('Create and delete Xmi Extent', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///unittest"
                        });
                        const createXmiResult = yield ClientExtent.createXmi({
                            extentUri: "dm:///unittest",
                            filePath: "./unittest.xmi",
                            workspace: "Test"
                        });
                        chai.assert.isTrue(createXmiResult.success, "Creation did not work");
                        const deleteExtentResult = yield ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///unittest"
                        });
                        chai.assert.isTrue(deleteExtentResult.success, "Deletion did not work");
                    });
                });
                it('Delete and Create and skip, if (not) existing', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        let deleteExtentResult = yield ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///notexisting",
                            skipIfNotExisting: true
                        });
                        chai.assert.isTrue(deleteExtentResult.success, "Tried Deletion was not successful");
                        chai.assert.isTrue(deleteExtentResult.skipped, "Was not skipped");
                        deleteExtentResult = yield ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfNotExisting: true
                        });
                        chai.assert.isTrue(deleteExtentResult.success, " Deletion was not successful");
                        deleteExtentResult = yield ClientExtent.createXmi({
                            filePath: "./unittests.xmi",
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfExisting: true
                        });
                        chai.assert.isTrue(deleteExtentResult.success, " Creation was not successful");
                        chai.assert.isFalse(deleteExtentResult.skipped, "Should not be skipped");
                        deleteExtentResult = yield ClientExtent.createXmi({
                            filePath: "./unittests.xmi",
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfExisting: true
                        });
                        chai.assert.isTrue(deleteExtentResult.success, "Creation was not successful");
                        chai.assert.isTrue(deleteExtentResult.skipped, "Should be skipped");
                        return ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfNotExisting: true
                        });
                    });
                });
                it('GetAnd Set Extent Properties', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        try {
                            let result = yield ClientExtent.createXmi({
                                filePath: "./unittests.xmi",
                                workspace: "Test",
                                extentUri: "dm:///newexisting",
                                skipIfExisting: true
                            });
                            chai.assert.isTrue(result.success, "Creation was not successful");
                            const value = new Mof_1.DmObject();
                            value.set('name', 'Testname');
                            yield ClientExtent.setProperties("Test", "dm:///newexisting", value);
                            const properties = yield ClientExtent.getProperties("Test", "dm:///newexisting");
                            chai.assert.isNotNull(properties, "Properties shall not be null");
                            chai.assert.equal(properties.get('name'), "Testname", "The property is not set");
                        }
                        catch (e) {
                            throw e;
                        }
                    });
                });
                it('ExportXmi', () => __awaiter(this, void 0, void 0, function* () {
                    let result = yield ClientExtent.createXmi({
                        filePath: "./unittests.xmi",
                        workspace: "Test",
                        extentUri: "dm:///newexisting",
                        skipIfExisting: true
                    });
                    const newItem = yield ClientItem.createItemInExtent("Test", "dm:///newexisting", {});
                    yield ClientItem.setProperty('Test', "dm:///newexisting#" + newItem.itemId, 'name', 'Martin');
                    const exportResult = yield ClientExtent.exportXmi('Test', 'dm:///newexisting');
                    chai.assert.isTrue(exportResult.xmi.indexOf('name') !== -1);
                    chai.assert.isTrue(exportResult.xmi.indexOf('Martin') !== -1);
                }));
                it('Clear Extent', () => __awaiter(this, void 0, void 0, function* () {
                    const createResult = yield ClientExtent.createXmi({
                        filePath: "./unittests_clear.xmi",
                        workspace: "Test",
                        extentUri: "dm:///newexisting_clear",
                        skipIfExisting: true
                    });
                    chai.assert.isTrue(createResult.success);
                    yield ClientItem.createItemInExtent("Test", "dm:///newexisting_clear", {});
                    let items = yield ClientItem.getRootElements("Test", "dm:///newexisting_clear");
                    chai.assert.isTrue(items.length === 1);
                    yield ClientExtent.clearExtent({
                        workspace: "Test",
                        extentUri: "dm:///newexisting_clear"
                    });
                    items = yield ClientItem.getRootElements("Test", "dm:///newexisting_clear");
                    chai.assert.isTrue(items.length === 0);
                    yield ClientExtent.deleteExtent({
                        workspace: "Test",
                        extentUri: "dm:///newexisting_clear"
                    });
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
//# sourceMappingURL=Test.Client.Extents.js.map