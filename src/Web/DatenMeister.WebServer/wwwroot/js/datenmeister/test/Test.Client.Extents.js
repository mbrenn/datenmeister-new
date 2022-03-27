var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Client.Extents", "../Client.Workspace", "../Mof"], function (require, exports, ClientExtent, ClientWorkspace, Mof_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Extents', function () {
                before(function () {
                    return new Promise(done => {
                        ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true }).then(() => done());
                    });
                });
                it('Create and delete Xmi Extent', function (done) {
                    ClientExtent.deleteExtent({
                        workspace: "Test",
                        extentUri: "dm:///unittest"
                    }).then(() => {
                        return ClientExtent.createXmi({
                            extentUri: "dm:///unittest",
                            filePath: "./unittest.xmi",
                            workspace: "Test"
                        });
                    }).then(result => {
                        chai.assert.isTrue(result.success, "Creation did not work");
                        return ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///unittest"
                        });
                    }).then(result => {
                        chai.assert.isTrue(result.success, "Deletion did not work");
                        done();
                    }).catch(e => done(e));
                });
                it('Delete and Create and skip, if (not) existing', function (done) {
                    ClientExtent.deleteExtent({
                        workspace: "Test",
                        extentUri: "dm:///notexisting",
                        skipIfNotExisting: true
                    })
                        .then(result => {
                        chai.assert.isTrue(result.success, "Tried Deletion was not successful");
                        chai.assert.isTrue(result.skipped, "Was not skipped");
                        return ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfNotExisting: true
                        });
                    })
                        .then(result => {
                        chai.assert.isTrue(result.success, " Deletion was not successful");
                        return ClientExtent.createXmi({
                            filePath: "./unittests.xmi",
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfExisting: true
                        });
                    })
                        .then(result => {
                        chai.assert.isTrue(result.success, " Creation was not successful");
                        chai.assert.isFalse(result.skipped, "Should not be skipped");
                        return ClientExtent.createXmi({
                            filePath: "./unittests.xmi",
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfExisting: true
                        });
                    })
                        .then(result => {
                        chai.assert.isTrue(result.success, "Creation was not successful");
                        chai.assert.isTrue(result.skipped, "Should be skipped");
                        return ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfNotExisting: true
                        });
                    })
                        .then(_ => done())
                        .catch(e => done(e));
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