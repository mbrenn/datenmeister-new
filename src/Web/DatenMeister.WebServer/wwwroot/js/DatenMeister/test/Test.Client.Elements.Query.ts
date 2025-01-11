import * as Mof from "../Mof.js";
import * as ClientElements from "../client/Elements.js"
import { _DatenMeister } from "../models/DatenMeister.class.js";
import { _UML } from "../models/uml.js"
import * as Query from "../modules/QueryEngine.js"

export function includeTests() {
    describe('Client', function () {
        describe('Elements.Query', function () {
            it('Querying Elements - API Check', async () => {

                // We are just querying the temporary extent and verify that the result is not null
                var query = new Query.QueryBuilder();
                var extentViewNode = Query.getElementsOfExtent(query, "Data", "dm:///_internal/temp");
                chai.expect(extentViewNode).to.not.be.null;

                var result = await ClientElements.queryObject(query.queryStatement);
                chai.expect(result).to.not.be.null;
                chai.expect(result.result).to.not.be.null;
                chai.expect(Array.isArray(result.result)).to.be.true;
            });

            it('Query elements of extent', async () => {
                // Loads the extent from dm:///_internal/types/internal and check that there are items within
                var query = new Query.QueryBuilder();
                var extentViewNode = Query.getElementsOfExtent(query, "Types", "dm:///_internal/types/internal");
                chai.expect(extentViewNode).to.not.be.null;

                var result = await ClientElements.queryObject(query.queryStatement);

                // First, expect that result.results has at least two items
                chai.expect(result.result.length > 2).to.be.true;

                // Second, expect that at least one package is called DatenMeister
                var found = null;
                for (var i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "DatenMeister") {
                        found = result.result[i];
                        break;
                    }
                }

                chai.expect(found).to.not.be.null;
                chai.expect((found as Mof.DmObject).metaClass.name == "Package");

                // Checks, that LoadExtentAction as deep element is not found
                var found = null;
                for (var i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "LoadExtentAction") {
                        found = result.result[i];
                        break;
                    }
                }

                chai.expect(found).to.be.null;
            });

            it('Query elements of extent with flattening', async () => {
                // Loads the extent from dm:///_internal/types/internal and check that there are items within
                var query = new Query.QueryBuilder();
                var extentViewNode = Query.getElementsOfExtent(query, "Types", "dm:///_internal/types/internal");
                var flattenNode = Query.flatten(query);
                chai.expect(extentViewNode).to.not.be.null;
                chai.expect(flattenNode).to.not.be.null;

                var result = await ClientElements.queryObject(query.queryStatement);

                // First, expect that result.results has at least two items
                chai.expect(result.result.length > 2).to.be.true;

                // Second, expect that at least one package is called LoadExtentAction
                var found = null;
                for (var i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "LoadExtentAction") {
                        found = result.result[i];
                        break;
                    }
                }

                chai.expect(found).to.not.be.null;
                chai.expect((found as Mof.DmObject).metaClass.name == "Class");


                // Second, expect that at least one package is called DatenMeister
                var found = null;
                for (var i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "DatenMeister") {
                        found = result.result[i];
                        break;
                    }
                }

                chai.expect(found).to.not.be.null;
                chai.expect((found as Mof.DmObject).metaClass.name == "Package");
            });

            it('Query elements by just looking for flatted classes', async () => {
                // Loads the extent from dm:///_internal/types/internal and check that there are items within
                var query = new Query.QueryBuilder();
                var extentViewNode = Query.getElementsOfExtent(query, "Types", "dm:///_internal/types/internal");
                var flattenNode = Query.flatten(query);
                var filterNode = Query.filterByMetaClass(query, Mof.DmObject.createFromReference("UML", _UML._StructuredClassifiers.__Class_Uri), false);

                chai.expect(extentViewNode).to.not.be.null;
                chai.expect(flattenNode).to.not.be.null;
                chai.expect(filterNode).to.not.be.null;

                var result = await ClientElements.queryObject(query.queryStatement);

                // First, expect that result.results has at least two items
                chai.expect(result.result.length > 2).to.be.true;

                // Second, expect that at least one package is called Actions
                var found = null;
                for (var i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "LoadExtentAction") {
                        found = result.result[i];
                        break;
                    }
                }

                chai.expect(found).to.not.be.null;
                chai.expect((found as Mof.DmObject).metaClass.name == "Class");

                // Second, expect that DatenMeister Package is filtered
                var found = null;
                for (var i = 0; i < result.result.length; i++) {
                    if (result.result[i].get("name", Mof.ObjectType.String) === "DatenMeister") {
                        found = result.result[i];
                        break;
                    }
                }

                chai.expect(found).to.be.null;
            });
        });
    });
}