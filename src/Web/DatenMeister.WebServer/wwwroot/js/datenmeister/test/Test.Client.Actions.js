import * as ClientActions from "../client/Actions.js";
import * as ClientExtent from "../client/Extents.js";
import * as Mof from "../Mof.js";
import { ObjectType } from "../Mof.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import * as ClientWorkspace from "../client/Workspace.js";
import * as ClientItems from "../client/Items.js";
import { _UML } from "../models/uml.js";
export function includeTests() {
    describe('Client', function () {
        describe('Actions', async function () {
            before(async function () {
                await ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true });
                await ClientExtent.createXmi({
                    extentUri: "dm:///unittest",
                    filePath: "./unittest.xmi",
                    workspace: "Test",
                    skipIfExisting: true
                });
                await ClientExtent.deleteExtent({
                    workspace: "Test",
                    extentUri: "dm:///unittestaction",
                    skipIfNotExisting: true
                });
            });
            it('Success Echo', async () => {
                const parameter = new Mof.DmObject();
                parameter.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction", 'Types');
                parameter.set('shallSuccess', 'OK');
                const result = await ClientActions.executeActionDirectly("Execute", { parameter: parameter });
                chai.assert.isTrue(result.success === true);
                chai.assert.isTrue(result.resultAsDmObject !== undefined);
                chai.assert.isTrue(result.resultAsDmObject.get("returnText", ObjectType.String) === "Returned");
            });
            it('No success Echo', async () => {
                const parameter = new Mof.DmObject();
                parameter.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction", 'Types');
                parameter.set('shallSuccess', 'NO');
                const result = await ClientActions.executeActionDirectly("Execute", { parameter: parameter });
                chai.assert.isTrue(result.success === false);
            });
            it('Create Extent via Action', async () => {
                let success = await ClientExtent.exists("Test", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === false, "dm:///unittestaction may not exist");
                const parameter = new Mof.DmObject();
                parameter.setMetaClassByUri(_DatenMeister._Actions.__LoadExtentAction_Uri, 'Types');
                const configuration = new Mof.DmObject();
                configuration.setMetaClassByUri(_DatenMeister._ExtentLoaderConfigs.__InMemoryLoaderConfig_Uri, 'Types');
                configuration.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.extentUri, "dm:///unittestaction");
                configuration.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig.workspaceId, "Test");
                configuration.set(_DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig._name_, "UnitTest");
                parameter.set(_DatenMeister._Actions._LoadExtentAction.configuration, configuration);
                await ClientActions.executeActionDirectly("Execute", { parameter: parameter });
                success = await ClientExtent.exists("Test", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === true, "dm:///unittestaction should exist");
                const drop = new Mof.DmObject();
                drop.setMetaClassByUri(_DatenMeister._Actions.__DropExtentAction_Uri, 'Types');
                drop.set(_DatenMeister._Actions._DropExtentAction.workspace, "Test");
                drop.set(_DatenMeister._Actions._DropExtentAction.extentUri, "dm:///unittestaction");
                await ClientActions.executeActionDirectly("Execute", { parameter: drop });
                success = await ClientExtent.exists("Test", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === false, "dm:///unittestaction should have been deleted");
            });
            it('Copy item', async () => {
                const parent1 = await ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                const parent2 = await ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                const child1 = await ClientItems.createItemAsChild("Test", parent1.itemId, {
                    property: _UML._Packages._Package.packagedElement,
                    asList: true
                });
                await ClientItems.setProperty("Test", child1.itemId, "name", "Yes");
                // Now create the copy action to copy the child 1 to parent2
                const action = new Mof.DmObject();
                action.setMetaClassByUri(_DatenMeister._Actions.__MoveOrCopyAction_Uri, 'Types');
                action.set(_DatenMeister._Actions._MoveOrCopyAction.source, Mof.DmObject.createFromReference("Test", child1.itemUrl));
                action.set(_DatenMeister._Actions._MoveOrCopyAction.target, Mof.DmObject.createFromReference("Test", parent2.itemUrl));
                action.set(_DatenMeister._Actions._MoveOrCopyAction.copyMode, _DatenMeister._Actions._MoveOrCopyType.Copy);
                const copyResult = await ClientActions.executeActionDirectly("Execute", { parameter: action });
                chai.assert.isTrue(copyResult !== undefined, "Copy Result should have a return");
                chai.assert.isTrue(copyResult.resultAsDmObject !== undefined, "Copy Result should have a result");
                const copyResultUri = copyResult.resultAsDmObject.get(_DatenMeister._Actions._MoveOrCopyActionResult.targetUrl, Mof.ObjectType.String);
                const copyResultWorkspace = copyResult.resultAsDmObject.get(_DatenMeister._Actions._MoveOrCopyActionResult.targetWorkspace, Mof.ObjectType.String);
                chai.assert.isTrue(copyResultUri !== undefined && copyResultUri !== "", "Uri of Copy Result is empty");
                // Check, if the new item is exsting
                const check = await ClientItems.getObjectByUri(copyResultWorkspace, copyResultUri);
                chai.assert.isTrue(check !== undefined, "Copied item was not found");
                chai.assert.isTrue(check.get('name', Mof.ObjectType.String) === 'Yes', "Copied item does not have the property");
                // Check, that Parent 2 has one child
                const checkList = await ClientItems.getObjectByUri("Test", parent2.itemId);
                const checkedProperty = checkList.get(_UML._Packages._Package.packagedElement, ObjectType.Array);
                chai.assert.isTrue(checkedProperty.length === 1, "New List does not contain copied item");
                // Check, that Parent 1 has one child
                const checkList2 = await ClientItems.getObjectByUri("Test", parent1.itemId);
                const checkedProperty2 = checkList2.get(_UML._Packages._Package.packagedElement, ObjectType.Array);
                chai.assert.isTrue(checkedProperty2.length === 1, "Old List does not contain anymore");
            });
            it('Move item', async () => {
                const parent1 = await ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                const parent2 = await ClientItems.createItemInExtent("Test", "dm:///unittest", {});
                const child1 = await ClientItems.createItemAsChild("Test", parent1.itemId, {
                    property: _UML._Packages._Package.packagedElement,
                    asList: true
                });
                await ClientItems.setProperty("Test", child1.itemId, "name", "Yes");
                // Now create the copy action to copy the child 1 to parent2
                const action = new Mof.DmObject();
                action.setMetaClassByUri(_DatenMeister._Actions.__MoveOrCopyAction_Uri, 'Types');
                action.set(_DatenMeister._Actions._MoveOrCopyAction.source, Mof.DmObject.createFromReference("Test", child1.itemUrl));
                action.set(_DatenMeister._Actions._MoveOrCopyAction.target, Mof.DmObject.createFromReference("Test", parent2.itemUrl));
                action.set(_DatenMeister._Actions._MoveOrCopyAction.copyMode, _DatenMeister._Actions._MoveOrCopyType.Move);
                const copyResult = await ClientActions.executeActionDirectly("Execute", { parameter: action });
                chai.assert.isTrue(copyResult !== undefined);
                chai.assert.isTrue(copyResult.resultAsDmObject !== undefined);
                const copyResultUri = copyResult.resultAsDmObject.get(_DatenMeister._Actions._MoveOrCopyActionResult.targetUrl, Mof.ObjectType.String);
                const copyResultWorkspace = copyResult.resultAsDmObject.get(_DatenMeister._Actions._MoveOrCopyActionResult.targetWorkspace, Mof.ObjectType.String);
                chai.assert.isTrue(copyResultUri !== undefined && copyResultUri !== "");
                // Check, if the new item is exsting
                const check = await ClientItems.getObjectByUri(copyResultWorkspace, copyResultUri);
                chai.assert.isTrue(check !== undefined);
                chai.assert.isTrue(check.get('name', Mof.ObjectType.String) === 'Yes');
                // Check, that Parent 2 has one child
                const checkList = await ClientItems.getObjectByUri("Test", parent2.itemId);
                const checkedProperty = checkList.get(_UML._Packages._Package.packagedElement, ObjectType.Array);
                chai.assert.isTrue(checkedProperty.length === 1);
                // Check, that Parent 1 has no child
                const checkList2 = await ClientItems.getObjectByUri("Test", parent1.itemId);
                const checkedProperty2 = checkList2.get(_UML._Packages._Package.packagedElement, ObjectType.Array);
                chai.assert.isTrue(checkedProperty2.length === 0, "Old List still contains the item");
            });
            after(async function () {
                await ClientExtent.deleteExtent({
                    workspace: "Test",
                    extentUri: "dm:///unittest",
                    skipIfNotExisting: true
                });
                await ClientExtent.deleteExtent({
                    workspace: "Test",
                    extentUri: "dm:///unittestaction",
                    skipIfNotExisting: true
                });
                await ClientWorkspace.deleteWorkspace("Test");
            });
        });
    });
}
//# sourceMappingURL=Test.Client.Actions.js.map