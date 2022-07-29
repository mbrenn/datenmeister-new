define(["require", "exports", "../forms/ViewModeLogic"], function (require, exports, VML) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        describe('ViewModeLogic', () => {
            it('Clear, get, set, get, clear, get', () => {
                VML.clearCurrentViewMode();
                let viewMode = VML.getCurrentViewMode();
                chai.assert.isTrue(viewMode === "Default", "viewMode needs to be 'Default'");
                VML.setCurrentViewMode('Test');
                viewMode = VML.getCurrentViewMode();
                chai.assert.isTrue(viewMode === "Test", "viewMode needs to be 'Test'");
                VML.clearCurrentViewMode();
                viewMode = VML.getCurrentViewMode();
                chai.assert.isTrue(viewMode === "Default", "viewMode needs to be 'Default'");
            });
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=Test.Forms.ViewModeLogic.js.map