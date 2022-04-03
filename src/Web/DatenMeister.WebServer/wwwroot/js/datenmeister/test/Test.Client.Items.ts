import * as ClientExtent from "../Client.Extents"
import * as ClientWorkspace from "../Client.Workspace"
import * as ClientItems from "../Client.Items"
import {DmObject} from "../Mof";

export function includeTests() {
    describe('Client', function () {
        describe('Items', function () {

            before(async function () {
                await ClientWorkspace.createWorkspace(
                    "Test",
                    "Annotation",
                    {skipIfExisting: true});

                await ClientExtent.createXmi(
                    {
                        extentUri: "dm:///unittest",
                        filePath: "./unittest.xmi",
                        workspace: "Test",
                        skipIfExisting: true
                    });
            });

            it('Get Non-existing item', async function () {
                const result = await ClientItems.getObjectByUri("Test", "Does_Not_Exist");
                chai.assert.isUndefined(result);
            });

            it('Create and Delete Item', async function () {
                const result = await ClientItems.createItemInExtent(
                    "Test",
                    "dm:///unittest",
                    {}
                );

                chai.assert.isTrue(result.success, 'Item was not created');
                chai.assert.isTrue(result.itemId !== undefined && result.itemId !== null, "Item id is null");

                const item = await ClientItems.getObjectByUri("Test", result.itemId);
                chai.assert.isTrue(item !== undefined, "Item is not existing");
                chai.assert.isTrue(item.extentUri === "dm:///unittest", "Extent Uri is not correctly set");
                chai.assert.isTrue(item.workspace === "Test", "Workspace is not correctly set");

                const resultDelete = await ClientItems.deleteItem("Test", result.itemId);
                chai.assert.isTrue(result.success, "Item was not successful deleted");

                const nonFoundItem = await ClientItems.getObjectByUri("Test", result.itemId);
                chai.assert.isTrue(item !== undefined, "Item was found when it should not be found");
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