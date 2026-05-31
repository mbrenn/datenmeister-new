import * as ClientWorkspace from "../client/Workspace.js"

import '../../node_modules/chai/register-assert.js';
declare var assert: Chai.AssertStatic;

export function includeTests() {

    describe('Client', function () {
        describe('Workspace', function () {
            it('Create and Delete Workspace', function (done) {
                    ClientWorkspace.deleteWorkspace("Test").then(result => {
                        return ClientWorkspace.createWorkspace("Test", "Annotation")
                    })
                        .then(result => {
                            assert.isTrue(result.success);
                            return ClientWorkspace.createWorkspace("Test", "Annotation");
                        })
                        .then(result => {
                            assert.isFalse(result.success);
                            return ClientWorkspace.deleteWorkspace("Test");
                        })
                        .then(result => {
                            assert.isTrue(result.success);
                            done();
                        }).catch(e => done(e));
                }
            );
        })
    })
}