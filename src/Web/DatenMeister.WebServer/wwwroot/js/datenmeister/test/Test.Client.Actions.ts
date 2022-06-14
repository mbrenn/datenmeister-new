import * as ClientActions from "../client/Actions"
import * as ClientExtent from "../client/Extents"
import * as Mof from "../Mof";
import * as DatenMeisterModel from "../models/DatenMeister.class"
import {_DatenMeister} from "../models/DatenMeister.class";
import _InMemoryLoaderConfig = _DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig;
import _LoadExtentAction = _DatenMeister._Actions._LoadExtentAction;
import _DropExtentAction = _DatenMeister._Actions._DropExtentAction;

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
            
            it('Create Extent via Action', async()=>{
                
                let success = await ClientExtent.exists("Data", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === false);
                
                const parameter = new Mof.DmObject();                
                parameter.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__LoadExtentAction_Uri);

                const configuration = new Mof.DmObject();
                configuration.setMetaClassByUri(DatenMeisterModel._DatenMeister._ExtentLoaderConfigs.__InMemoryLoaderConfig_Uri);
                configuration.set(_InMemoryLoaderConfig.extentUri, "dm:///unittestaction");
                configuration.set(_InMemoryLoaderConfig._name_, "UnitTest");
                parameter.set(_LoadExtentAction.configuration, configuration);
                
                await ClientActions.executeAction("Execute", {parameter: parameter});

                success = await ClientExtent.exists("Data", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === true);


                const drop = new Mof.DmObject();
                drop.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__DropExtentAction_Uri);
                drop.set(_DropExtentAction.workspace, "Data");
                drop.set(_DropExtentAction.extentUri, "dm:///unittestaction");

                await ClientActions.executeAction("Execute", {parameter: drop});

                success = await ClientExtent.exists("Data", "dm:///unittestaction");
                chai.assert.isTrue(success.exists === false);
            });                
        });
    });
}