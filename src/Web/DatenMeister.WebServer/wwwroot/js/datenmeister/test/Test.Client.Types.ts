import * as ClientTypes from "../client/Types"
import {_DatenMeister} from "../models/DatenMeister.class"

export function includeTests() {
    describe('Client', function () {
        describe('Types', function () {
            it('Get all Types', async function () {
                const result = await ClientTypes.getAllTypes();
                chai.assert.isTrue(result.length > 0);
            });

            it('Get Property Type', async function () {
                let result = await ClientTypes.getPropertyType(
                    _DatenMeister._CommonTypes._OSIntegration.__CommandLineApplication_Uri,
                    _DatenMeister._CommonTypes._OSIntegration._CommandLineApplication._name_);

                chai.assert.isTrue(result.id === "String");


                result = await ClientTypes.getPropertyType(
                    _DatenMeister._DynamicRuntimeProvider.__DynamicRuntimeLoaderConfig_Uri,
                    _DatenMeister._DynamicRuntimeProvider._DynamicRuntimeLoaderConfig.configuration);

                chai.assert.isTrue(result === undefined);
            });
        });
    });
}