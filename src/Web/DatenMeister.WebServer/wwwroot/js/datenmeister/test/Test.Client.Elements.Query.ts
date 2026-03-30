import * as Mof from "../Mof.js";
import * as ClientElements from "../client/Elements.js"
import { _UML } from "../models/uml.js"
import * as Query from "../modules/QueryEngine.js"


import '../../node_modules/chai/register-assert.js';
import '../../node_modules/chai/register-expect.js';
declare var assert: Chai.AssertStatic;
declare var expect: Chai.ExpectStatic;

export function includeTests() {
    describe('Client', function () {
        describe('Elements.Query', function () {
            it('Querying Elements - API Check', async () => {

                // We are just querying the temporary extent and verify that the result is not null
                const query = new Query.QueryBuilder();
                const extentViewNode = Query.getElementsOfExtent(query, "TemporaryData", "dm:///_internal/temp");
                expect(extentViewNode).to.not.be.null;

                const result = await ClientElements.queryObject(query.queryStatement);
                expect(result).to.not.be.null;
                expect(result.result).to.not.be.null;
                expect(Array.isArray(result.result)).to.be.true;
            });

            it('Query elements of extent', async () => {
                let i;
// Loads the extent from dm:///_internal/types/internal and check that there are items within
                const query = new Query.QueryBuilder();
                const extentViewNode = Query.getElementsOfExtent(query, "Types", "dm:///_internal/types/internal");
                expect(extentViewNode).to.not.be.null;

                const result = await ClientElements.queryObject(query.queryStatement);

                // First, expect that result.results has at least two items
                expect(result.result.length > 2).to.be.true;

                // Second, expect that at least one package is called DatenMeister
                let found = null;
                for (i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "DatenMeister") {
                        found = result.result[i];
                        break;
                    }
                }

                expect(found).to.not.be.null;
                expect((found as Mof.DmObject).metaClass.name == "Package");

                // Checks, that LoadExtentAction as deep element is not found
                found = null;
                for (i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "LoadExtentAction") {
                        found = result.result[i];
                        break;
                    }
                }

                expect(found).to.be.null;
            });

            it('Query elements of extent with flattening', async () => {
                let i;
// Loads the extent from dm:///_internal/types/internal and check that there are items within
                const query = new Query.QueryBuilder();
                const extentViewNode = Query.getElementsOfExtent(query, "Types", "dm:///_internal/types/internal");
                const flattenNode = Query.flatten(query);
                expect(extentViewNode).to.not.be.null;
                expect(flattenNode).to.not.be.null;

                const result = await ClientElements.queryObject(query.queryStatement);

                // First, expect that result.results has at least two items
                expect(result.result.length > 2).to.be.true;

                // Second, expect that at least one package is called LoadExtentAction
                let found = null;
                for (i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "LoadExtentAction") {
                        found = result.result[i];
                        break;
                    }
                }

                expect(found).to.not.be.null;
                expect((found as Mof.DmObject).metaClass.name == "Class");


                // Second, expect that at least one package is called DatenMeister
                found = null;
                for (i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "DatenMeister") {
                        found = result.result[i];
                        break;
                    }
                }

                expect(found).to.not.be.null;
                expect((found as Mof.DmObject).metaClass.name == "Package");
            });

            it('Query elements by just looking for flatted classes', async () => {
                let i;
// Loads the extent from dm:///_internal/types/internal and check that there are items within
                const query = new Query.QueryBuilder();
                const extentViewNode = Query.getElementsOfExtent(query, "Types", "dm:///_internal/types/internal");
                const flattenNode = Query.flatten(query);
                const filterNode = Query.filterByMetaClass(query, Mof.DmObject.createFromReference("UML", _UML._StructuredClassifiers.__Class_Uri), false);

                expect(extentViewNode).to.not.be.null;
                expect(flattenNode).to.not.be.null;
                expect(filterNode).to.not.be.null;

                const result = await ClientElements.queryObject(query.queryStatement);

                // First, expect that result.results has at least two items
                expect(result.result.length > 2).to.be.true;

                // Second, expect that at least one package is called Actions
                let found = null;
                for (i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "LoadExtentAction") {
                        found = result.result[i];
                        break;
                    }
                }

                expect(found).to.not.be.null;
                expect((found as Mof.DmObject).metaClass.name == "Class");

                // Second, expect that DatenMeister Package is filtered
                found = null;
                for (i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "DatenMeister") {
                        found = result.result[i];
                        break;
                    }
                }

                expect(found).to.be.null;
            });
        });
    });
}