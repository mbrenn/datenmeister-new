import * as MofTests from "./MofTests"
import * as ClientWorkspace from "./Test.Client.Workspace"
import * as ClientExtent from "./Test.Client.Extents"

export function includeTests()
{
    MofTests.includeTests();
    ClientWorkspace.includeTests();
    ClientExtent.includeTests();
}
