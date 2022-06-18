var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Actions", "../client/Extents", "../Mof", "../models/DatenMeister.class", "../models/DatenMeister.class"], function (require, exports, ClientActions, ClientExtent, Mof, DatenMeisterModel, DatenMeister_class_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    var _InMemoryLoaderConfig = DatenMeister_class_1._DatenMeister._ExtentLoaderConfigs._InMemoryLoaderConfig;
    var _LoadExtentAction = DatenMeister_class_1._DatenMeister._Actions._LoadExtentAction;
    var _DropExtentAction = DatenMeister_class_1._DatenMeister._Actions._DropExtentAction;
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
                    it('Create Extent via Action', () => __awaiter(this, void 0, void 0, function* () {
                        let success = yield ClientExtent.exists("Data", "dm:///unittestaction");
                        chai.assert.isTrue(success.exists === false);
                        const parameter = new Mof.DmObject();
                        parameter.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__LoadExtentAction_Uri);
                        const configuration = new Mof.DmObject();
                        configuration.setMetaClassByUri(DatenMeisterModel._DatenMeister._ExtentLoaderConfigs.__InMemoryLoaderConfig_Uri);
                        configuration.set(_InMemoryLoaderConfig.extentUri, "dm:///unittestaction");
                        configuration.set(_InMemoryLoaderConfig._name_, "UnitTest");
                        parameter.set(_LoadExtentAction.configuration, configuration);
                        yield ClientActions.executeAction("Execute", { parameter: parameter });
                        success = yield ClientExtent.exists("Data", "dm:///unittestaction");
                        chai.assert.isTrue(success.exists === true);
                        const drop = new Mof.DmObject();
                        drop.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__DropExtentAction_Uri);
                        drop.set(_DropExtentAction.workspace, "Data");
                        drop.set(_DropExtentAction.extentUri, "dm:///unittestaction");
                        yield ClientActions.executeAction("Execute", { parameter: drop });
                        success = yield ClientExtent.exists("Data", "dm:///unittestaction");
                        chai.assert.isTrue(success.exists === false);
                    }));
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Actions.js.map