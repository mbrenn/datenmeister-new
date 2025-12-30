import * as ClientElements from "../client/Elements.js";
import * as ClientItems from "../client/Items.js";
import * as Mof from "../Mof.js";
import { sync } from "../MofSync.js";
export function includeTests() {
    describe('Sync', function () {
        it('Create Temporary Object and do Sync without property update', async function () {
            const result = await ClientElements.createTemporaryElement();
            chai.assert.isTrue(result.success === true);
            chai.assert.isTrue(result.workspace !== undefined && result.workspace !== "");
            chai.assert.isTrue(result.uri !== undefined && result.uri !== "");
            const dmResult = Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
            await sync(dmResult);
        });
        it('Set Properties', async function () {
            const result = await ClientElements.createTemporaryElement();
            const dmResult = Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
            dmResult.set('test', 'testing');
            dmResult.set('number', 3);
            await sync(dmResult);
            const checkResult = await ClientItems.getObjectByUri(result.workspace, result.uri);
            chai.assert.isTrue(checkResult.get('test', Mof.ObjectType.String) === 'testing', 'String should be correct');
            chai.assert.isTrue(checkResult.get('number', Mof.ObjectType.Number) === 3, 'Number should be correct');
        });
        it('Set References', async function () {
            const result = await ClientElements.createTemporaryElement();
            const result2 = await ClientElements.createTemporaryElement();
            const dmResult = Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
            dmResult.set('test', result2);
            await sync(dmResult);
            const checkResult = await ClientItems.getObjectByUri(result.workspace, result.uri);
            const reference = checkResult.get('test', Mof.ObjectType.Object);
            chai.assert.isTrue(reference !== undefined, 'Reference should be set');
            chai.assert.isTrue(reference.uri === result2.uri, 'Uri of reference should be set');
        });
        it('Unset Properties', async function () {
            const result = await ClientElements.createTemporaryElement();
            const dmResult = Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
            dmResult.set('test', 'testing');
            await sync(dmResult);
            let checkResult = await ClientItems.getObjectByUri(result.workspace, result.uri);
            chai.assert.isTrue(checkResult.get('test', Mof.ObjectType.String) === 'testing', 'String should be set');
            chai.assert.isTrue(checkResult.isSet('test'), 'Should be set');
            dmResult.unset('test');
            await sync(dmResult);
            // Now the value should be undefined
            checkResult = await ClientItems.getObjectByUri(result.workspace, result.uri);
            chai.assert.isFalse(checkResult.isSet('test'), 'Should be not set');
            chai.assert.isTrue(checkResult.get('test') === undefined, 'Should be undefined');
        });
    });
}
//# sourceMappingURL=Test.Client.Sync.js.map