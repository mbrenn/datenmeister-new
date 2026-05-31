import * as ClientTypes from "../client/Types.js"
import * as _DatenMeister from "../models/DatenMeister.class.js";

import '../../node_modules/chai/register-assert.js';
declare var assert: Chai.AssertStatic;

export function includeTests() {
    describe('Client', function () {
        describe('Types', function () {
            it('Get all Types', async function () {
                const result = await ClientTypes.getAllTypes();
                assert.isTrue(result.length > 0);
            });

            it('Get Property Type', async function () {
                let result = await ClientTypes.getPropertyType(
                    "Types",
                    _DatenMeister._CommonTypes._OSIntegration.__CommandLineApplication_Uri,
                    _DatenMeister._CommonTypes._OSIntegration._CommandLineApplication._name_);

                assert.isTrue(result.id === "String");


                result = await ClientTypes.getPropertyType(
                    "Types",
                    _DatenMeister._DynamicRuntimeProvider.__DynamicRuntimeLoaderConfig_Uri,
                    _DatenMeister._DynamicRuntimeProvider._DynamicRuntimeLoaderConfig.configuration);

                assert.isTrue(result === undefined);
            });
        });
    });
}