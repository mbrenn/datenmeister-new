import * as Forms from '../forms/Forms'
import * as ClientExtent from "../client/Extents";
import * as ClientWorkspace from "../client/Workspace";
import * as ClientItems from "../client/Items";
import * as ClientForms from "../client/Forms";
import {DmObject, ObjectType} from "../Mof";
import {_DatenMeister} from "../models/DatenMeister.class";

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
            const form = await ClientForms.getCollectionFormForExtent('Test', 'dm:///unittest', '');
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
        
        it('Load Specific Form with different Form Types', async () => {
            // Test that default form is a row form
            const form = await ClientForms.getForm('dm:///_internal/forms/internal#ImportManagerFindExtent');
            
            chai.assert.isTrue(form.metaClass.name === "RowForm", 'Not a row Form');
            
            // Test that retrieval as collection form is working
            const formAsCollection = await ClientForms.getForm(
                'dm:///_internal/forms/internal#ImportManagerFindExtent', 
                ClientForms.FormType.Collection);
            chai.assert.isTrue(formAsCollection.metaClass.name === "CollectionForm", 'Not a collection Form');
            
            const tabs = formAsCollection.get(_DatenMeister._Forms._CollectionForm.tab, ObjectType.Array);
            chai.assert.isTrue(tabs.length === 1, '# of tabs of CollectionForm is not 1');
            chai.assert.isTrue((tabs[0] as DmObject).metaClass.name === "RowForm", 'Tab of CollectionForm is not a RowForm');

            // Test that retrieval as Object Form is working
            const formAsObject = await ClientForms.getForm(
                'dm:///_internal/forms/internal#ImportManagerFindExtent',
                ClientForms.FormType.Object);
            chai.assert.isTrue(formAsObject.metaClass.name === "ObjectForm", 'Not an Object Form');

            const tabsObject = formAsObject.get(_DatenMeister._Forms._CollectionForm.tab, ObjectType.Array);
            chai.assert.isTrue(tabsObject.length === 1, '# of tabs of ObjectForm is not 1');
            chai.assert.isTrue((tabsObject[0] as DmObject).metaClass.name === "RowForm", 'Tab of ObjectForm is not a RowForm');
        });

        it('Load Default Form for Detail', async () => {
            const form = await ClientForms.getObjectFormForItem('Test', itemUri, '');
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