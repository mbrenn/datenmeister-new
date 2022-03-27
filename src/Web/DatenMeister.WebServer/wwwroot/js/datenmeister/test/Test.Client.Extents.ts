import * as ClientExtent from "../Client.Extents"
import * as ClientWorkspace from "../Client.Workspace"
import {DmObject} from "../Mof";

export function includeTests() {
    describe('Client', function () {
        describe('Extents', function () {

            before(function () {
                return new Promise<void>(done => {
                    ClientWorkspace.createWorkspace(
                        "Test",
                        "Annotation",
                        {skipIfExisting: true}).then(() => done());
                });
            });

            it('Create and delete Xmi Extent', function (done) {
                ClientExtent.deleteExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///unittest"
                    }
                ).then(() => {
                    return ClientExtent.createXmi(
                        {
                            extentUri: "dm:///unittest",
                            filePath: "./unittest.xmi",
                            workspace: "Test"
                        }
                    );
                }).then(result => {
                    chai.assert.isTrue(result.success, "Creation did not work");
                    return ClientExtent.deleteExtent(
                        {
                            workspace: "Test",
                            extentUri: "dm:///unittest"
                        }
                    );
                }).then(result => {
                    chai.assert.isTrue(result.success, "Deletion did not work");
                    done();
                }).catch(e => done(e));
            });

            it('Delete and Create and skip, if (not) existing', function (done) {
                ClientExtent.deleteExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///notexisting",
                        skipIfNotExisting: true
                    })                    
                    .then(result => {
                        chai.assert.isTrue(result.success, "Tried Deletion was not successful");
                        chai.assert.isTrue(result.skipped, "Was not skipped");
                        return ClientExtent.deleteExtent(
                            {
                                workspace: "Test",
                                extentUri: "dm:///newexisting",
                                skipIfNotExisting: true
                            });                        
                    })
                    .then(result => {
                        chai.assert.isTrue(result.success, " Deletion was not successful");
                        return ClientExtent.createXmi(
                            {
                                filePath: "./unittests.xmi",
                                workspace: "Test",
                                extentUri: "dm:///newexisting",
                                skipIfExisting: true
                            });
                    })
                    .then(result => {
                        chai.assert.isTrue(result.success, " Creation was not successful");
                        chai.assert.isFalse(result.skipped, "Should not be skipped");
                        
                        return ClientExtent.createXmi(
                            {
                                filePath: "./unittests.xmi",
                                workspace: "Test",
                                extentUri: "dm:///newexisting",
                                skipIfExisting: true
                            });                        
                    })
                    .then(result => {
                        chai.assert.isTrue(result.success, "Creation was not successful");
                        chai.assert.isTrue(result.skipped, "Should be skipped");
                        
                        return ClientExtent.deleteExtent(
                            {
                                workspace: "Test",
                                extentUri: "dm:///newexisting",
                                skipIfNotExisting: true
                            });
                    })
                    .then (_ => done())
                    .catch(e => done(e));
            });
            
            it('GetAnd Set Extent Properties', async function(){
                try {

                    let result = await ClientExtent.createXmi(
                        {
                            filePath: "./unittests.xmi",
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfExisting: true
                        });

                    chai.assert.isTrue(result.success, "Creation was not successful");

                    const value = new DmObject();
                    value.set('name', 'Testname');
                    await ClientExtent.setProperties("Test", "dm:///newexisting", value);
                    
                    const properties = await ClientExtent.getProperties("Test", "dm:///newexisting");
                    chai.assert.isNotNull(properties, "Properties shall not be null");
                    chai.assert.equal(properties.get('name'), "Testname", "The property is not set");
                }
                catch (e)
                {
                    throw e;
                }
            });

            after(async function () {
                
                await ClientExtent.deleteExtent({
                        workspace: "Test",
                        extentUri: "dm:///unittest",
                        skipIfNotExisting: true
                    });
                
                await ClientExtent.deleteExtent({
                    workspace: "Test",
                    extentUri: "dm:///newexisting",
                    skipIfNotExisting: true
                });
                
                await ClientWorkspace.deleteWorkspace("Test");
            });
        });
    });
}