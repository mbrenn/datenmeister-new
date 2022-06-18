var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Elements", "../client/Items"], function (require, exports, ClientElements, ClientItems) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Elements', function () {
                it('Test Temporary Element', () => __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientElements.createTemporaryElement();
                    chai.assert.isTrue(result.success, "Element was not successfully created");
                    const uri = result.uri;
                    chai.assert.isTrue(uri !== undefined && uri !== null && uri !== "", "Uri was not set");
                    yield ClientItems.setProperty("Data", uri, "name", "Test");
                    const property = yield ClientItems.getProperty("Data", uri, "name");
                    chai.assert.isTrue(property === "Test", "Property could not be set correctly");
                }));
                it('Test Temporary Element with MetaClass', () => __awaiter(this, void 0, void 0, function* () {
                    const result = yield ClientElements.createTemporaryElement("dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
                    chai.assert.isTrue(result.success, "Element was not successfully created");
                    const uri = result.uri;
                    chai.assert.isTrue(uri !== undefined && uri !== null && uri !== "", "Uri was not set");
                    const element = yield ClientItems.getObjectByUri("Data", uri);
                    chai.assert.isTrue(element !== undefined, "Element is unexpectedly null");
                    chai.assert.isTrue(element.metaClass !== undefined, "Element.metaClass is unexpectedly null");
                    chai.assert.isTrue(element.metaClass.uri === "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
                }));
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Elements.js.map