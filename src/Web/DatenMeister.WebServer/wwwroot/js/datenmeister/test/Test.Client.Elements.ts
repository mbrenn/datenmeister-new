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
        });
    });
}