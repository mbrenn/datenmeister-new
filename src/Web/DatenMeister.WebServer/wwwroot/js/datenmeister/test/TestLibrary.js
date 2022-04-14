define(["require", "exports", "./MofTests", "./Test.Client.Workspace", "./Test.Client.Extents", "./Test.Client.Items", "./Test.Controls.SelectItemControl", "./Test.Forms", "../../burnsystems/tests/events.test"], function (require, exports, MofTests, ClientWorkspace, ClientExtent, ClientItems, ControlSelectItemControl, Forms, UserEvents) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        UserEvents.includeTests();
        MofTests.includeTests();
        ClientWorkspace.includeTests();
        ClientExtent.includeTests();
        ClientItems.includeTests();
        ControlSelectItemControl.includeTests();
        Forms.includeTests();
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=TestLibrary.js.map