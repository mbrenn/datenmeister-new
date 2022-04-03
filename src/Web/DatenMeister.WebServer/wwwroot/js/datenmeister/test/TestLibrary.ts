import * as MofTests from "./MofTests"
import * as ClientWorkspace from "./Test.Client.Workspace"
import * as ClientExtent from "./Test.Client.Extents"
import * as ClientItems from "./Test.Client.Items"

export function includeTests()
{
    MofTests.includeTests();
    ClientWorkspace.includeTests();
    ClientExtent.includeTests();
    ClientItems.includeTests();
}
