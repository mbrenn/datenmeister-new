import * as ClientElements from "../client/Elements.js"
import * as ClientItems from "../client/Items.js"

import '../../node_modules/chai/register-assert.js';
declare var assert: Chai.AssertStatic;

export function includeTests() {
    describe('Client', function () {
        describe('Elements', function () {
            it('Test Temporary Element', async () => {
                const result = await ClientElements.createTemporaryElement();
                assert.isTrue(result.success, "Element was not successfully created");
                
                const uri = result.uri;
                assert.isTrue(uri !== undefined && uri !== null && uri !== "", "Uri was not set");
                
                await ClientItems.setProperty(result.workspace, uri, "name", "Test");
                
                const property = await ClientItems.getProperty(result.workspace, uri, "name");
                assert.isTrue(property === "Test", "Property could not be set correctly");                
            });

            it('Test Temporary Element with MetaClass', async () => {
                const result = await ClientElements.createTemporaryElement("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
                assert.isTrue(result.success, "Element was not successfully created");

                const uri = result.uri;
                assert.isTrue(uri !== undefined && uri !== null && uri !== "", "Uri was not set");
                
                const element = await ClientItems.getObjectByUri(result.workspace, uri);
                assert.isTrue(element !== undefined, "Element is unexpectedly null");
                assert.isTrue(element.metaClass !== undefined, "Element.metaClass is unexpectedly null");
                assert.isTrue(element.metaClass.uri === "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
            });
        });
    });
}