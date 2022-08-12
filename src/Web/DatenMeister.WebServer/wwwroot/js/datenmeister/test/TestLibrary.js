define(["require", "exports", "./MofTests", "./Test.Client.Workspace", "./Test.Client.Extents", "./Test.Client.Items", "./Test.Client.Types", "./Test.Client.Elements", "./Test.Client.Actions", "./Test.Controls.SelectItemControl", "./Test.Forms", "./Test.Forms.ViewModeLogic", "../../burnsystems/tests/events.test"], function (require, exports, MofTests, ClientWorkspace, ClientExtent, ClientItems, ClientTypes, ClientElements, ClientActions, ControlSelectItemControl, Forms, ViewModeLogic, UserEvents) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
        UserEvents.includeTests();
        MofTests.includeTests();
        ClientWorkspace.includeTests();
        ClientExtent.includeTests();
        ClientItems.includeTests();
        ClientTypes.includeTests();
        ClientElements.includeTests();
        ClientActions.includeTests();
        ControlSelectItemControl.includeTests();
        ViewModeLogic.includeTests();
        Forms.includeTests();
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=TestLibrary.js.map