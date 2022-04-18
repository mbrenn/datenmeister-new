var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Workspace", "../client/Extents", "../client/Items", "../controls/SelectItemControl"], function (require, exports, ClientWorkspace, ClientExtent, ClientItems, SelectItemControl_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function lookForChildWithText(children, textToLookFor) {
        let found = false;
        let foundItem = undefined;
        children.each((index, child) => {
            if ($(child).text() === textToLookFor) {
                found = true;
                foundItem = child;
            }
        });
        return { found, foundItem };
    }
    function includeTests() {
        describe('Controls', () => {
            before(function () {
                return __awaiter(this, void 0, void 0, function* () {
                    yield ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true });
                    yield ClientExtent.createXmi({
                        extentUri: "dm:///unittest",
                        filePath: "./unittest.xmi",
                        workspace: "Test",
                        skipIfExisting: true
                    });
                    yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                    const createdItem = yield ClientItems.createItemInExtent('Test', 'dm:///unittest', {});
                    const itemUri = 'dm:///unittest#' + createdItem.itemId;
                    yield ClientItems.setProperty('Test', itemUri, 'name', 'NamedElement');
                    const createdChild = yield ClientItems.createItemAsChild('Test', itemUri, { property: 'packagedElement' });
                    const childItem = 'dm:///unittest#' + createdChild.itemId;
                    yield ClientItems.setProperty('Test', childItem, 'name', 'ChildElement');
                });
            });
            describe('SelectItemControl', () => {
                it('Create Controls and Load Workspaces', () => __awaiter(this, void 0, void 0, function* () {
                    let div = $("#dom_test");
                    if (div === undefined) {
                        div = $("<div></div>");
                    }
                    const sic = new SelectItemControl_1.SelectItemControl();
                    const query = yield sic.initAsync(div);
                    const workspace = $(".dm-sic-workspace select", query);
                    chai.assert.isTrue(workspace !== undefined, "No select given");
                    const children = workspace.children();
                    let { found, foundItem } = lookForChildWithText(children, 'Test');
                    chai.assert.isTrue(found, "Workspace was not found");
                    div.remove();
                }));
                it('Create Controls and Load Extents', () => __awaiter(this, void 0, void 0, function* () {
                    let div = $("#dom_test");
                    if (div === undefined) {
                        div = $("<div></div>");
                    }
                    const sic = new SelectItemControl_1.SelectItemControl();
                    const query = yield sic.initAsync(div);
                    yield sic.setWorkspaceById('Test');
                    const extent = $(".dm-sic-extent select", query);
                    chai.assert.isTrue(extent !== undefined, "No select given");
                    const children = extent.children();
                    let { found, foundItem } = lookForChildWithText(children, 'dm:///unittest');
                    chai.assert.isTrue(found, "Extent was not found");
                    div.remove();
                }));
                it('Create Controls and Load Items', () => __awaiter(this, void 0, void 0, function* () {
                    let div = $("#dom_test");
                    if (div === undefined) {
                        div = $("<div></div>");
                    }
                    let itemCounter = 0;
                    const sic = new SelectItemControl_1.SelectItemControl();
                    sic.itemSelected.addListener(() => {
                        itemCounter++;
                    });
                    const query = yield sic.initAsync(div);
                    yield sic.setWorkspaceById('Test');
                    yield sic.setExtentByUri('dm:///unittest');
                    const items = $(".dm-sic-items ul", query);
                    chai.assert.isTrue(items !== undefined, "No select given");
                    const children = items.children();
                    const textToLookFor = "NamedElement";
                    let { found, foundItem } = lookForChildWithText(children, textToLookFor);
                    chai.assert.isTrue(found, "Item was not found was not found");
                    chai.assert.isTrue(foundItem !== undefined, "Item Dom was not found was not found");
                    chai.assert.isTrue(itemCounter === 0, "Item Counter is not 0");
                    if (foundItem === undefined)
                        throw 'Should not happen';
                    foundItem.click();
                    // The click should have happened synchronously.
                    chai.assert.isTrue(itemCounter === 1, "Item Counter is not 1");
                    yield new Promise(resolve => {
                        sic.domItemsUpdated.addListener(() => {
                            const items = $(".dm-sic-items ul", query);
                            chai.assert.isTrue(items !== undefined, "No select given");
                            const children = items.children();
                            let { found, foundItem } = lookForChildWithText(children, 'ChildElement');
                            chai.assert.isTrue(found, "Child was not found");
                            resolve();
                        });
                    });
                    div.remove();
                }));
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Controls.SelectItemControl.js.map