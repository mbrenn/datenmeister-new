define(["require", "exports", "../Client.Extents", "../Client.Workspace"], function (require, exports, ClientExtent, ClientWorkspace) {
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
                /*it('Create and delete Properties, if existing', function(done) {
                    ClientExtent.deleteExtent(
                        {
                            workspace: "Test",
                            extentUri: "dm:///notexisting",
                            skipIfNotExisting: true
                        }).then(result => {
                        chai.assert.isTrue(result.success, "Tried Deletion was not successful");
                        chai.assert.isTrue(result.skipped, "Was not skipped");
                    }).catch(e => done(e));
                });*/
                after(function () {
                    return new Promise(done => {
                        return ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///unittest",
                            skipIfNotExisting: true
                        }).then(() => done());
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