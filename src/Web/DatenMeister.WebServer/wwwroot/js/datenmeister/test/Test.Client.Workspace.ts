import * as ClientWorkspace from "../client/Workspace"

export function includeTests() {

    describe('Client', function () {
        describe('Workspace', function () {
            it('Create and Delete Workspace', function (done) {
                    ClientWorkspace.deleteWorkspace("Test").then(result => {
                        return ClientWorkspace.createWorkspace("Test", "Annotation")
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
                }
            );
        })
    })
}