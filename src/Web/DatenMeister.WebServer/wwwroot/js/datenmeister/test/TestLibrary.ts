import * as MofTests from "./MofTests"
import * as ClientWorkspace from "./Test.Client.Workspace"
import * as ClientExtent from "./Test.Client.Extents"
import * as ClientItems from "./Test.Client.Items"
import * as ClientElements from "./Test.Client.Elements"
import * as ClientActions from "./Test.Client.Actions"
import * as ControlSelectItemControl from "./Test.Controls.SelectItemControl"
import * as Forms from "./Test.Forms"
import * as ViewModeLogic from "./Test.Forms.ViewModeLogic"
import * as UserEvents from "../../burnsystems/tests/events.test"

export function includeTests()
{
    UserEvents.includeTests();
    MofTests.includeTests();
    ClientWorkspace.includeTests();
    ClientExtent.includeTests();
    ClientItems.includeTests();
    ClientElements.includeTests();
    ClientActions.includeTests();
    ControlSelectItemControl.includeTests();
    ViewModeLogic.includeTests();
    Forms.includeTests();
}