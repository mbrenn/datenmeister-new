import * as MofTests from "./MofTests"
import * as ClientWorkspace from "./Test.Client.Workspace"
import * as ClientExtent from "./Test.Client.Extents"
import * as ClientItems from "./Test.Client.Items"
import * as ControlSelectItemControl from "./Test.Controls.SelectItemControl"
import * as Forms from "./Test.Forms"
import * as UserEvents from "../../burnsystems/tests/events.test"

export function includeTests()
{
    UserEvents.includeTests();
    MofTests.includeTests();
    ClientWorkspace.includeTests();
    ClientExtent.includeTests();
    ClientItems.includeTests();
    ControlSelectItemControl.includeTests();
    Forms.includeTests();
}
