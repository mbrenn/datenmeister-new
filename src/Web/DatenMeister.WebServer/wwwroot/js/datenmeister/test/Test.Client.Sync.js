var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Elements", "../client/Items", "../Mof", "../MofSync"], function (require, exports, ClientElements, ClientItems, Mof, MofSync_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Sync', function () {
            it('Create Temporary Object and do Sync without property update', function () {
                return __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientElements.createTemporaryElement();
                    chai.assert.isTrue(result.success === true);
                    chai.assert.isTrue(result.workspace !== undefined && result.workspace !== "");
                    chai.assert.isTrue(result.uri !== undefined && result.uri !== "");
                    const dmResult = Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
                    yield (0, MofSync_1.sync)(dmResult);
                });
            });
            it('Set Properties', function () {
                return __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientElements.createTemporaryElement();
                    const dmResult = Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
                    dmResult.set('test', 'testing');
                    dmResult.set('number', 3);
                    yield (0, MofSync_1.sync)(dmResult);
                    const checkResult = yield ClientItems.getObjectByUri(result.workspace, result.uri);
                    chai.assert.isTrue(checkResult.get('test', Mof.ObjectType.String) === 'testing', 'String should be correct');
                    chai.assert.isTrue(checkResult.get('number', Mof.ObjectType.Number) === 3, 'Number should be correct');
                });
            });
            it('Set References', function () {
                return __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientElements.createTemporaryElement();
                    const result2 = yield ClientElements.createTemporaryElement();
                    const dmResult = Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
                    dmResult.set('test', result2);
                    yield (0, MofSync_1.sync)(dmResult);
                    const checkResult = yield ClientItems.getObjectByUri(result.workspace, result.uri);
                    const reference = checkResult.get('test', Mof.ObjectType.Object);
                    chai.assert.isTrue(reference !== undefined, 'Reference should be set');
                    chai.assert.isTrue(reference.uri === result2.uri, 'Uri of reference should be set');
                });
            });
            it('Unset Properties', function () {
                return __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientElements.createTemporaryElement();
                    const dmResult = Mof.DmObjectWithSync.createFromReference(result.workspace, result.uri);
                    dmResult.set('test', 'testing');
                    yield (0, MofSync_1.sync)(dmResult);
                    let checkResult = yield ClientItems.getObjectByUri(result.workspace, result.uri);
                    chai.assert.isTrue(checkResult.get('test', Mof.ObjectType.String) === 'testing', 'String should be set');
                    dmResult.unset('test');
                    yield (0, MofSync_1.sync)(dmResult);
                    // Now the value should be undefined
                    checkResult = yield ClientItems.getObjectByUri(result.workspace, result.uri);
                    chai.assert.isTrue(checkResult.get('test', Mof.ObjectType.Default) === undefined, 'Should be undefined');
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Sync.js.map