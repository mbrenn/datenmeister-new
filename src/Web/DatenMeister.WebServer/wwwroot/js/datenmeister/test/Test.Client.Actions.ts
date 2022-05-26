import * as ClientActions from "../client/Actions"
import * as Mof from "../Mof";

export function includeTests() {
    describe('Client', function () {
        describe('Actions', async function () {
            it('Success Echo', async () => {
                const parameter = new Mof.DmObject();
                parameter.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction")
                parameter.set('shallSuccess', 'OK');

                const result = await ClientActions.executeAction(
                    "Execute",
                    {parameter: parameter}
                );

                chai.assert.isTrue(result.success === true);
            });

            it('No success Echo', async () => {
                const parameter = new Mof.DmObject();
                parameter.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction")
                parameter.set('shallSuccess', 'NO');

                const result = await ClientActions.executeAction(
                    "Execute",
                    {parameter: parameter}
                );

                chai.assert.isTrue(result.success === false);
            });
        });
    });
}
