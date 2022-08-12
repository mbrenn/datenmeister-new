var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Types", "../models/DatenMeister.class"], function (require, exports, ClientTypes, DatenMeister_class_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Types', function () {
                it('Get all Types', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        const result = yield ClientTypes.getAllTypes();
                        chai.assert.isTrue(result.length > 0);
                    });
                });
                it('Get Property Type', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        let result = yield ClientTypes.getPropertyType(DatenMeister_class_1._DatenMeister._CommonTypes._OSIntegration.__CommandLineApplication_Uri, DatenMeister_class_1._DatenMeister._CommonTypes._OSIntegration._CommandLineApplication._name_);
                        chai.assert.isTrue(result.id === "String");
                        result = yield ClientTypes.getPropertyType(DatenMeister_class_1._DatenMeister._DynamicRuntimeProvider.__DynamicRuntimeLoaderConfig_Uri, DatenMeister_class_1._DatenMeister._DynamicRuntimeProvider._DynamicRuntimeLoaderConfig.configuration);
                        chai.assert.isTrue(result === undefined);
                    });
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Types.js.map