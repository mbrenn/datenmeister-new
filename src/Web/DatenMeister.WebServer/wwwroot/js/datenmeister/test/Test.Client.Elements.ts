import * as ClientElements from "../client/Elements"
import * as ClientItems from "../client/Items"

export function includeTests() {
    describe('Client', function () {
        describe('Elements', function () {
            it('Test Temporary Element', async () =>{
                const result = await ClientElements.createTemporaryElement();
                chai.assert.isTrue(result.success, "Element was not successfully created");
                
                const uri = result.uri;
                chai.assert.isTrue(uri !== undefined && uri !== null && uri !== "", "Uri was not set");
                
                await ClientItems.setProperty("Data", uri, "name", "Test");
                
                const property = await ClientItems.getProperty("Data", uri, "name");
                chai.assert.isTrue(property === "Test", "Property could not be set correctly");                
            });

            it('Test Temporary Element with MetaClass', async () =>{
                const result = await ClientElements.createTemporaryElement("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
                chai.assert.isTrue(result.success, "Element was not successfully created");

                const uri = result.uri;
                chai.assert.isTrue(uri !== undefined && uri !== null && uri !== "", "Uri was not set");
                
                const element = await ClientItems.getObjectByUri("Data", uri);
                chai.assert.isTrue(element !== undefined, "Element is unexpectedly null");
                chai.assert.isTrue(element.metaClass !== undefined, "Element.metaClass is unexpectedly null");
                chai.assert.isTrue(element.metaClass.uri === "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
            });
        });
    });
}