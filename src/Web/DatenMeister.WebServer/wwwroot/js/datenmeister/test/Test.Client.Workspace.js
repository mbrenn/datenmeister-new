define(["require", "exports", "../Client.Workspace"], function (require, exports, ClientWorkspace) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Workspace', function () {
                it('Create and Delete Workspace', function (done) {
                    ClientWorkspace.deleteWorkspace("Test").then(result => {
                        return ClientWorkspace.createWorkspace("Test", "Annotation");
                    })
                        .then(result => {
                        chai.assert.isTrue(result.success);
                        return ClientWorkspace.createWorkspace("Test", "Annotation");
                    })
                        .then(result => {
                        chai.assert.isFalse(result.success);
                        return ClientWorkspace.deleteWorkspace("Test");
                    })
                        .then(result => {
                        chai.assert.isTrue(result.success);
                        done();
                    }).catch(e => done(e));
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Workspace.js.map