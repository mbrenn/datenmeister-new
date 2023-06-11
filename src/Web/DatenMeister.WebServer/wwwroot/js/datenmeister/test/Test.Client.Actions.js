import * as ClientActions from "../client/Actions.js";
import * as ClientExtent from "../client/Extents.js";
import * as Mof from "../Mof.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
var _InMemoryLoaderConfig = _DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig;
var _LoadExtentAction = _DatenMeister._Actions._LoadExtentAction;
var _DropExtentAction = _DatenMeister._Actions._DropExtentAction;
export function includeTests() {
    describe('Client', function () {
        describe('Actions', async function () {
            it('Success Echo', async () => {
                const parameter = new Mof.DmObject();
                parameter.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction", 'Types');
                parameter.set('shallSuccess', 'OK');
                const result = await ClientActions.executeActionDirectly("Execute", { parameter: parameter });
                chai.assert.isTrue(result.success === true);
            });
            it('No success Echo', async () => {
                const parameter = new Mof.DmObject();
                parameter.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction", 'Types');
                parameter.set('shallSuccess', 'NO');
                const result = await ClientActions.executeActionDirectly("Execute", { parameter: parameter });
                chai.assert.isTrue(result.success === false);
            });
            it('Create Extent via Action', async () => {
                let success = await ClientExtent.exists("Data", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === false);
                const parameter = new Mof.DmObject();
                parameter.setMetaClassByUri(_DatenMeister._Actions.__LoadExtentAction_Uri, 'Types');
                const configuration = new Mof.DmObject();
                configuration.setMetaClassByUri(_DatenMeister._ExtentLoaderConfigs.__InMemoryLoaderConfig_Uri, 'Types');
                configuration.set(_InMemoryLoaderConfig.extentUri, "dm:///unittestaction");
                configuration.set(_InMemoryLoaderConfig._name_, "UnitTest");
                parameter.set(_LoadExtentAction.configuration, configuration);
                await ClientActions.executeActionDirectly("Execute", { parameter: parameter });
                success = await ClientExtent.exists("Data", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === true);
                const drop = new Mof.DmObject();
                drop.setMetaClassByUri(_DatenMeister._Actions.__DropExtentAction_Uri, 'Types');
                drop.set(_DropExtentAction.workspace, "Data");
                drop.set(_DropExtentAction.extentUri, "dm:///unittestaction");
                await ClientActions.executeActionDirectly("Execute", { parameter: drop });
                success = await ClientExtent.exists("Data", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === false);
            });
        });
    });
}
//# sourceMappingURL=Test.Client.Actions.js.map