import * as ClientWorkspace from "../client/Workspace";
import * as ClientExtent from "../client/Extents";
import * as ClientItems from "../client/Items";
import {SelectItemControl} from "../controls/SelectItemControl";


function lookForChildWithText(children: JQuery<HTMLElement>, textToLookFor: string) {
    let found = false;
    let foundItem: HTMLElement | undefined = undefined;
    children.each((index, child) => {
        if ($(child).text().indexOf(textToLookFor) !== -1) {
            found = true;
            foundItem = child;
        }
    });
    return {found, foundItem};
}

export function includeTests() {
    describe('Controls',
        () => {

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

                await ClientItems.deleteRootElements("Test", "dm:///unittest");

                const createdItem = await ClientItems.createItemInExtent('Test', 'dm:///unittest', {});

                const itemUri = 'dm:///unittest#' + createdItem.itemId;
                await ClientItems.setProperty(
                    'Test', itemUri, 'name', 'NamedElement');

                const createdChild = await ClientItems.createItemAsChild('Test', itemUri, {property: 'packagedElement'});

                const childItem = 'dm:///unittest#' + createdChild.itemId;
                await ClientItems.setProperty(
                    'Test', childItem, 'name', 'ChildElement');

            });

            describe('SelectItemControl',
                () => {
                    it('Create Controls and Load Workspaces',
                        async () => {
                            let div = $("#dom_test");
                            if (div === undefined) {
                                div = $("<div></div>");
                            }

                            const sic = new SelectItemControl();
                            const query = await sic.initAsync(div);

                            const workspace = $(".dm-sic-workspace select", query);
                            chai.assert.isTrue(workspace !== undefined, "No select given");

                            const children = workspace.children();
                            let {found, foundItem} = lookForChildWithText(children, 'Test');

                            chai.assert.isTrue(found, "Workspace was not found")

                            div.remove();
                        });

                    it('Create Controls and Load Extents',
                        async () => {
                            let div = $("#dom_test");
                            if (div === undefined) {
                                div = $("<div></div>");
                            }

                            const sic = new SelectItemControl();
                            const query = await sic.initAsync(div);

                            await sic.setWorkspaceById('Test');

                            const extent = $(".dm-sic-extent select", query);
                            chai.assert.isTrue(extent !== undefined, "No select given");

                            const children = extent.children();
                            let {found, foundItem} = lookForChildWithText(children, 'dm:///unittest');

                            chai.assert.isTrue(found, "Extent was not found")

                            div.remove();
                        });

                    it('Create Controls and Load Items',
                        async () => {
                            let div = $("#dom_test");
                            if (div === undefined) {
                                div = $("<div></div>");
                            }

                            let itemCounter = 0;
                            const sic = new SelectItemControl();
                            sic.itemClicked.addListener(() => {
                                itemCounter++;
                            });

                            const query = await sic.initAsync(div);

                            await sic.setExtentByUri("Test", 'dm:///unittest');

                            const items = $(".dm-sic-items ul", query);
                            chai.assert.isTrue(items !== undefined, "No select given");

                            const children = items.children();
                            const textToLookFor = "NamedElement";
                            let {found, foundItem} = lookForChildWithText(children, textToLookFor);

                            chai.assert.isTrue(found, "Item was not found");
                            chai.assert.isTrue(foundItem !== undefined, "Item Dom was not found was not found");
                            chai.assert.isTrue(itemCounter === 0, "Item Counter is not 0");

                            if (foundItem === undefined) throw 'Should not happen';
                            foundItem.click();

                            await sic.loadItems();

                            // The click should have happened synchronously.
                            chai.assert.isTrue(itemCounter === 1, "Item Counter is not 1");

                            // Check, if the enumerated list is given
                            const itemsUl = $(".dm-sic-items ul", query);
                            chai.assert.isTrue(itemsUl !== undefined, "No select given");

                            const childrenUl = itemsUl.children();

                            let result = lookForChildWithText(childrenUl, 'ChildElement');
                            chai.assert.isTrue(result.found, "Child was not found");

                            div.remove();
                        });
                });
        });
}