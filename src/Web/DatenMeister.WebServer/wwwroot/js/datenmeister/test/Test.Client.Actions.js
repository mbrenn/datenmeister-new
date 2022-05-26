var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Actions", "../Mof"], function (require, exports, ClientActions, Mof) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Actions', function () {
                return __awaiter(this, void 0, void 0, function* () {
                    it('Success Echo', () => __awaiter(this, void 0, void 0, function* () {
                        const parameter = new Mof.DmObject();
                        parameter.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction");
                        parameter.set('shallSuccess', 'OK');
                        const result = yield ClientActions.executeAction("Execute", { parameter: parameter });
                        chai.assert.isTrue(result.success === true);
                    }));
                    it('No success Echo', () => __awaiter(this, void 0, void 0, function* () {
                        const parameter = new Mof.DmObject();
                        parameter.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction");
                        parameter.set('shallSuccess', 'NO');
                        const result = yield ClientActions.executeAction("Execute", { parameter: parameter });
                        chai.assert.isTrue(result.success === false);
                    }));
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Actions.js.map