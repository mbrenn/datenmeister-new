import * as ClientExtent from "../client/Extents"
import * as ClientWorkspace from "../client/Workspace"
import * as ClientItems from "../client/Items"
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

            it('Create and Delete All', async function () {
                const result = await ClientItems.createItemInExtent(
                    "Test",
                    "dm:///unittest",
                    {}
                );

                chai.assert.isTrue(result.success, 'Item was not created');

                const item = await ClientItems.getObjectByUri("Test", result.itemId);
                chai.assert.isTrue(item !== undefined, "Item is not existing");

                const result2 = await ClientItems.deleteRootElements("Test", "dm:///unittest");
                chai.assert.isTrue(result2.success, "Deletion of all root Elements did not work");

                const nonFoundItem = await ClientItems.getObjectByUri("Test", result.itemId);
                chai.assert.isTrue(nonFoundItem === undefined, "Item was found when it should not be found");
            });

            it('Get and Set Properties', async function () {

                const result = await ClientItems.createItemInExtent(
                    "Test",
                    "dm:///unittest",
                    {}
                );

                chai.assert.isTrue(result.success, 'Item was not created');

                await ClientItems.setProperty('Test', '#' + result.itemId, 'name', 'Brenn');
                const property = await ClientItems.getProperty('Test', result.itemId, 'name');
                chai.assert.isTrue(property === 'Brenn');

                const result2 = await ClientItems.deleteRootElements("Test", "dm:///unittest");
                chai.assert.isTrue(result2.success, "Deletion of all root Elements did not work");
            });

            it('Get Root Elements', async function () {
                const result = await ClientItems.createItemInExtent(
                    "Test",
                    "dm:///unittest",
                    {}
                );

                const result2= await ClientItems.createItemInExtent(
                    "Test",
                    "dm:///unittest",
                    {}
                );

                chai.assert.isTrue(result.success, 'Item was not created');
                chai.assert.isTrue(result2.success, 'Item was not created');
                
                const allItems = await ClientItems.getRootElements('Test', 'dm:///unittest');
                chai.assert.equal(allItems.length, 2, "There are less or more items in the root elements");
                for (var n in allItems) {
                    const item = allItems[n];
                    chai.assert.isTrue(item.uri === "dm:///unittest#" + result.itemId
                        || item.uri === "dm:///unittest#" + result2.itemId);
                }

                const result3 = await ClientItems.deleteRootElements("Test", "dm:///unittest");
                chai.assert.isTrue(result3.success, "Deletion of all root Elements did not work");
            });
            
            it('Set and get multiple properties', async function(){

                const element = new DmObject();
                element.set('name', 'Brenn');
                element.set('age', 40);
                const result = await ClientItems.createItemInExtent(
                    "Test",
                    "dm:///unittest",
                    {properties: element}
                );
                
                let property = await ClientItems.getProperty('Test', result.itemId, 'name');
                chai.assert.equal(property, 'Brenn', 'Name is not set');
                property = await ClientItems.getProperty('Test', result.itemId, 'age');
                chai.assert.equal(property.toString(), '40', 'Age is not set');

                const element2 = new DmObject();
                element2.set('prename', 'Martin');
                element2.set('zip', 12345);
                
                await ClientItems.setProperties('Test', result.itemId, element2);

                property = await ClientItems.getProperty('Test', result.itemId, 'prename');
                chai.assert.equal(property, 'Martin', 'Martin is not set');
                property = await ClientItems.getProperty('Test', result.itemId, 'zip');
                chai.assert.equal(property.toString(), '12345', 'Zip is not set');
                
                const properties = await ClientItems.getObjectByUri('Test', result.itemId);
                chai.assert.isTrue(properties !== undefined && properties !== null, 'properties are not set');
                chai.assert.equal(properties.get('name'), 'Brenn', 'name is not correctly set');
                chai.assert.equal(properties.get('prename'), 'Martin', 'prename is not correctly set');
                chai.assert.equal(properties.get('age').toString(), '40', 'age is not correctly set');
                chai.assert.equal(properties.get('zip').toString(), '12345', 'zip is not correctly set');
                
                const result3 = await ClientItems.deleteRootElements("Test", "dm:///unittest");
                chai.assert.isTrue(result3.success, "Deletion of all root Elements did not work");
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