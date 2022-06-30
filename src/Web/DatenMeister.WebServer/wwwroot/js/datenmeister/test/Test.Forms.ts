import * as Forms from '../forms/Forms'
import * as ClientExtent from "../client/Extents";
import * as ClientWorkspace from "../client/Workspace";
import * as ClientItems from "../client/Items";
import * as ClientForms from "../client/Forms";
import {DmObject} from "../Mof";

export function includeTests() {
    describe('Forms', () => {

        let itemUri;

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
            itemUri = 'dm:///unittest#' + createdItem.itemId;
            await ClientItems.setProperty(
                'Test', itemUri, 'name', 'NamedElement');

            const createdChild = await ClientItems.createItemAsChild('Test', itemUri, {property: 'packagedElement'});
            const childItem = 'dm:///unittest#' + createdChild.itemId;
            await ClientItems.setProperty(
                'Test', childItem, 'name', 'ChildElement');
        });

        it('Load Default Form for Extents', async () => {
            const form = await ClientForms.getDefaultFormForExtent('Test', 'dm:///unittest', '');
            chai.assert.isTrue(form !== undefined, 'Form was not found');

            const tab = form.get('tab');
            chai.assert.isTrue(tab !== undefined, 'tabs are not defined');
            chai.assert.isTrue(Array.isArray(tab), 'Tabs is not an array');
            const firstTab = tab[0]
            chai.assert.isTrue(firstTab !== undefined, 'First tab is not defined');

            const fields = firstTab.get('field');
            chai.assert.isTrue(fields !== undefined, 'fields are not defined');
            chai.assert.isTrue(Array.isArray(fields), 'fields is not an array');

            // Check, if any of the field has a name 
            let found = false;
            for (let n in fields) {
                const field = fields[n] as DmObject;
                if (field.get('name') === 'name') {
                    found = true;
                }
            }

            chai.assert.isTrue(found, 'Field with name was not found');
        });

        it('Load Default Form for Detail', async () => {
            const form = await ClientForms.getDefaultFormForItem('Test', itemUri, '');
            chai.assert.isTrue(form !== undefined, 'Form was not found');

            const tab = form.get('tab');
            chai.assert.isTrue(tab !== undefined, 'tabs are not defined');
            chai.assert.isTrue(Array.isArray(tab), 'Tabs is not an array');
            const firstTab = tab[0]
            chai.assert.isTrue(firstTab !== undefined, 'First tab is not defined');

            const fields = firstTab.get('field');
            chai.assert.isTrue(fields !== undefined, 'fields are not defined');
            chai.assert.isTrue(Array.isArray(fields), 'fields is not an array');

            // Check, if any of the field has a name 
            let found = false;
            for (let n in fields) {
                const field = fields[n] as DmObject;
                if (field.get('name') === 'name') {
                    found = true;
                }
            }
            chai.assert.isTrue(found, 'Field with name was not found');

            // Check, if we have a metaclass 
            found = false;
            for (let n in fields) {
                const field = fields[n] as DmObject;
                if (field.metaClass.uri === 'dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData') {
                    found = true;
                }
            }

            chai.assert.isTrue(found, 'Fields do not contain a metaclass');

        });

        it('Load ViewModes', async () => {
            const viewModes = await ClientForms.getViewModes();
            let found = false;
            for (let n in viewModes.viewModes) {
                const v = viewModes.viewModes[n];
                if (v.get('id') === "Default") {
                    found = true;
                }
            }

            chai.assert.isTrue(found, "The Default viewMode was not found");
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
    })
}