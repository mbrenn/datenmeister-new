var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Client.Extents", "../Client.Workspace"], function (require, exports, ClientExtent, ClientWorkspace) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Client', function () {
            describe('Items', function () {
                before(function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true });
                        yield ClientExtent.createXmi({
                            extentUri: "dm:///unittest",
                            filePath: "./unittest.xmi",
                            workspace: "Test",
                            skipIfExisting: true
                        });
                    });
                });
                it('Create and Delete Item', function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        try {
                        }
                        catch (e) {
                            throw e;
                        }
                    });
                });
                after(function () {
                    return __awaiter(this, void 0, void 0, function* () {
                        yield ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///unittest",
                            skipIfNotExisting: true
                        });
                        yield ClientExtent.deleteExtent({
                            workspace: "Test",
                            extentUri: "dm:///newexisting",
                            skipIfNotExisting: true
                        });
                        yield ClientWorkspace.deleteWorkspace("Test");
                    });
                });
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Client.Items.js.map