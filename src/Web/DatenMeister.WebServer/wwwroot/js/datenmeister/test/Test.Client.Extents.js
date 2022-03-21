define(["require", "exports", "../Client.Extents", "../Client.Workspace"], function (require, exports, ClientExtent, ClientWorkspace) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Extents', function () {
                before(function () {
                    return new Promise(done => {
                        ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true }).done(() => done());
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
                after(function () {
                    return new Promise(done => {
                        return ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///unittest",
                            skipIfNotExisting: true
                        }).done(done);
                    }).then(done => {
                        ClientWorkspace.deleteWorkspace("Test");
                    });
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Extents.js.map