import * as MofTests from "./Test.Mof.js";
import * as TableState from "./Test.TableState.js";
import * as StaticObjectQuery from "./Test.StaticObjectQuery.js";
import * as UserEvents from "./events.test.js";

export function includeTests() {
    UserEvents.includeTests();
    MofTests.includeTests();
    TableState.includeTests();
    StaticObjectQuery.includeTests();
}

includeTests();
