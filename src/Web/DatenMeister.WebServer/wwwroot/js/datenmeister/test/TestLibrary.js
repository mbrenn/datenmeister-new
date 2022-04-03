define(["require", "exports", "./MofTests", "./Test.Client.Workspace", "./Test.Client.Extents", "./Test.Client.Items"], function (require, exports, MofTests, ClientWorkspace, ClientExtent, ClientItems) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        MofTests.includeTests();
        ClientWorkspace.includeTests();
        ClientExtent.includeTests();
        ClientItems.includeTests();
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=TestLibrary.js.map