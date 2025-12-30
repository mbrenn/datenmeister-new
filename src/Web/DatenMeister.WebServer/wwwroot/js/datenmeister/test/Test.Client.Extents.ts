import * as ClientExtent from "../client/Extents.js"
import * as ClientItem from "../client/Items.js"
import * as ClientWorkspace from "../client/Workspace.js"
import * as Mof from "../Mof.js";

export function includeTests() {
    describe('Client', function () {
        describe('Extents', function () {

            before(async function () {
                await ClientWorkspace.createWorkspace(
                    "Test",
                    "Annotation",
                    {skipIfExisting: true});
            });

            it('Create and delete Xmi Extent', async function () {
                await ClientExtent.deleteExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///unittest"
                    }
                );

                const createXmiResult = await ClientExtent.createXmi(
                    {
                        extentUri: "dm:///unittest",
                        filePath: "./unittest.xmi",
                        workspace: "Test"
                    }
                );

                chai.assert.isTrue(createXmiResult.success, "Creation did not work");
                const deleteExtentResult = await ClientExtent.deleteExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///unittest"
                    }
                );

                chai.assert.isTrue(deleteExtentResult.success, "Deletion did not work");
            });

            it('Delete and Create and skip, if (not) existing', async function () {
                let deleteExtentResult = await ClientExtent.deleteExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///notexisting",
                        skipIfNotExisting: true
                    });
                chai.assert.isTrue(deleteExtentResult.success, "Tried Deletion was not successful");
                chai.assert.isTrue(deleteExtentResult.skipped, "Was not skipped");
                deleteExtentResult = await ClientExtent.deleteExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///newexisting",
                        skipIfNotExisting: true
                    });

                chai.assert.isTrue(deleteExtentResult.success, " Deletion was not successful");
                deleteExtentResult = await ClientExtent.createXmi(
                    {
                        filePath: "./unittests.xmi",
                        workspace: "Test",
                        extentUri: "dm:///newexisting",
                        skipIfExisting: true
                    });
                chai.assert.isTrue(deleteExtentResult.success, " Creation was not successful");
                chai.assert.isFalse(deleteExtentResult.skipped, "Should not be skipped");

                deleteExtentResult = await ClientExtent.createXmi(
                    {
                        filePath: "./unittests.xmi",
                        workspace: "Test",
                        extentUri: "dm:///newexisting",
                        skipIfExisting: true
                    });
                chai.assert.isTrue(deleteExtentResult.success, "Creation was not successful");
                chai.assert.isTrue(deleteExtentResult.skipped, "Should be skipped");

                return ClientExtent.deleteExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///newexisting",
                        skipIfNotExisting: true
                    });
            });

            it('GetAnd Set Extent Properties', async function () {
                try {

                    let result = await ClientExtent.createXmi(
                        {
                            filePath: "./unittests.xmi",
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfExisting: true
                        });

                    chai.assert.isTrue(result.success, "Creation was not successful");

                    const value = new Mof.DmObject();
                    value.set('name', 'Testname');
                    await ClientExtent.setProperties("Test", "dm:///newexisting", value);

                    const properties = await ClientExtent.getProperties("Test", "dm:///newexisting");
                    chai.assert.isNotNull(properties, "Properties shall not be null");
                    chai.assert.equal(properties.get('name'), "Testname", "The property is not set");
                } catch (e) {
                    throw e;
                }
            });

            it('ExportXmi', async () => {

                let result = await ClientExtent.createXmi(
                    {
                        filePath: "./unittests.xmi",
                        workspace: "Test",
                        extentUri: "dm:///newexisting",
                        skipIfExisting: true
                    });
                
                const newItem = await ClientItem.createItemInExtent("Test", "dm:///newexisting", {                  
                });
                
                await ClientItem.setProperty('Test', "dm:///newexisting#" + newItem.itemId, 'name', 'Martin');
                
                const exportResult = await ClientExtent.exportXmi('Test', 'dm:///newexisting');
                chai.assert.isTrue(exportResult.xmi.indexOf('name') !== -1);
                chai.assert.isTrue(exportResult.xmi.indexOf('Martin') !== -1);
                
            });

            it('Clear Extent', async () => {

                const createResult = await ClientExtent.createXmi(
                    {
                        filePath: "./unittests_clear.xmi",
                        workspace: "Test",
                        extentUri: "dm:///newexisting_clear",
                        skipIfExisting: true
                    });
                
                chai.assert.isTrue(createResult.success);

                await ClientItem.createItemInExtent("Test", "dm:///newexisting_clear", {});

                let items =
                    (await ClientItem.getRootElements("Test", "dm:///newexisting_clear"))
                        .rootElementsAsObjects;
                chai.assert.isTrue(items.length === 1);

                await ClientExtent.clearExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///newexisting_clear"
                    }
                );

                items = (await ClientItem.getRootElements("Test", "dm:///newexisting_clear"))
                        .rootElementsAsObjects;
                chai.assert.isTrue(items.length === 0);
                
                await ClientExtent.deleteExtent(
                    {
                        workspace: "Test",
                        extentUri: "dm:///newexisting_clear"
                    });                
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