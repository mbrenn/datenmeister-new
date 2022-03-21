define(["require", "exports", "./MofTests", "./Test.Client.Workspace", "./Test.Client.Extents"], function (require, exports, MofTests, ClientWorkspace, ClientExtent) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        MofTests.includeTests();
        ClientWorkspace.includeTests();
        ClientExtent.includeTests();
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=TestLibrary.js.map