import * as ClientWorkspace from "../client/Workspace.js";
import * as ClientExtent from "../client/Extents.js";
import * as ClientItems from "../client/Items.js";
import * as ClientActionsItem from "../client/Actions.Items.js";
import {DmObject, ObjectType} from "../Mof.js";

export function includeTests() {
    describe('Client', () => {
        describe('Actions', () => {
            describe('Item', () => {

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
                
                it('MoveUp/Down in Collection', async () => {
                    
                    await ClientItems.deleteRootElements("Test", "dm:///unittest");
                    
                    const result = await ClientItems.createItemInExtent(
                        "Test",
                        "dm:///unittest",
                        {}
                    );

                    const subChild1 = await ClientItems.createItemAsChild(
                        "Test",
                        "dm:///unittest#" + result.itemId,
                        {property: "packagedElement", asList: true}
                    );
                    
                    await ClientItems.setProperty("Test", subChild1.itemId, "name", "Child 1");

                    const subChild2 = await ClientItems.createItemAsChild(
                        "Test",
                        "dm:///unittest#" + result.itemId,
                        {property: "packagedElement", asList: true}
                    );

                    await ClientItems.setProperty("Test", subChild2.itemId, "name", "Child 2");
                    
                    const subChild3 = await ClientItems.createItemAsChild(
                        "Test",
                        "dm:///unittest#" + result.itemId,
                        {property: "packagedElement", asList: true}
                    );

                    await ClientItems.setProperty("Test", subChild3.itemId, "name", "Child 3");
                    
                    const children = 
                        await ClientItems.getProperty("Test", result.itemId, "packagedElement" );
                    chai.assert.isTrue(Array.isArray(children) === true, "Array has to be true");
                    chai.assert.isTrue((children as Array<any>).length === 3, "Length of array has to be 3");
                    const child2Name = (children[1] as DmObject).get("name", ObjectType.String);
                    
                    chai.assert.isTrue(child2Name === "Child 2", "Name is not found");
                    
                    await ClientActionsItem.moveItemInCollectionUp(
                        "Test",
                        result.itemId,
                        "packagedElement",
                        subChild2.itemId);

                    const newChildren =
                        await ClientItems.getProperty("Test", result.itemId, "packagedElement" );
                    const child1Name = (newChildren[1] as DmObject).get("name", ObjectType.String);

                    chai.assert.isTrue(child1Name === "Child 1", "Item has not been moved");

                    await ClientActionsItem.moveItemInCollectionDown(
                        "Test",
                        result.itemId,
                        "packagedElement",
                        subChild2.itemId);
                    await ClientActionsItem.moveItemInCollectionDown(
                        "Test",
                        result.itemId,
                        "packagedElement",
                        subChild2.itemId);

                    const newChildren2 =
                        await ClientItems.getProperty("Test", result.itemId, "packagedElement" );
                    const child1Name2 = (newChildren2[2] as DmObject).get("name", ObjectType.String);

                    chai.assert.isTrue(child1Name2 === "Child 2", "Item has not been moved back");
                });

                it('MoveUp/Down in Extent', async () => {
                    
                    await ClientItems.deleteRootElements("Test", "dm:///unittest");
                    
                    const subChild1 = await ClientItems.createItemInExtent(
                        "Test",
                        "dm:///unittest",
                        {}
                    );

                    await ClientItems.setProperty("Test", subChild1.itemId, "name", "Child 1");

                    const subChild2 = await ClientItems.createItemInExtent(
                        "Test",
                        "dm:///unittest",
                        {}
                    );

                    await ClientItems.setProperty("Test", subChild2.itemId, "name", "Child 2");

                    const subChild3 = await ClientItems.createItemInExtent(
                        "Test",
                        "dm:///unittest",
                        {}
                    );

                    await ClientItems.setProperty("Test", subChild3.itemId, "name", "Child 3");

                    const children =
                        await ClientItems.getRootElements("Test", "dm:///unittest");
                    chai.assert.isTrue(Array.isArray(children) === true, "Array has to be true");
                    chai.assert.isTrue((children as Array<any>).length === 3, "Length of array has to be 3");
                    const child2Name = (children[1] as DmObject).get("name", ObjectType.String);

                    chai.assert.isTrue(child2Name === "Child 2", "Name is not found");

                    await ClientActionsItem.moveItemInExtentUp(
                        "Test",
                        "dm:///unittest",
                        subChild2.itemId);

                    const newChildren =
                        await ClientItems.getRootElements("Test", "dm:///unittest");
                    const child1Name = (newChildren[1] as DmObject).get("name", ObjectType.String);

                    chai.assert.isTrue(child1Name === "Child 1", "Item has not been moved");

                    await ClientActionsItem.moveItemInExtentDown(
                        "Test",
                        "dm:///unittest",
                        subChild2.itemId);
                    await ClientActionsItem.moveItemInExtentDown(
                        "Test",
                        "dm:///unittest",
                        subChild2.itemId);

                    const newChildren2 =
                        await ClientItems.getRootElements("Test", "dm:///unittest");
                    const child1Name2 = (newChildren2[2] as DmObject).get("name", ObjectType.String);

                    chai.assert.isTrue(child1Name2 === "Child 2", "Item has not been moved back");
                });


                after(async function () {
                    await ClientExtent.deleteExtent({
                        workspace: "Test",
                        extentUri: "dm:///unittest",
                        skipIfNotExisting: true
                    });

                    await ClientWorkspace.deleteWorkspace("Test");
                });
            });
        });
    });
}