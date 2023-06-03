import * as MofTests from "./MofTests.js"
import * as ClientWorkspace from "./Test.Client.Workspace.js"
import * as ClientExtent from "./Test.Client.Extents.js"
import * as ClientItems from "./Test.Client.Items.js"
import * as ClientTypes from "./Test.Client.Types.js"
import * as ClientElements from "./Test.Client.Elements.js"
import * as ClientActions from "./Test.Client.Actions.js"
import * as ClientSync from "./Test.Client.Sync.js"
import * as ClientForms from "./Test.Client.Forms.js"
import * as ClientActionsItems from "./Test.Client.Actions.Item.js"
import * as ControlSelectItemControl from "./Test.Controls.SelectItemControl.js"
import * as Forms from "./Test.Forms.js"
import * as ViewModeLogic from "./Test.Forms.ViewModeLogic.js"
import * as UserEvents from "../../burnsystems/tests/events.test.js"

export function includeTests()
{
    UserEvents.includeTests();
    MofTests.includeTests();
    ClientWorkspace.includeTests();
    ClientExtent.includeTests();
    ClientItems.includeTests();
    ClientTypes.includeTests();
    ClientElements.includeTests();
    ClientForms.includeTests();
    ClientActions.includeTests();
    ClientActionsItems.includeTests();
    ClientSync.includeTests();
    ControlSelectItemControl.includeTests();
    ViewModeLogic.includeTests();
    Forms.includeTests();
}