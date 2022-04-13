var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Workspace", "../client/Extents", "../client/Items", "../controls/SelectItemControl"], function (require, exports, ClientWorkspace, ClientExtent, ClientItems, SelectItemControl_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('Controls', () => {
            before(function () {
                return __awaiter(this, void 0, void 0, function* () {
                    yield ClientWorkspace.createWorkspace("Test", "Annotation", { skipIfExisting: true });
                    yield ClientExtent.createXmi({
                        extentUri: "dm:///unittest",
                        filePath: "./unittest.xmi",
                        workspace: "Test",
                        skipIfExisting: true
                    });
                    yield ClientItems.deleteRootElements("Test", "dm:///unittest");
                });
            });
            describe('SelectItemControl', () => {
                it('Create Controls and Load Workspaces', () => __awaiter(this, void 0, void 0, function* () {
                    let div = $("#dom_test");
                    if (div === undefined) {
                        div = $("<div></div>");
                    }
                    const sic = new SelectItemControl_1.SelectItemControl();
                    const query = yield sic.initAsync(div);
                    const workspace = $(".dm-sic-workspace select", query);
                    chai.assert.isTrue(workspace !== undefined, "No select given");
                    const children = workspace.children();
                    let found = false;
                    children.each((index, child) => {
                        if ($(child).text() === "Test") {
                            found = true;
                        }
                    });
                    chai.assert.isTrue(found, "Workspace was not found");
                    div.remove();
                }));
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Controls.SelectItemControl.js.map